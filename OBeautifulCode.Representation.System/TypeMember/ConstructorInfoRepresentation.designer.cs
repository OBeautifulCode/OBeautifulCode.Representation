﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.141.0)
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
    public partial class ConstructorInfoRepresentation : IModel<ConstructorInfoRepresentation>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="ConstructorInfoRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(ConstructorInfoRepresentation left, ConstructorInfoRepresentation right)
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
        /// Determines whether two objects of type <see cref="ConstructorInfoRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(ConstructorInfoRepresentation left, ConstructorInfoRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ConstructorInfoRepresentation other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.ConstructorHash.IsEqualTo(other.ConstructorHash, StringComparer.Ordinal)
                      && this.Type.IsEqualTo(other.Type);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ConstructorInfoRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.ConstructorHash)
            .Hash(this.Type)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public ConstructorInfoRepresentation DeepClone()
        {
            var result = new ConstructorInfoRepresentation(
                                 this.Type?.DeepClone(),
                                 this.ConstructorHash?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ConstructorHash" />.
        /// </summary>
        /// <param name="constructorHash">The new <see cref="ConstructorHash" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConstructorInfoRepresentation" /> using the specified <paramref name="constructorHash" /> for <see cref="ConstructorHash" /> and a deep clone of every other property.</returns>
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
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ConstructorInfoRepresentation DeepCloneWithConstructorHash(string constructorHash)
        {
            var result = new ConstructorInfoRepresentation(
                                 this.Type?.DeepClone(),
                                 constructorHash);

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Type" />.
        /// </summary>
        /// <param name="type">The new <see cref="Type" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConstructorInfoRepresentation" /> using the specified <paramref name="type" /> for <see cref="Type" /> and a deep clone of every other property.</returns>
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
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ConstructorInfoRepresentation DeepCloneWithType(TypeRepresentation type)
        {
            var result = new ConstructorInfoRepresentation(
                                 type,
                                 this.ConstructorHash?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.ConstructorInfoRepresentation: ConstructorHash = {this.ConstructorHash?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Type = {this.Type?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}