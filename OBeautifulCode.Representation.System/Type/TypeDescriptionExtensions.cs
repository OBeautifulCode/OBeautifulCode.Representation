// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDescriptionExtensions.cs" company="OBeautifulCode">
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
    /// Extensions to <see cref="TypeDescription"/>.
    /// </summary>
    public static class TypeDescriptionExtensions
    {
        private static readonly ConcurrentDictionary<TypeDescriptionCacheKey, Type> TypeDescriptionCacheKeyToTypeMap = new ConcurrentDictionary<TypeDescriptionCacheKey, Type>();

        /// <summary>
        /// Creates a new type description from a given type.
        /// </summary>
        /// <param name="type">Input type to use.</param>
        /// <returns>Type description describing input type.</returns>
        public static TypeDescription ToDescription(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            TypeDescription result;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                var genericArguments = type.GetGenericArguments();
                var genericArgumentDefinitions = genericArguments.Select(_ => _.ToDescription()).ToList();
                result = new TypeDescription(
                    genericType.Namespace,
                    genericType.Name,
                    genericType.AssemblyQualifiedName,
                    genericArgumentDefinitions);
            }
            else
            {
                result = new TypeDescription(
                    type.Namespace,
                    type.Name,
                    type.AssemblyQualifiedName,
                    new List<TypeDescription>());
            }

            return result;
        }

        /// <summary>
        /// Resolve the <see cref="TypeDescription" /> from the loaded types.
        /// </summary>
        /// <param name="typeDescription">Type description to look for.</param>
        /// <param name="typeMatchStrategy">Strategy to use for equality when matching.</param>
        /// <param name="multipleMatchStrategy">Strategy to use with collisions when matching.</param>
        /// <returns>Matched type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Want to swallow that specific exception.")]
        public static Type ResolveFromLoadedTypes(this TypeDescription typeDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { typeDescription }.Must().NotBeNull();

            Type result;

            var cacheKey = new TypeDescriptionCacheKey(typeDescription, typeMatchStrategy, multipleMatchStrategy);
            if (TypeDescriptionCacheKeyToTypeMap.ContainsKey(cacheKey))
            {
                result = TypeDescriptionCacheKeyToTypeMap[cacheKey];

                return result;
            }

            // first deal with special hack implementation of array types...
            if (typeDescription.Name.Contains("[]") || typeDescription.AssemblyQualifiedName.Contains("[]"))
            {
                var arrayItemTypeDescription = new TypeDescription
                {
                    AssemblyQualifiedName = typeDescription.AssemblyQualifiedName.Replace("[]", string.Empty),
                    Namespace = typeDescription.Namespace,
                    Name = typeDescription.Name.Replace("[]", string.Empty),
                };

                var arrayItemType = arrayItemTypeDescription.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);

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
                    TypeDescription description = null;

                    try
                    {
                        /* For types that have dependent assemblies that are not found on disk this will fail when it tries to get properties from the type.
                         * Added because we encountered a FileNotFoundException for an assembly that was not on disk when taking a loaded type and calling
                         * ToDescription on it (specifically it threw on the type.Namespace getter call).
                         */

                        description = _.ToDescription();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    if (description == null)
                    {
                        return false;
                    }

                    return typeComparer.Equals(description, typeDescription);
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

            TypeDescriptionCacheKeyToTypeMap.TryAdd(cacheKey, result);

            return result;
        }
    }
}
