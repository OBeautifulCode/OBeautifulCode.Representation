// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorInfoRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using OBeautifulCode.Math.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ConstructorInfo" />.
    /// </summary>
    public class ConstructorInfoRepresentation : IEquatable<ConstructorInfoRepresentation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInfoRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorHash">The method hash.</param>
        public ConstructorInfoRepresentation(TypeRepresentation type, string constructorHash)
        {
            this.Type = type;
            this.ConstructorHash = constructorHash;
        }

        /// <summary>
        /// Gets the constructor hash.
        /// </summary>
        /// <value>The constructor hash.</value>
        public string ConstructorHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="ConstructorInfoRepresentation" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            ConstructorInfoRepresentation left,
            ConstructorInfoRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = (left.Type == right.Type)
                      && (left.ConstructorHash == right.ConstructorHash);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="ConstructorInfoRepresentation" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            ConstructorInfoRepresentation left,
            ConstructorInfoRepresentation right)
            => !(left == right);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(ConstructorInfoRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ConstructorInfoRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                          .Hash(this.Type)
                          .Hash(this.ConstructorHash)
                          .Value;
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ConstructorInfoRepresentation" />.
    /// </summary>
    public static class ConstructorInfoRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Gets the constructor hash.</summary>
        /// <param name="constructorInfo">The constructor information.</param>
        /// <returns>Hash of the constructor.</returns>
        public static string GetSignatureHash(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var methodName = constructorInfo.Name;
            var generics = constructorInfo.IsGenericMethod ? string.Join(",", constructorInfo.GetGenericArguments().Select(_ => _.FullName)) : null;
            var genericsAddIn = generics == null ? string.Empty : Invariant($"<{generics}>");
            var parameters = string.Join(",", constructorInfo.GetParameters().Select(_ => Invariant($"{_.ParameterType}-{_.Name}")));
            var result = Invariant($"{methodName}{genericsAddIn}({parameters})");
            return result;
        }

        /// <summary>
        /// Converts to representation.
        /// </summary>
        /// <param name="constructorInfo">The constructor information.</param>
        /// <returns>Converted <see cref="ConstructorInfoRepresentation" />.</returns>
        public static ConstructorInfoRepresentation ToRepresentation(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var type = constructorInfo.DeclaringType.ToRepresentation();
            var constructorHash = constructorInfo.GetSignatureHash();
            var result = new ConstructorInfoRepresentation(type, constructorHash);
            return result;
        }

        /// <summary>
        /// Converts from representation.
        /// </summary>
        /// <param name="constructorInfoRepresentation">The representation.</param>
        /// <returns>Converted <see cref="ConstructorInfo" />.</returns>
        public static ConstructorInfo FromRepresentation(this ConstructorInfoRepresentation constructorInfoRepresentation)
        {
            if (constructorInfoRepresentation == null)
            {
                throw new ArgumentNullException(nameof(constructorInfoRepresentation));
            }

            var type = constructorInfoRepresentation.Type.ResolveFromLoadedTypes();
            var results = type.GetConstructors()
                              .Where(_ => _.GetSignatureHash().Equals(constructorInfoRepresentation.ConstructorHash, StringComparison.OrdinalIgnoreCase))
                              .ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find a constructor that matched hash '{constructorInfoRepresentation.ConstructorHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.ToString()));
                throw new ArgumentException(Invariant($"Found too many constructors that matched hash '{constructorInfoRepresentation.ConstructorHash}' on type '{type}'; {foundAddIn}."));
            }

            return results.Single();
        }
    }
}