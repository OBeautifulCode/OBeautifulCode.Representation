﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.79.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Linq;

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class MethodInfoRepresentation : IModel<MethodInfoRepresentation>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="MethodInfoRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(MethodInfoRepresentation left, MethodInfoRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Equals(right);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="MethodInfoRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(MethodInfoRepresentation left, MethodInfoRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(MethodInfoRepresentation other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.GenericArguments.IsEqualTo(other.GenericArguments)
                      && this.MethodHash.IsEqualTo(other.MethodHash, StringComparer.Ordinal)
                      && this.Type.IsEqualTo(other.Type);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as MethodInfoRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.GenericArguments)
            .Hash(this.MethodHash)
            .Hash(this.Type)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public MethodInfoRepresentation DeepClone()
        {
            var result = new MethodInfoRepresentation(
                                 this.Type?.DeepClone(),
                                 this.MethodHash?.Clone().ToString(),
                                 this.GenericArguments?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="GenericArguments" />.
        /// </summary>
        /// <param name="genericArguments">The new <see cref="GenericArguments" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="MethodInfoRepresentation" /> using the specified <paramref name="genericArguments" /> for <see cref="GenericArguments" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        public MethodInfoRepresentation DeepCloneWithGenericArguments(IReadOnlyList<TypeRepresentation> genericArguments)
        {
            var result = new MethodInfoRepresentation(
                                 this.Type?.DeepClone(),
                                 this.MethodHash?.Clone().ToString(),
                                 genericArguments);

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="MethodHash" />.
        /// </summary>
        /// <param name="methodHash">The new <see cref="MethodHash" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="MethodInfoRepresentation" /> using the specified <paramref name="methodHash" /> for <see cref="MethodHash" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        public MethodInfoRepresentation DeepCloneWithMethodHash(string methodHash)
        {
            var result = new MethodInfoRepresentation(
                                 this.Type?.DeepClone(),
                                 methodHash,
                                 this.GenericArguments?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Type" />.
        /// </summary>
        /// <param name="type">The new <see cref="Type" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="MethodInfoRepresentation" /> using the specified <paramref name="type" /> for <see cref="Type" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        public MethodInfoRepresentation DeepCloneWithType(TypeRepresentation type)
        {
            var result = new MethodInfoRepresentation(
                                 type,
                                 this.MethodHash?.Clone().ToString(),
                                 this.GenericArguments?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.MethodInfoRepresentation: GenericArguments = {this.GenericArguments?.ToString() ?? "<null>"}, MethodHash = {this.MethodHash?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Type = {this.Type?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}