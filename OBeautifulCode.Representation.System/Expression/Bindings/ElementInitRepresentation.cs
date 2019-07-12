// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementInitRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;

    /// <summary>
    /// Representation of <see cref="ElementInit" />.
    /// </summary>
    public class ElementInitRepresentation : IEquatable<ElementInitRepresentation>
    {
        /// <summary>Initializes a new instance of the <see cref="ElementInitRepresentation"/> class.</summary>
        /// <param name="type">Type with method.</param>
        /// <param name="addMethod">The add method.</param>
        /// <param name="arguments">The arguments.</param>
        public ElementInitRepresentation(
            TypeRepresentation type, MethodInfoRepresentation addMethod, IReadOnlyList<ExpressionRepresentationBase> arguments)
        {
            this.Type = type;
            this.AddMethod = addMethod;
            this.Arguments = arguments;
        }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }

        /// <summary>Gets the add method.</summary>
        /// <value>The add method.</value>
        public MethodInfoRepresentation AddMethod { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionRepresentationBase> Arguments { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="ElementInitRepresentation" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            ElementInitRepresentation left,
            ElementInitRepresentation right)
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
                (left.AddMethod == right.AddMethod) &&
                left.Arguments.SequenceEqualHandlingNulls(right.Arguments);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="ElementInitRepresentation" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            ElementInitRepresentation left,
            ElementInitRepresentation right)
            => !(left == right);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(ElementInitRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ElementInitRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.Type)
                .Hash(this.AddMethod)
                .HashElements(this.Arguments)
                .Value;
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="ElementInitRepresentation" /> and <see cref="ElementInit" />.
                              /// </summary>
    public static class ElementInitRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="elementInit">The elementInitRepresentation.</param>
        /// <returns>Serializable version.</returns>
        public static ElementInitRepresentation ToRepresentation(this ElementInit elementInit)
        {
            if (elementInit == null)
            {
                throw new ArgumentNullException(nameof(elementInit));
            }

            var type = elementInit.AddMethod.DeclaringType.ToRepresentation();
            var addMethodRepresentation = elementInit.AddMethod.ToRepresentation();
            var arguments = elementInit.Arguments.ToRepresentation();
            var result = new ElementInitRepresentation(type, addMethodRepresentation, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="elementInitRepresentation">The elementInitRepresentation.</param>
        /// <returns>Converted version.</returns>
        public static ElementInit FromRepresentation(this ElementInitRepresentation elementInitRepresentation)
        {
            if (elementInitRepresentation == null)
            {
                throw new ArgumentNullException(nameof(elementInitRepresentation));
            }

            var type = elementInitRepresentation.Type.ResolveFromLoadedTypes();
            var addMethod = type.GetMethods().Single(_ => _.ToRepresentation().Equals(elementInitRepresentation.AddMethod));
            var arguments = elementInitRepresentation.Arguments.FromRepresentation();

            var result = Expression.ElementInit(addMethod, arguments);
            return result;
        }

        /// <summary>Converts to description.</summary>
        /// <param name="elementInitList">The list of <see cref="ElementInit" />.</param>
        /// <returns>Converted list of <see cref="ElementInitRepresentation" />.</returns>
        public static IReadOnlyList<ElementInitRepresentation> ToRepresentation(this IReadOnlyList<ElementInit> elementInitList)
        {
            var result = elementInitList.Select(_ => _.ToRepresentation()).ToList();
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="elementInitRepresentationList">The elementInitRepresentation.</param>
        /// <returns>Converted version.</returns>
        public static IReadOnlyList<ElementInit> FromRepresentation(this IReadOnlyList<ElementInitRepresentation> elementInitRepresentationList)
        {
            var result = elementInitRepresentationList.Select(_ => _.FromRepresentation()).ToList();
            return result;
        }
    }
}
