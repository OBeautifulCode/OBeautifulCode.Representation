// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementInitRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ElementInit" />.
    /// </summary>
    public class ElementInitRepresentation : IModel<ElementInitRepresentation>
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
        /// Determines whether two objects of type <see cref="ElementInitRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; otherwise false.</returns>
        public static bool operator ==(ElementInitRepresentation left, ElementInitRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Type == right.Type
                      && left.AddMethod == right.AddMethod
                      && left.Arguments.SequenceEqualHandlingNulls(right.Arguments);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="ElementInitRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; otherwise false.</returns>
        public static bool operator !=(ElementInitRepresentation left, ElementInitRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ElementInitRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ElementInitRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Type)
            .Hash(this.AddMethod)
            .HashElements(this.Arguments)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public ElementInitRepresentation DeepClone()
        {
            var result = new ElementInitRepresentation(
                                 this.Type,
                                 this.AddMethod,
                                 this.Arguments?.Select(_ => _).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="type" />.
        /// </summary>
        /// <param name="type">The new <see cref="Type" />.</param>
        /// <returns>New <see cref="ElementInitRepresentation" /> using the specified <paramref name="type" /> for <see cref="Type" /> and a deep clone of every other property.</returns>
        public ElementInitRepresentation DeepCloneWithType(TypeRepresentation type)
        {
            var result = new ElementInitRepresentation(
                                 type,
                                 this.AddMethod,
                                 this.Arguments?.Select(_ => _).ToList());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="addMethod" />.
        /// </summary>
        /// <param name="addMethod">The new <see cref="AddMethod" />.</param>
        /// <returns>New <see cref="ElementInitRepresentation" /> using the specified <paramref name="addMethod" /> for <see cref="AddMethod" /> and a deep clone of every other property.</returns>
        public ElementInitRepresentation DeepCloneWithAddMethod(MethodInfoRepresentation addMethod)
        {
            var result = new ElementInitRepresentation(
                                 this.Type,
                                 addMethod,
                                 this.Arguments?.Select(_ => _).ToList());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="arguments" />.
        /// </summary>
        /// <param name="arguments">The new <see cref="Arguments" />.</param>
        /// <returns>New <see cref="ElementInitRepresentation" /> using the specified <paramref name="arguments" /> for <see cref="Arguments" /> and a deep clone of every other property.</returns>
        public ElementInitRepresentation DeepCloneWithArguments(IReadOnlyList<ExpressionRepresentationBase> arguments)
        {
            var result = new ElementInitRepresentation(
                                 this.Type,
                                 this.AddMethod,
                                 arguments);
            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"{nameof(OBeautifulCode.Representation)}.{nameof(ElementInitRepresentation)}: Type = {this.Type.ToString() ?? "<null>"}, AddMethod = {this.AddMethod.ToString() ?? "<null>"}, Arguments = {this.Arguments.ToString() ?? "<null>"}.");

            return result;
        }
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

        /// <summary>Converts to representation.</summary>
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
