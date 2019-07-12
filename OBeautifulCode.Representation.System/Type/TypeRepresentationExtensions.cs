// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Validation.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extensions to <see cref="TypeRepresentation"/>.
    /// </summary>
    public static class TypeRepresentationExtensions
    {
        private static readonly ConcurrentDictionary<TypeRepresentationCacheKey, Type> TypeRepresentationCacheKeyToTypeMap = new ConcurrentDictionary<TypeRepresentationCacheKey, Type>();

        /// <summary>
        /// Creates a new type representation from a given type.
        /// </summary>
        /// <param name="type">Input type to use.</param>
        /// <returns>Type representation describing input type.</returns>
        public static TypeRepresentation ToRepresentation(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            TypeRepresentation result;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                var genericArguments = type.GetGenericArguments();
                var genericArgumentDefinitions = genericArguments.Select(_ => _.ToRepresentation()).ToList();
                result = new TypeRepresentation(
                    genericType.Namespace,
                    genericType.Name,
                    genericType.AssemblyQualifiedName,
                    genericArgumentDefinitions);
            }
            else
            {
                result = new TypeRepresentation(
                    type.Namespace,
                    type.Name,
                    type.AssemblyQualifiedName,
                    new List<TypeRepresentation>());
            }

            return result;
        }

        /// <summary>
        /// Resolve the <see cref="TypeRepresentation" /> from the loaded types.
        /// </summary>
        /// <param name="typeRepresentation">Type representation to look for.</param>
        /// <param name="typeMatchStrategy">Strategy to use for equality when matching.</param>
        /// <param name="multipleMatchStrategy">Strategy to use with collisions when matching.</param>
        /// <returns>Matched type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Want to swallow that specific exception.")]
        public static Type ResolveFromLoadedTypes(this TypeRepresentation typeRepresentation, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { typeRepresentation }.Must().NotBeNull();

            Type result;

            var cacheKey = new TypeRepresentationCacheKey(typeRepresentation, typeMatchStrategy, multipleMatchStrategy);
            if (TypeRepresentationCacheKeyToTypeMap.ContainsKey(cacheKey))
            {
                result = TypeRepresentationCacheKeyToTypeMap[cacheKey];

                return result;
            }

            // first deal with special hack implementation of array types...
            if (typeRepresentation.Name.Contains("[]") || typeRepresentation.AssemblyQualifiedName.Contains("[]"))
            {
                var arrayItemTypeRepresentation = new TypeRepresentation
                {
                    AssemblyQualifiedName = typeRepresentation.AssemblyQualifiedName.Replace("[]", string.Empty),
                    Namespace = typeRepresentation.Namespace,
                    Name = typeRepresentation.Name.Replace("[]", string.Empty),
                };

                var arrayItemType = arrayItemTypeRepresentation.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);

                result = arrayItemType?.MakeArrayType();
            }
            else
            {
                // if it's not an array type then run normal logic
                var loadedAssemblies = AssemblyLoader.GetLoadedAssemblies().Distinct().ToList();
                var allTypes = new List<Type>();
                var reflectionTypeLoadExceptions = new List<ReflectionTypeLoadException>();
                foreach (var assembly in loadedAssemblies)
                {
                    try
                    {
                        allTypes.AddRange(new[] { assembly }.GetTypesFromAssemblies());
                    }
                    catch (TypeLoadException ex) when (ex.InnerException?.GetType() == typeof(ReflectionTypeLoadException))
                    {
                        var reflectionTypeLoadException = (ReflectionTypeLoadException)ex.InnerException;
                        allTypes.AddRange(reflectionTypeLoadException.Types);
                        reflectionTypeLoadExceptions.Add(reflectionTypeLoadException);
                    }
                }

                AggregateException accumulatedReflectionTypeLoadExceptions = reflectionTypeLoadExceptions.Any()
                    ? new AggregateException(Invariant($"Getting types from assemblies threw one or more {nameof(ReflectionTypeLoadException)}.  See inner exceptions."), reflectionTypeLoadExceptions)
                    : null;

                allTypes = allTypes.Where(_ => _ != null).Distinct().ToList();
                var typeComparer = new TypeComparer(typeMatchStrategy);
                var allMatchingTypes = allTypes.Where(_ =>
                {
                    TypeRepresentation representation = null;

                    try
                    {
                        /* For types that have dependent assemblies that are not found on disk this will fail when it tries to get properties from the type.
                         * Added because we encountered a FileNotFoundException for an assembly that was not on disk when taking a loaded type and calling
                         * ToRepresentation on it (specifically it threw on the type.Namespace getter call).
                         */

                        representation = _.ToRepresentation();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    if (representation == null)
                    {
                        return false;
                    }

                    return typeComparer.Equals(representation, typeRepresentation);
                }).ToList();

                switch (multipleMatchStrategy)
                {
                    case MultipleMatchStrategy.ThrowOnMultiple:
                        if (allMatchingTypes.Count > 1)
                        {
                            var message = "Found multiple versions and multiple match strategy was: " + multipleMatchStrategy;
                            var types = string.Join(",", allMatchingTypes.Select(_ => _.AssemblyQualifiedName + " at " + _.Assembly.CodeBase));
                            throw new InvalidOperationException(message + "; types found: " + types, accumulatedReflectionTypeLoadExceptions);
                        }
                        else
                        {
                            result = allMatchingTypes.SingleOrDefault();
                        }

                        break;
                    case MultipleMatchStrategy.NewestVersion:
                        result = allMatchingTypes.OrderByDescending(_ => (_.Assembly.GetName().Version ?? new Version(0, 0, 0, 1)).ToString()).FirstOrDefault();
                        break;
                    case MultipleMatchStrategy.OldestVersion:
                        result = allMatchingTypes.OrderBy(_ => (_.Assembly.GetName().Version ?? new Version(0, 0, 0, 1)).ToString()).FirstOrDefault();
                        break;
                    default:
                        throw new NotSupportedException("Multiple match strategy not supported: " + multipleMatchStrategy);
                }

                if ((accumulatedReflectionTypeLoadExceptions != null) && (result == null))
                {
                    throw accumulatedReflectionTypeLoadExceptions;
                }
            }

            TypeRepresentationCacheKeyToTypeMap.TryAdd(cacheKey, result);

            return result;
        }
    }
}
