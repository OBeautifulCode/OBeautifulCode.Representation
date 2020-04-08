﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.75.0)
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
    using global::System.Linq.Expressions;

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    public partial class MemberInitExpressionRepresentation : IModel<MemberInitExpressionRepresentation>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="MemberInitExpressionRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(MemberInitExpressionRepresentation left, MemberInitExpressionRepresentation right)
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
        /// Determines whether two objects of type <see cref="MemberInitExpressionRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(MemberInitExpressionRepresentation left, MemberInitExpressionRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(MemberInitExpressionRepresentation other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.NodeType.IsEqualTo(other.NodeType)
                      && this.Type.IsEqualTo(other.Type)
                      && this.NewExpressionRepresentation.IsEqualTo(other.NewExpressionRepresentation)
                      && this.Bindings.IsEqualTo(other.Bindings);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as MemberInitExpressionRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.NodeType)
            .Hash(this.Type)
            .Hash(this.NewExpressionRepresentation)
            .Hash(this.Bindings)
            .Value;

        /// <inheritdoc />
        public new MemberInitExpressionRepresentation DeepClone() => (MemberInitExpressionRepresentation)this.DeepCloneInternal();

        /// <inheritdoc />
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
        public override ExpressionRepresentationBase DeepCloneWithNodeType(ExpressionType nodeType)
        {
            throw new NotSupportedException("The constructor in-use (by code gen) for MemberInitExpressionRepresentation does not have a parameter that corresponds with the 'NodeType' property.  As such, this method, DeepCloneWithNodeType(ExpressionType nodeType), cannot utilize the specified 'nodeType' value for that property.");
        }

        /// <inheritdoc />
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
        public override ExpressionRepresentationBase DeepCloneWithType(TypeRepresentation type)
        {
            var result = new MemberInitExpressionRepresentation(
                                 type,
                                 this.NewExpressionRepresentation?.DeepClone(),
                                 this.Bindings?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="NewExpressionRepresentation" />.
        /// </summary>
        /// <param name="newExpressionRepresentation">The new <see cref="NewExpressionRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="MemberInitExpressionRepresentation" /> using the specified <paramref name="newExpressionRepresentation" /> for <see cref="NewExpressionRepresentation" /> and a deep clone of every other property.</returns>
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
        public MemberInitExpressionRepresentation DeepCloneWithNewExpressionRepresentation(NewExpressionRepresentation newExpressionRepresentation)
        {
            var result = new MemberInitExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 newExpressionRepresentation,
                                 this.Bindings?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Bindings" />.
        /// </summary>
        /// <param name="bindings">The new <see cref="Bindings" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="MemberInitExpressionRepresentation" /> using the specified <paramref name="bindings" /> for <see cref="Bindings" /> and a deep clone of every other property.</returns>
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
        public MemberInitExpressionRepresentation DeepCloneWithBindings(IReadOnlyCollection<MemberBindingRepresentationBase> bindings)
        {
            var result = new MemberInitExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NewExpressionRepresentation?.DeepClone(),
                                 bindings);

            return result;
        }

        /// <inheritdoc />
        protected override ExpressionRepresentationBase DeepCloneInternal()
        {
            var result = new MemberInitExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NewExpressionRepresentation?.DeepClone(),
                                 this.Bindings?.Select(i => i?.DeepClone()).ToList());

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.MemberInitExpressionRepresentation: NodeType = {this.NodeType.ToString() ?? "<null>"}, Type = {this.Type?.ToString() ?? "<null>"}, NewExpressionRepresentation = {this.NewExpressionRepresentation?.ToString() ?? "<null>"}, Bindings = {this.Bindings?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}