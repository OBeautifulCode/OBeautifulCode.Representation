// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodInfoRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="MethodInfo" />.
    /// </summary>
    public partial class MethodInfoRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoRepresentation" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodHash">The method hash.</param>
        /// <param name="genericArguments">The generic arguments.</param>
        public MethodInfoRepresentation(
            TypeRepresentation type,
            string methodHash,
            IReadOnlyList<TypeRepresentation> genericArguments)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodHash == null)
            {
                throw new ArgumentNullException(nameof(methodHash));
            }

            if (string.IsNullOrWhiteSpace(methodHash))
            {
                throw new ArgumentException(Invariant($"'{nameof(methodHash)}' is white space"));
            }

            if (genericArguments == null)
            {
                throw new ArgumentNullException(nameof(genericArguments));
            }

            if (!genericArguments.Any())
            {
                throw new ArgumentException(Invariant($"'{nameof(genericArguments)}' is an empty enumerable"));
            }

            if (genericArguments.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(genericArguments)}' contains an element that is null"));
            }

            this.Type = type;
            this.MethodHash = methodHash;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        public IReadOnlyList<TypeRepresentation> GenericArguments { get; private set; }

        /// <summary>
        /// Gets the method hash.
        /// </summary>
        public string MethodHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MethodInfoRepresentation" />.
    /// </summary>
    public static class MethodInfoRepresentationExtensions
    {
        /// <summary>
        /// Gets the method hash.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>
        /// Hash of the method.
        /// </returns>
        public static string GetSignatureHash(
            this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodName = methodInfo.Name;

            var generics = methodInfo.IsGenericMethod ? string.Join(",", methodInfo.GetGenericArguments().Select(_ => _.FullName)) : null;

            var genericsAddIn = generics == null ? string.Empty : Invariant($"<{generics}>");

            var parameters = string.Join(",", methodInfo.GetParameters().Select(_ => Invariant($"{_.ParameterType}-{_.Name}")));

            var result = Invariant($"{methodName}{genericsAddIn}({parameters})");

            return result;
        }

        /// <summary>
        /// Converts to representation.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>
        /// Converted <see cref="MethodInfoRepresentation" />.
        /// </returns>
        public static MethodInfoRepresentation ToRepresentation(
            this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodHash = methodInfo.GetSignatureHash();

            var genericArguments = methodInfo.GetGenericArguments().Select(_ => _.ToRepresentation()).ToList();

            var result = new MethodInfoRepresentation(methodInfo.DeclaringType.ToRepresentation(), methodHash, genericArguments);

            return result;
        }

        /// <summary>
        /// Converts from representation.
        /// </summary>
        /// <param name="methodInfoRepresentation">The representation.</param>
        /// <returns>
        /// Converted <see cref="MemberInfo" />.
        /// </returns>
        public static MethodInfo FromRepresentation(
            this MethodInfoRepresentation methodInfoRepresentation)
        {
            if (methodInfoRepresentation == null)
            {
                throw new ArgumentNullException(nameof(methodInfoRepresentation));
            }

            var methodHash = methodInfoRepresentation.MethodHash;

            var genericArguments = methodInfoRepresentation.GenericArguments.Select(_ => _.ResolveFromLoadedTypes()).ToArray();

            var type = methodInfoRepresentation.Type.ResolveFromLoadedTypes();

            var methodInfos = type.GetAllMethodInfos();

            var methodHashAndInfoTupleSet = methodInfos.Select(methodInfo =>
            {
                var localMethodInfo = methodInfo.IsGenericMethod
                    ? methodInfo.MakeGenericMethod(genericArguments)
                    : methodInfo;
                var localMethodHash = localMethodInfo.GetSignatureHash();

                return new Tuple<string, MethodInfo>(localMethodHash, localMethodInfo);
            });

            var results = methodHashAndInfoTupleSet.Where(_ => _.Item1.Equals(methodHash, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find a member that matched hash '{methodInfoRepresentation.MethodHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.Item2.ToString()));
                throw new ArgumentException(Invariant($"Found too many members that matched hash '{methodInfoRepresentation.MethodHash}' on type '{type}'; {foundAddIn}."));
            }

            var result = results.Single().Item2;

            return result;
        }

        private static IReadOnlyCollection<MethodInfo> GetAllMethodInfos(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = new List<MethodInfo>();

            var considered = new List<Type>();

            var queue = new Queue<Type>();

            considered.Add(type);

            queue.Enqueue(type);

            while (queue.Count > 0)
            {
                var subType = queue.Dequeue();

                foreach (var subInterface in subType.GetInterfaces())
                {
                    if (considered.Contains(subInterface))
                    {
                        continue;
                    }

                    considered.Add(subInterface);

                    queue.Enqueue(subInterface);
                }

                var typeProperties = subType.GetMethods(
                    BindingFlags.FlattenHierarchy
                    | BindingFlags.Public
                    | BindingFlags.Instance);

                var newPropertyInfos = typeProperties
                    .Where(x => !result.Contains(x));

                result.InsertRange(0, newPropertyInfos);
            }

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}