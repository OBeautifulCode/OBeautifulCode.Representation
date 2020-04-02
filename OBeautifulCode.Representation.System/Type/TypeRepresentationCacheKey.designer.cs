﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.68.0)
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

    public partial class TypeRepresentationCacheKey : IModel<TypeRepresentationCacheKey>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentationCacheKey"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(TypeRepresentationCacheKey left, TypeRepresentationCacheKey right)
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
        /// Determines whether two objects of type <see cref="TypeRepresentationCacheKey"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(TypeRepresentationCacheKey left, TypeRepresentationCacheKey right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeRepresentationCacheKey other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.TypeRepresentation.IsEqualTo(other.TypeRepresentation)
                      && this.TypeMatchStrategy.IsEqualTo(other.TypeMatchStrategy)
                      && this.MultipleMatchStrategy.IsEqualTo(other.MultipleMatchStrategy);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeRepresentationCacheKey);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.TypeRepresentation)
            .Hash(this.TypeMatchStrategy)
            .Hash(this.MultipleMatchStrategy)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public TypeRepresentationCacheKey DeepClone()
        {
            var result = new TypeRepresentationCacheKey(
                                 this.TypeRepresentation?.DeepClone(),
                                 this.TypeMatchStrategy,
                                 this.MultipleMatchStrategy);

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeRepresentation" />.
        /// </summary>
        /// <param name="typeRepresentation">The new <see cref="TypeRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TypeRepresentationCacheKey" /> using the specified <paramref name="typeRepresentation" /> for <see cref="TypeRepresentation" /> and a deep clone of every other property.</returns>
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
        public TypeRepresentationCacheKey DeepCloneWithTypeRepresentation(TypeRepresentation typeRepresentation)
        {
            var result = new TypeRepresentationCacheKey(
                                 typeRepresentation,
                                 this.TypeMatchStrategy,
                                 this.MultipleMatchStrategy);

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeMatchStrategy" />.
        /// </summary>
        /// <param name="typeMatchStrategy">The new <see cref="TypeMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TypeRepresentationCacheKey" /> using the specified <paramref name="typeMatchStrategy" /> for <see cref="TypeMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public TypeRepresentationCacheKey DeepCloneWithTypeMatchStrategy(TypeMatchStrategy typeMatchStrategy)
        {
            var result = new TypeRepresentationCacheKey(
                                 this.TypeRepresentation?.DeepClone(),
                                 typeMatchStrategy,
                                 this.MultipleMatchStrategy);

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="MultipleMatchStrategy" />.
        /// </summary>
        /// <param name="multipleMatchStrategy">The new <see cref="MultipleMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TypeRepresentationCacheKey" /> using the specified <paramref name="multipleMatchStrategy" /> for <see cref="MultipleMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public TypeRepresentationCacheKey DeepCloneWithMultipleMatchStrategy(MultipleMatchStrategy multipleMatchStrategy)
        {
            var result = new TypeRepresentationCacheKey(
                                 this.TypeRepresentation?.DeepClone(),
                                 this.TypeMatchStrategy,
                                 multipleMatchStrategy);

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.TypeRepresentationCacheKey: TypeRepresentation = {this.TypeRepresentation?.ToString() ?? "<null>"}, TypeMatchStrategy = {this.TypeMatchStrategy.ToString() ?? "<null>"}, MultipleMatchStrategy = {this.MultipleMatchStrategy.ToString() ?? "<null>"}.");

            return result;
        }
    }
}