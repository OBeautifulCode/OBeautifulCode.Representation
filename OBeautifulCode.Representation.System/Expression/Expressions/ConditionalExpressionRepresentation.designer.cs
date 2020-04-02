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
    using global::System.Linq.Expressions;

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    public partial class ConditionalExpressionRepresentation : IModel<ConditionalExpressionRepresentation>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="ConditionalExpressionRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(ConditionalExpressionRepresentation left, ConditionalExpressionRepresentation right)
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
        /// Determines whether two objects of type <see cref="ConditionalExpressionRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(ConditionalExpressionRepresentation left, ConditionalExpressionRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ConditionalExpressionRepresentation other)
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
                      && this.Test.IsEqualTo(other.Test)
                      && this.IfTrue.IsEqualTo(other.IfTrue)
                      && this.IfFalse.IsEqualTo(other.IfFalse);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ConditionalExpressionRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.NodeType)
            .Hash(this.Type)
            .Hash(this.Test)
            .Hash(this.IfTrue)
            .Hash(this.IfFalse)
            .Value;

        /// <inheritdoc />
        public new ConditionalExpressionRepresentation DeepClone() => (ConditionalExpressionRepresentation)this.DeepCloneInternal();

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
        public override ExpressionRepresentationBase DeepCloneWithNodeType(ExpressionType nodeType)
        {
            var result = new ConditionalExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 nodeType,
                                 this.Test?.DeepClone(),
                                 this.IfTrue?.DeepClone(),
                                 this.IfFalse?.DeepClone());

            return result;
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
        public override ExpressionRepresentationBase DeepCloneWithType(TypeRepresentation type)
        {
            var result = new ConditionalExpressionRepresentation(
                                 type,
                                 this.NodeType,
                                 this.Test?.DeepClone(),
                                 this.IfTrue?.DeepClone(),
                                 this.IfFalse?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Test" />.
        /// </summary>
        /// <param name="test">The new <see cref="Test" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConditionalExpressionRepresentation" /> using the specified <paramref name="test" /> for <see cref="Test" /> and a deep clone of every other property.</returns>
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
        public ConditionalExpressionRepresentation DeepCloneWithTest(ExpressionRepresentationBase test)
        {
            var result = new ConditionalExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NodeType,
                                 test,
                                 this.IfTrue?.DeepClone(),
                                 this.IfFalse?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="IfTrue" />.
        /// </summary>
        /// <param name="ifTrue">The new <see cref="IfTrue" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConditionalExpressionRepresentation" /> using the specified <paramref name="ifTrue" /> for <see cref="IfTrue" /> and a deep clone of every other property.</returns>
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
        public ConditionalExpressionRepresentation DeepCloneWithIfTrue(ExpressionRepresentationBase ifTrue)
        {
            var result = new ConditionalExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NodeType,
                                 this.Test?.DeepClone(),
                                 ifTrue,
                                 this.IfFalse?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="IfFalse" />.
        /// </summary>
        /// <param name="ifFalse">The new <see cref="IfFalse" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConditionalExpressionRepresentation" /> using the specified <paramref name="ifFalse" /> for <see cref="IfFalse" /> and a deep clone of every other property.</returns>
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
        public ConditionalExpressionRepresentation DeepCloneWithIfFalse(ExpressionRepresentationBase ifFalse)
        {
            var result = new ConditionalExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NodeType,
                                 this.Test?.DeepClone(),
                                 this.IfTrue?.DeepClone(),
                                 ifFalse);

            return result;
        }

        /// <inheritdoc />
        protected override ExpressionRepresentationBase DeepCloneInternal()
        {
            var result = new ConditionalExpressionRepresentation(
                                 this.Type?.DeepClone(),
                                 this.NodeType,
                                 this.Test?.DeepClone(),
                                 this.IfTrue?.DeepClone(),
                                 this.IfFalse?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.ConditionalExpressionRepresentation: NodeType = {this.NodeType.ToString() ?? "<null>"}, Type = {this.Type?.ToString() ?? "<null>"}, Test = {this.Test?.ToString() ?? "<null>"}, IfTrue = {this.IfTrue?.ToString() ?? "<null>"}, IfFalse = {this.IfFalse?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}