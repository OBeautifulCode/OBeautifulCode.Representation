// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodInfoDescription.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Description of <see cref="MethodInfo" />.
    /// </summary>
    public class MethodInfoDescription : IEquatable<MethodInfoDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoDescription" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodHash">The method hash.</param>
        /// <param name="genericArguments">The generic arguments.</param>
        public MethodInfoDescription(TypeDescription type, string methodHash, IReadOnlyList<TypeDescription> genericArguments)
        {
            this.Type = type;
            this.MethodHash = methodHash;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <value>The generic arguments.</value>
        public IReadOnlyList<TypeDescription> GenericArguments { get; private set; }

        /// <summary>
        /// Gets the method hash.
        /// </summary>
        /// <value>The method hash.</value>
        public string MethodHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeDescription Type { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="MethodInfoDescription" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            MethodInfoDescription left,
            MethodInfoDescription right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result =
                (left.Type == right.Type) &&
                (left.MethodHash == right.MethodHash) &&
                left.GenericArguments.SequenceEqualHandlingNulls(right.GenericArguments);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="MethodInfoDescription" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            MethodInfoDescription left,
            MethodInfoDescription right)
            => !(left == right);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(MethodInfoDescription other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as MethodInfoDescription);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.Type)
                .Hash(this.MethodHash)
                .HashElements(this.GenericArguments)
                .Value;
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MethodInfoDescription" />.
    /// </summary>
    public static class MethodInfoDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Gets the method hash.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>Hash of the method.</returns>
        public static string GetSignatureHash(this MethodInfo methodInfo)
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
        /// Converts to description.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>Converted <see cref="MethodInfoDescription" />.</returns>
        public static MethodInfoDescription ToDescription(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodHash = methodInfo.GetSignatureHash();
            var genericArguments = methodInfo.GetGenericArguments().Select(_ => _.ToDescription()).ToList();
            var result = new MethodInfoDescription(methodInfo.DeclaringType.ToDescription(), methodHash, genericArguments);
            return result;
        }

        /// <summary>
        /// Converts from description.
        /// </summary>
        /// <param name="methodInfoDescription">The description.</param>
        /// <returns>Converted <see cref="MemberInfo" />.</returns>
        public static MethodInfo FromDescription(this MethodInfoDescription methodInfoDescription)
        {
            if (methodInfoDescription == null)
            {
                throw new ArgumentNullException(nameof(methodInfoDescription));
            }

            var methodHash = methodInfoDescription.MethodHash;
            var genericArguments = methodInfoDescription.GenericArguments.Select(_ => _.ResolveFromLoadedTypes()).ToArray();
            var type = methodInfoDescription.Type.ResolveFromLoadedTypes();
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
                throw new ArgumentException(Invariant($"Could not find a member that matched hash '{methodInfoDescription.MethodHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.Item2.ToString()));
                throw new ArgumentException(Invariant($"Found too many members that matched hash '{methodInfoDescription.MethodHash}' on type '{type}'; {foundAddIn}."));
            }

            return results.Single().Item2;
        }

        private static IReadOnlyCollection<MethodInfo> GetAllMethodInfos(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var methodInfos = new List<MethodInfo>();

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
                    .Where(x => !methodInfos.Contains(x));

                methodInfos.InsertRange(0, newPropertyInfos);
            }

            return methodInfos;
        }
    }
}