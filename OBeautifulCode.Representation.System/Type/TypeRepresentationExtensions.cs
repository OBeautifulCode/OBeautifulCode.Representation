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
    using global::System.Linq;
    using global::System.Reflection;
    using global::System.Text.RegularExpressions;

    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    using ResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKey = global::System.Tuple<string, Type.VersionMatchStrategy, bool>;
    using ResolveFromLoadedTypesUsingTypeRepresentationCacheKey = global::System.Tuple<TypeRepresentation, Type.VersionMatchStrategy, bool>;

    /// <summary>
    /// Extensions to <see cref="TypeRepresentation"/>.
    /// </summary>
    public static class TypeRepresentationExtensions
    {
        private static readonly ConcurrentDictionary<ResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKey, Type> CachedResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKeyToTypeMap = new ConcurrentDictionary<ResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKey, Type>();

        private static readonly ConcurrentDictionary<ResolveFromLoadedTypesUsingTypeRepresentationCacheKey, Type> CachedResolveFromLoadedTypesUsingTypeRepresentationCacheKeyToTypeMap = new ConcurrentDictionary<ResolveFromLoadedTypesUsingTypeRepresentationCacheKey, Type>();

        private static readonly ConcurrentDictionary<string, TypeRepresentation> CachedAssemblyQualifiedNameToTypeRepresentationMap = new ConcurrentDictionary<string, TypeRepresentation>();

        private static readonly ConcurrentDictionary<Type, TypeRepresentation> CachedTypeToTypeRepresentationMap = new ConcurrentDictionary<Type, TypeRepresentation>();

        private static readonly ConcurrentDictionary<TypeRepresentation, string> CachedTypeRepresentationToAssemblyQualifiedNameMap = new ConcurrentDictionary<TypeRepresentation, string>();

        private static readonly Regex BeforeLastCommaRegex = new Regex(@"(.*?)(?=\,[^,]+$)", RegexOptions.Compiled);

        private static readonly Regex LastArrayIdentifierRegex = new Regex(@"\[\*?\,*\]$", RegexOptions.Compiled);

        private static readonly Regex AllGenericArgumentsRegex = new Regex(@"\[(.*)\]", RegexOptions.Compiled);

        private static readonly Regex ConvertDotNetAssemblyQualifiedNameToObcAssemblyQualifiedNameRegex = new Regex(", Culture=.*?, PublicKeyToken=[a-z0-9]*", RegexOptions.Compiled);

        private static readonly Regex IsGenericTypeRegex = new Regex(@"`\d+$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the type is an array type.
        /// </summary>
        /// <param name="typeRepresentation">The type representation.</param>
        /// <returns>
        /// true if the type is an array type, otherwise false.
        /// </returns>
        public static bool IsArray(
            this TypeRepresentation typeRepresentation)
        {
            if (typeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(typeRepresentation));
            }

            var result = typeRepresentation.Name.EndsWith("[]", StringComparison.Ordinal) ||
                         typeRepresentation.Name.EndsWith("[*]", StringComparison.Ordinal) ||
                         typeRepresentation.Name.EndsWith(",]", StringComparison.Ordinal);

            return result;
        }

        /// <summary>
        /// Determines whether this type is a generic type.
        /// </summary>
        /// <param name="typeRepresentation">The type representation.</param>
        /// <returns>
        /// true if the type is a generic type, otherwise false.
        /// </returns>
        public static bool IsGenericType(
            this TypeRepresentation typeRepresentation)
        {
            if (typeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(typeRepresentation));
            }

            var result = typeRepresentation.GenericArguments != null;

            return result;
        }

        /// <summary>
        /// Determines whether this type is a generic type definition.
        /// </summary>
        /// <param name="typeRepresentation">The type representation.</param>
        /// <returns>
        /// true if the type is a generic type definition, otherwise false.
        /// </returns>
        public static bool IsGenericTypeDefinition(
            this TypeRepresentation typeRepresentation)
        {
            if (typeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(typeRepresentation));
            }

            var result = typeRepresentation.IsGenericType() && (!typeRepresentation.GenericArguments.Any());

            return result;
        }

        /// <summary>
        /// Determines whether this type is a closed generic type.
        /// </summary>
        /// <param name="typeRepresentation">The type representation.</param>
        /// <returns>
        /// true if the type is a closed generic type, otherwise false.
        /// </returns>
        public static bool IsClosedGenericType(
            this TypeRepresentation typeRepresentation)
        {
            if (typeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(typeRepresentation));
            }

            var result = typeRepresentation.IsGenericType() && typeRepresentation.GenericArguments.Any();

            return result;
        }

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

            if (CachedTypeToTypeRepresentationMap.TryGetValue(type, out TypeRepresentation result))
            {
                return result;
            }

            if (type.ContainsGenericParameters)
            {
                // only generic type definition is supported for open types
                if (!type.IsGenericTypeDefinition)
                {
                    throw new ArgumentException(Invariant($"'{nameof(type.IsGenericTypeDefinition)}' is false"));
                }
            }

            var name = type.GetFullyNestedName();

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                var genericArgumentTypeRepresentations = new List<TypeRepresentation>();

                if (!type.IsGenericTypeDefinition)
                {
                    var genericArguments = type.GetGenericArguments();

                    genericArgumentTypeRepresentations = genericArguments.Select(_ => _.ToRepresentation()).ToList();
                }

                var assemblyName = genericTypeDefinition.Assembly.GetName();

                result = new TypeRepresentation(
                    genericTypeDefinition.Namespace,
                    name,
                    assemblyName.Name,
                    assemblyName.Version.ToString(),
                    genericArgumentTypeRepresentations);
            }
            else if (type.IsArray)
            {
                var elementType = type.GetElementType();

                // ReSharper disable once PossibleNullReferenceException
                while (elementType.IsArray)
                {
                    elementType = elementType.GetElementType();
                }

                IReadOnlyList<TypeRepresentation> genericArgumentTypeRepresentations = null;

                // note that an array of generic type definition is an open type, but not a generic type definition
                // so this method would throw at the very top and we would never get here for such a type
                if (elementType.IsGenericType)
                {
                    var genericArguments = elementType.GetGenericArguments();

                    genericArgumentTypeRepresentations = genericArguments.Select(_ => _.ToRepresentation()).ToArray();
                }

                var assemblyName = type.Assembly.GetName();

                result = new TypeRepresentation(
                    type.Namespace,
                    name,
                    assemblyName.Name,
                    assemblyName.Version.ToString(),
                    genericArgumentTypeRepresentations);
            }
            else
            {
                var assemblyName = type.Assembly.GetName();

                result = new TypeRepresentation(
                    type.Namespace,
                    name,
                    assemblyName.Name,
                    assemblyName.Version.ToString(),
                    null);
            }

            CachedTypeToTypeRepresentationMap.TryAdd(type, result);

            return result;
        }

        /// <summary>
        /// Resolve the assembly qualified name into a Type from the loaded Types.
        /// </summary>
        /// <param name="assemblyQualifiedName">The assembly qualified name.</param>
        /// <param name="assemblyVersionMatchStrategy">Strategy to use for matching assembly versions.</param>
        /// <param name="throwIfCannotResolve">
        /// Optional value indicating whether to throw an exception if the required assembly(ies) or the type(s) cannot be found
        /// (generics, having generic type arguments, may cause the specified type representation to encapsulate multiple assemblies and/or multiple types).
        /// Default is true.
        /// </param>
        /// <returns>
        /// Resolved/matching Type.
        /// </returns>
        public static Type ResolveFromLoadedTypes(
            this string assemblyQualifiedName,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion,
            bool throwIfCannotResolve = true)
        {
            if (assemblyQualifiedName == null)
            {
                throw new ArgumentNullException(nameof(assemblyQualifiedName));
            }

            if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
            {
                throw new ArgumentException(Invariant($"'{nameof(assemblyQualifiedName)}' is white space"));
            }

            var cacheKey = new ResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKey(assemblyQualifiedName, assemblyVersionMatchStrategy, throwIfCannotResolve);

            if (CachedResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKeyToTypeMap.TryGetValue(cacheKey, out Type result))
            {
                return result;
            }

            result = assemblyQualifiedName.ToTypeRepresentationFromAssemblyQualifiedName().ResolveFromLoadedTypes(assemblyVersionMatchStrategy, throwIfCannotResolve);

            CachedResolveFromLoadedTypesUsingAssemblyQualifiedNameCacheKeyToTypeMap.TryAdd(cacheKey, result);

            return result;
        }

        /// <summary>
        /// Resolve the <see cref="TypeRepresentation" /> into a Type from the loaded Types.
        /// </summary>
        /// <param name="typeRepresentation">The representation of the type.</param>
        /// <param name="assemblyVersionMatchStrategy">Strategy to use for matching assembly versions.</param>
        /// <param name="throwIfCannotResolve">
        /// Optional value indicating whether to throw an exception if the required assembly(ies) or the type(s) cannot be found
        /// (generics, having generic type arguments, may cause the specified type representation to encapsulate multiple assemblies and/or multiple types).
        /// Default is true.
        /// </param>
        /// <returns>
        /// Resolved/matching Type.
        /// </returns>
        public static Type ResolveFromLoadedTypes(
            this TypeRepresentation typeRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion,
            bool throwIfCannotResolve = true)
        {
            if (typeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(typeRepresentation));
            }

            // If another version of the type is loaded after the type has been cached, then we potentially
            // may not be honoring the specified AssemblyMatchStrategy.  While we could move the whole
            // chunk of cache code right before Type.GetType, which would save us the cost of that call,
            // we have found that the most expensive call in the below code is assigning matchingAssemblies
            // (in testing it was 2x more expensive).  So another solution would be to cache the call to
            // AssemblyLoader.GetLoadedAssemblies() but then this would similarly not honor the specified
            // AssemblyMatchStrategy because the loaded assemblies would be cached upon the first call and
            // never refreshed.
            var cacheKey = new ResolveFromLoadedTypesUsingTypeRepresentationCacheKey(typeRepresentation, assemblyVersionMatchStrategy, throwIfCannotResolve);

            if (CachedResolveFromLoadedTypesUsingTypeRepresentationCacheKeyToTypeMap.TryGetValue(cacheKey, out Type result))
            {
                return result;
            }

            var assemblyQualifiedName = typeRepresentation.BuildAssemblyQualifiedName();

            var assemblyNamesInUse = typeRepresentation.GetAssemblyNamesInUse();

            var matchingAssemblies = AssemblyLoader.GetLoadedAssemblies().Where(_ => assemblyNamesInUse.Contains(_.GetName().Name)).ToList();

            var matchingAssembliesGroupedByName = matchingAssemblies.GroupBy(_ => _.GetName().Name).ToList();

            var matchingAssemblyNames = new HashSet<string>(matchingAssembliesGroupedByName.Select(_ => _.Key));

            var missingAssemblyNames = assemblyNamesInUse.Where(_ => !matchingAssemblyNames.Contains(_)).ToList();

            var foundAssemblyNames = new List<string>();

            foreach (var missingAssemblyName in missingAssemblyNames)
            {
                try
                {
                    var loadedAssembly = Assembly.Load(missingAssemblyName);

                    if (loadedAssembly != null)
                    {
                        foundAssemblyNames.Add(missingAssemblyName);
                    }
                }
                catch (Exception)
                {
                }
            }

            missingAssemblyNames = missingAssemblyNames.Except(foundAssemblyNames).ToList();

            if (missingAssemblyNames.Any())
            {
                if (throwIfCannotResolve)
                {
                    throw new InvalidOperationException(Invariant($"Unable to resolve the specified {nameof(TypeRepresentation)} ({assemblyQualifiedName}).  These assemblies are not loaded: {missingAssemblyNames.ToDelimitedString(", ")}."));
                }
            }
            else
            {
                switch (assemblyVersionMatchStrategy)
                {
                    case VersionMatchStrategy.AnySingleVersion:
                        var assembliesWithMultipleVersions = matchingAssembliesGroupedByName.Where(_ => _.Count() > 1).SelectMany(_ => _).ToList();

                        if (assembliesWithMultipleVersions.Any())
                        {
                            if (throwIfCannotResolve)
                            {
                                throw new InvalidOperationException(Invariant($"Unable to resolve the specified {nameof(TypeRepresentation)} ({assemblyQualifiedName}) with {nameof(VersionMatchStrategy)}.{nameof(VersionMatchStrategy.AnySingleVersion)}.  There were multiple versions of the following assemblies loaded: {assembliesWithMultipleVersions.Select(_ => "[" + _.FullName + "]").ToDelimitedString(", ")}."));
                            }
                        }
                        else
                        {
                            result = Type.GetType(assemblyQualifiedName, throwIfCannotResolve, false);
                        }

                        break;
                    default:
                        throw new NotSupportedException(Invariant($"This {nameof(VersionMatchStrategy)} is not supported: {assemblyVersionMatchStrategy}."));
                }
            }

            if ((result == null) && throwIfCannotResolve)
            {
                throw new InvalidOperationException(Invariant($"Unable to resolve the specified {nameof(TypeRepresentation)} {assemblyQualifiedName} with {nameof(VersionMatchStrategy)}.{nameof(VersionMatchStrategy.AnySingleVersion)} for unknown reasons.  We never expected to hit this line of code."));
            }

            CachedResolveFromLoadedTypesUsingTypeRepresentationCacheKeyToTypeMap.TryAdd(cacheKey, result);

            return result;
        }

        /// <summary>
        /// Hydrates a <see cref="TypeRepresentation"/> from an assembly qualified name
        /// generated by <see cref="BuildAssemblyQualifiedName"/>.
        /// </summary>
        /// <param name="assemblyQualifiedName">The assembly qualified name.</param>
        /// <returns>
        /// The <see cref="TypeRepresentation" /> for the specified assembly qualified name.
        /// </returns>
        public static TypeRepresentation ToTypeRepresentationFromAssemblyQualifiedName(
            this string assemblyQualifiedName)
        {
            if (assemblyQualifiedName == null)
            {
                throw new ArgumentNullException(nameof(assemblyQualifiedName));
            }

            if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
            {
                throw new ArgumentException(Invariant($"'{nameof(assemblyQualifiedName)}' is white space"));
            }

            if (CachedAssemblyQualifiedNameToTypeRepresentationMap.TryGetValue(assemblyQualifiedName, out var result))
            {
                return result;
            }

            var localAssemblyQualifiedName = assemblyQualifiedName;

            // if assemblyQualifiedName generated by Type.AssemblyQualifiedName then convert to OBC format (removes Culture and PublicKeyToken)
            localAssemblyQualifiedName = ConvertDotNetAssemblyQualifiedNameToObcAssemblyQualifiedNameRegex.Replace(localAssemblyQualifiedName, string.Empty);

            var versionSegment = BeforeLastCommaRegex.Replace(localAssemblyQualifiedName, string.Empty);

            string assemblyVersion = null;

            if (versionSegment.StartsWith(", Version=", StringComparison.Ordinal))
            {
                assemblyVersion = versionSegment.Split('=').Last();

                localAssemblyQualifiedName = BeforeLastCommaRegex.Match(localAssemblyQualifiedName).Value;
            }

            var assemblyName = BeforeLastCommaRegex.Replace(localAssemblyQualifiedName, string.Empty).Split(' ').Last();

            localAssemblyQualifiedName = BeforeLastCommaRegex.Match(localAssemblyQualifiedName).Value;

            var arrayIdentifiers = new List<string>();

            while (!string.IsNullOrEmpty(LastArrayIdentifierRegex.Match(localAssemblyQualifiedName).Value))
            {
                arrayIdentifiers.Add(LastArrayIdentifierRegex.Match(localAssemblyQualifiedName).Value);

                localAssemblyQualifiedName = LastArrayIdentifierRegex.Replace(localAssemblyQualifiedName, string.Empty);
            }

            arrayIdentifiers.Reverse();

            var arrayIdentifierForName = string.Join(string.Empty, arrayIdentifiers);

            string nameWithNamespace;

            IReadOnlyList<TypeRepresentation> genericArguments;

            // is closed generic type?
            if (localAssemblyQualifiedName.EndsWith("]]", StringComparison.Ordinal))
            {
                nameWithNamespace = localAssemblyQualifiedName.Substring(0, localAssemblyQualifiedName.IndexOf('['));

                localAssemblyQualifiedName = AllGenericArgumentsRegex.Match(localAssemblyQualifiedName).Groups[1].Value;

                genericArguments = localAssemblyQualifiedName
                    .ToGenericArgumentAssemblyQualifiedNames()
                    .Select(_ => _.Substring(1, _.Length - 2))
                    .Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName())
                    .ToList();
            }
            else
            {
                nameWithNamespace = localAssemblyQualifiedName;

                // is generic type definition?
                genericArguments = string.IsNullOrEmpty(IsGenericTypeRegex.Match(localAssemblyQualifiedName).Value)
                    ? null
                    : new TypeRepresentation[0];
            }

            var name = nameWithNamespace.Split('.').Last() + arrayIdentifierForName;

            var @namespace = string.Join(".", nameWithNamespace.Split('.').Reverse().Skip(1).Reverse());

            result = new TypeRepresentation(@namespace, name, assemblyName, assemblyVersion, genericArguments);

            CachedAssemblyQualifiedNameToTypeRepresentationMap.TryAdd(assemblyQualifiedName, result);

            return result;
        }

        /// <summary>
        /// Removes all assembly versions from the specified <see cref="TypeRepresentation"/>.
        /// </summary>
        /// <param name="representation">The type representation.</param>
        /// <returns>
        /// The specified <see cref="TypeRepresentation"/>, except with <see cref="TypeRepresentation.AssemblyVersion"/>
        /// set to null on the outer type as well as the tree of generic type arguments.
        /// </returns>
        public static TypeRepresentation RemoveAssemblyVersions(
            this TypeRepresentation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            var result = representation.DeepCloneWithAssemblyVersion(null);

            result = result.DeepCloneWithGenericArguments(result.GenericArguments?.Select(RemoveAssemblyVersions).ToList());

            return result;
        }

        /// <summary>
        /// Gets the name of the type, including it's chain of declaring types separated
        /// by the plus (+) symbol, similar to how the type's name appears in <see cref="Type.FullName"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Type.FullName"/> includes other details that aren't needed and there's no property on Type
        /// that returns the fully nested name.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The fully nested name of the type.
        /// </returns>
        public static string GetFullyNestedName(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = type.Name;

            // ReSharper disable once PossibleNullReferenceException
            while (type.IsArray)
            {
                type = type.GetElementType();
            }

            while (type.IsNested)
            {
                type = type.DeclaringType;

                // ReSharper disable once PossibleNullReferenceException
                result = type.Name + "+" + result;
            }

            return result;
        }

        /// <summary>
        /// Builds the assembly qualified name for a type.
        /// </summary>
        /// <param name="representation">The type representation.</param>
        /// <returns>
        /// The assembly qualified name of the specified type.
        /// </returns>
        public static string BuildAssemblyQualifiedName(
            this TypeRepresentation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            if (CachedTypeRepresentationToAssemblyQualifiedNameMap.TryGetValue(representation, out string result))
            {
                return result;
            }

            var assemblyName = representation.BuildAssemblyName();

            var genericArgumentsQualifiedNames = representation.GenericArguments?.Select(_ => "[" + _.BuildAssemblyQualifiedName() + "]").ToArray() ?? new string[0];

            var genericToken = genericArgumentsQualifiedNames.Any()
                ? Invariant($"[{string.Join(",", genericArgumentsQualifiedNames)}]")
                : string.Empty;

            if (representation.IsArray() && representation.IsClosedGenericType())
            {
                var arraySpecifierStartIndex = representation.Name.IndexOf('[');

                var nameWithGenericArguments = representation.Name.Insert(arraySpecifierStartIndex, genericToken);

                result = Invariant($"{representation.Namespace}.{nameWithGenericArguments}, {assemblyName.FullName}");
            }
            else
            {
                result = Invariant($"{representation.Namespace}.{representation.Name}{genericToken}, {assemblyName.FullName}");
            }

            CachedTypeRepresentationToAssemblyQualifiedNameMap.TryAdd(representation, result);

            return result;
        }

        /// <summary>
        /// Builds the <see cref="AssemblyName"/> of the assembly that contains a specified type.
        /// </summary>
        /// <param name="representation">The type representation.</param>
        /// <returns>
        /// The <see cref="AssemblyName"/> of the assembly that contains the specified type.
        /// </returns>
        public static AssemblyName BuildAssemblyName(
            this TypeRepresentation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            var versionToken = (representation.AssemblyVersion != null)
                ? Invariant($", Version={representation.AssemblyVersion}")
                : string.Empty;

            var assemblyName = Invariant($"{representation.AssemblyName}{versionToken}");

            var result = new AssemblyName(assemblyName);

            return result;
        }

        /// <summary>
        /// Determines if two <see cref="TypeRepresentation"/>s are equal, ignoring all version information.
        /// </summary>
        /// <param name="type1">The first type to compare.</param>
        /// <param name="type2">The second type to compare.</param>
        /// <returns>
        /// true if the types are equal when version is ignored, otherwise false.
        /// </returns>
        public static bool EqualsIgnoringVersion(
            this TypeRepresentation type1,
            TypeRepresentation type2)
        {
            var type1WithoutVersion = type1?.RemoveAssemblyVersions();
            var type2WithoutVersion = type2?.RemoveAssemblyVersions();

            var result = type1WithoutVersion == type2WithoutVersion;

            return result;
        }

        private static HashSet<string> GetAssemblyNamesInUse(
            this TypeRepresentation typeRepresentation)
        {
            var result = new HashSet<string> { typeRepresentation.AssemblyName };

            if (typeRepresentation.GenericArguments != null)
            {
                result.AddRange(typeRepresentation.GenericArguments.SelectMany(_ => _.GetAssemblyNamesInUse()));
            }

            return result;
        }

        private static IEnumerable<string> ToGenericArgumentAssemblyQualifiedNames(
            this string input)
        {
            // see: https://stackoverflow.com/a/61105221/356790
            var start = 0;
            var brackets = 0;

            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == '[')
                {
                    brackets++;

                    continue;
                }

                if (input[i] == ']')
                {
                    brackets--;

                    continue;
                }

                if (brackets == 0 && input[i] == ',')
                {
                    yield return input.Substring(start, i - start);

                    start = i + 1;
                }
            }

            if (start < input.Length)
            {
                yield return input.Substring(start);
            }
        }
    }
}
