// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System.Internal;
    using OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    /// <summary>
    /// Extensions to <see cref="TypeRepresentation"/>.
    /// </summary>
    public static class TypeRepresentationExtensions
    {
        private static readonly ConcurrentDictionary<TypeRepresentationCacheKey, Type>
            TypeRepresentationCacheKeyToTypeMap = new ConcurrentDictionary<TypeRepresentationCacheKey, Type>();

        /// <summary>
        /// Creates a new type representation from a given type.
        /// </summary>
        /// <param name="type">Input type to use.</param>
        /// <returns>Type representation describing input type.</returns>
        public static TypeRepresentation ToRepresentation(
            this Type type)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { type.ContainsGenericParameters }.AsArg().Must().BeFalse(); // can't be an open type

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
            else if (type.IsArray)
            {
                var arrayKind = type.GetArrayKind();

                if (arrayKind == ArrayKind.Vector)
                {
                    result = typeof(VectorArray<>).MakeGenericType(type.GetElementType()).ToRepresentation();
                }
                else if (arrayKind == ArrayKind.Multidimensional)
                {
                    var arrayRankType = type.GetArrayRank().GetArrayRankType();

                    result = typeof(MultidimensionalArray<,>).MakeGenericType(type.GetElementType(), arrayRankType).ToRepresentation();
                }
                else
                {
                    throw new NotSupportedException("This kind of array is not supported: " + arrayKind);
                }
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
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping all together.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Keeping all together.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Want to swallow that specific exception.")]
        public static Type ResolveFromLoadedTypes(
            this TypeRepresentation typeRepresentation,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { typeRepresentation }.AsArg().Must().NotBeNull();

            Type result;

            var cacheKey = new TypeRepresentationCacheKey(typeRepresentation, typeMatchStrategy, multipleMatchStrategy);

            if (TypeRepresentationCacheKeyToTypeMap.ContainsKey(cacheKey))
            {
                result = TypeRepresentationCacheKeyToTypeMap[cacheKey];

                return result;
            }

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

            var accumulatedReflectionTypeLoadExceptions = reflectionTypeLoadExceptions.Any()
                ? new AggregateException(Invariant($"Getting types from assemblies threw one or more {nameof(ReflectionTypeLoadException)}.  See inner exceptions."), reflectionTypeLoadExceptions)
                : null;

            allTypes = allTypes.Where(_ => _ != null).Distinct().ToList();

            var typeComparer = new TypeComparer(typeMatchStrategy);

            var typeHasGenericArguments = typeRepresentation.GenericArguments.Any();

            var allMatchingTypes = allTypes
                .Select(_ =>
                {
                    try
                    {
                        if (_.Name != typeRepresentation.Name)
                        {
                            return null;
                        }

                        Type typeToConsider;

                        if (typeHasGenericArguments
                            && _.IsGenericType
                            && _.IsGenericTypeDefinition
                            && (_.GetGenericArguments().Length == typeRepresentation.GenericArguments.Count))
                        {
                            var resolvedGenericArguments = typeRepresentation
                                .GenericArguments
                                .Select(a => a.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy))
                                .ToArray();

                            typeToConsider = _.MakeGenericType(resolvedGenericArguments);
                        }
                        else
                        {
                            typeToConsider = _;
                        }

                        var typeToConsiderRepresentation = typeToConsider.ToRepresentation();

                        if (typeComparer.Equals(typeRepresentation, typeToConsiderRepresentation))
                        {
                            if (typeToConsider.IsAssignableTo(typeof(VectorArray<>), treatGenericTypeDefinitionAsAssignableTo: true))
                            {
                                typeToConsider = typeToConsider.GetGenericArguments()[0].MakeArrayType();
                            }
                            else if (typeToConsider.IsAssignableTo(typeof(MultidimensionalArray<,>), treatGenericTypeDefinitionAsAssignableTo: true))
                            {
                                var multidimensionalArrayTypeArguments = typeToConsider.GetGenericArguments();

                                var arrayRank = multidimensionalArrayTypeArguments[1].GetArrayRankFromArrayRankType();

                                typeToConsider = multidimensionalArrayTypeArguments[0].MakeArrayType(arrayRank);
                            }

                            return typeToConsider;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        // For types that have dependent assemblies that are not found on disk this will fail when it tries to get properties from the type.
                        // Added because we encountered a FileNotFoundException for an assembly that was not on disk when taking a loaded type and calling
                        // ToRepresentation on it (specifically it threw on the type.Namespace getter call).
                        return null;
                    }
                })
                .Where(_ => _ != null)
                .ToList();

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

            TypeRepresentationCacheKeyToTypeMap.TryAdd(cacheKey, result);

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA_ALL_SeeOtherSuppressionMessages)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private static Type GetArrayRankType(
            this int rank)
        {
            switch (rank)
            {
                case 1:
                    return typeof(ArrayRank1);
                case 2:
                    return typeof(ArrayRank2);
                case 3:
                    return typeof(ArrayRank3);
                case 4:
                    return typeof(ArrayRank4);
                case 5:
                    return typeof(ArrayRank5);
                case 6:
                    return typeof(ArrayRank6);
                case 7:
                    return typeof(ArrayRank7);
                case 8:
                    return typeof(ArrayRank8);
                case 9:
                    return typeof(ArrayRank9);
                case 10:
                    return typeof(ArrayRank10);
                case 11:
                    return typeof(ArrayRank11);
                case 12:
                    return typeof(ArrayRank12);
                case 13:
                    return typeof(ArrayRank13);
                case 14:
                    return typeof(ArrayRank14);
                case 15:
                    return typeof(ArrayRank15);
                case 16:
                    return typeof(ArrayRank16);
                case 17:
                    return typeof(ArrayRank17);
                case 18:
                    return typeof(ArrayRank18);
                case 19:
                    return typeof(ArrayRank19);
                case 20:
                    return typeof(ArrayRank20);
                case 21:
                    return typeof(ArrayRank21);
                case 22:
                    return typeof(ArrayRank22);
                case 23:
                    return typeof(ArrayRank23);
                case 24:
                    return typeof(ArrayRank24);
                case 25:
                    return typeof(ArrayRank25);
                case 26:
                    return typeof(ArrayRank26);
                case 27:
                    return typeof(ArrayRank27);
                case 28:
                    return typeof(ArrayRank28);
                case 29:
                    return typeof(ArrayRank29);
                case 30:
                    return typeof(ArrayRank30);
                case 31:
                    return typeof(ArrayRank31);
                case 32:
                    return typeof(ArrayRank32);
                default:
                    throw new NotSupportedException(Invariant($"This rank is not supported: {rank}."));
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA_ALL_SeeOtherSuppressionMessages)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private static int GetArrayRankFromArrayRankType(
            this Type type)
        {
            if (type == typeof(ArrayRank1))
            {
                return 1;
            }
            else if (type == typeof(ArrayRank2))
            {
                return 2;
            }
            else if (type == typeof(ArrayRank3))
            {
                return 3;
            }
            else if (type == typeof(ArrayRank4))
            {
                return 4;
            }
            else if (type == typeof(ArrayRank5))
            {
                return 5;
            }
            else if (type == typeof(ArrayRank6))
            {
                return 6;
            }
            else if (type == typeof(ArrayRank7))
            {
                return 7;
            }
            else if (type == typeof(ArrayRank8))
            {
                return 8;
            }
            else if (type == typeof(ArrayRank9))
            {
                return 9;
            }
            else if (type == typeof(ArrayRank10))
            {
                return 10;
            }
            else if (type == typeof(ArrayRank11))
            {
                return 11;
            }
            else if (type == typeof(ArrayRank12))
            {
                return 12;
            }
            else if (type == typeof(ArrayRank13))
            {
                return 13;
            }
            else if (type == typeof(ArrayRank14))
            {
                return 14;
            }
            else if (type == typeof(ArrayRank15))
            {
                return 15;
            }
            else if (type == typeof(ArrayRank16))
            {
                return 16;
            }
            else if (type == typeof(ArrayRank17))
            {
                return 17;
            }
            else if (type == typeof(ArrayRank18))
            {
                return 18;
            }
            else if (type == typeof(ArrayRank19))
            {
                return 19;
            }
            else if (type == typeof(ArrayRank20))
            {
                return 20;
            }
            else if (type == typeof(ArrayRank21))
            {
                return 21;
            }
            else if (type == typeof(ArrayRank22))
            {
                return 22;
            }
            else if (type == typeof(ArrayRank23))
            {
                return 23;
            }
            else if (type == typeof(ArrayRank24))
            {
                return 24;
            }
            else if (type == typeof(ArrayRank25))
            {
                return 25;
            }
            else if (type == typeof(ArrayRank26))
            {
                return 26;
            }
            else if (type == typeof(ArrayRank27))
            {
                return 27;
            }
            else if (type == typeof(ArrayRank28))
            {
                return 28;
            }
            else if (type == typeof(ArrayRank29))
            {
                return 29;
            }
            else if (type == typeof(ArrayRank30))
            {
                return 30;
            }
            else if (type == typeof(ArrayRank31))
            {
                return 31;
            }
            else if (type == typeof(ArrayRank32))
            {
                return 32;
            }
            else
            {
                throw new NotSupportedException("This type is not supported: " + type.ToStringReadable());
            }
        }
    }
}
