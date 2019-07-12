// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementInitDescription.cs" company="OBeautifulCode">
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
    /// Description of <see cref="ElementInit" />.
    /// </summary>
    public class ElementInitDescription : IEquatable<ElementInitDescription>
    {
        /// <summary>Initializes a new instance of the <see cref="ElementInitDescription"/> class.</summary>
        /// <param name="type">Type with method.</param>
        /// <param name="addMethod">The add method.</param>
        /// <param name="arguments">The arguments.</param>
        public ElementInitDescription(
            TypeDescription type, MethodInfoDescription addMethod, IReadOnlyList<ExpressionDescriptionBase> arguments)
        {
            this.Type = type;
            this.AddMethod = addMethod;
            this.Arguments = arguments;
        }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeDescription Type { get; private set; }

        /// <summary>Gets the add method.</summary>
        /// <value>The add method.</value>
        public MethodInfoDescription AddMethod { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionDescriptionBase> Arguments { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="ElementInitDescription" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            ElementInitDescription left,
            ElementInitDescription right)
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
        /// Determines whether two objects of type <see cref="ElementInitDescription" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            ElementInitDescription left,
            ElementInitDescription right)
            => !(left == right);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(ElementInitDescription other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ElementInitDescription);

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
                              /// Extensions to <see cref="ElementInitDescription" /> and <see cref="ElementInit" />.
                              /// </summary>
    public static class ElementInitDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="elementInit">The elementInitDescription.</param>
        /// <returns>Serializable version.</returns>
        public static ElementInitDescription ToDescription(this ElementInit elementInit)
        {
            if (elementInit == null)
            {
                throw new ArgumentNullException(nameof(elementInit));
            }

            var type = elementInit.AddMethod.DeclaringType.ToDescription();
            var addMethodDescription = elementInit.AddMethod.ToDescription();
            var arguments = elementInit.Arguments.ToDescription();
            var result = new ElementInitDescription(type, addMethodDescription, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="elementInitDescription">The elementInitDescription.</param>
        /// <returns>Converted version.</returns>
        public static ElementInit FromDescription(this ElementInitDescription elementInitDescription)
        {
            if (elementInitDescription == null)
            {
                throw new ArgumentNullException(nameof(elementInitDescription));
            }

            var type = elementInitDescription.Type.ResolveFromLoadedTypes();
            var addMethod = type.GetMethods().Single(_ => _.ToDescription().Equals(elementInitDescription.AddMethod));
            var arguments = elementInitDescription.Arguments.FromDescription();

            var result = Expression.ElementInit(addMethod, arguments);
            return result;
        }

        /// <summary>Converts to description.</summary>
        /// <param name="elementInitList">The list of <see cref="ElementInit" />.</param>
        /// <returns>Converted list of <see cref="ElementInitDescription" />.</returns>
        public static IReadOnlyList<ElementInitDescription> ToDescription(this IReadOnlyList<ElementInit> elementInitList)
        {
            var result = elementInitList.Select(_ => _.ToDescription()).ToList();
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="elementInitDescriptionList">The elementInitDescription.</param>
        /// <returns>Converted version.</returns>
        public static IReadOnlyList<ElementInit> FromDescription(this IReadOnlyList<ElementInitDescription> elementInitDescriptionList)
        {
            var result = elementInitDescriptionList.Select(_ => _.FromDescription()).ToList();
            return result;
        }
    }
}
