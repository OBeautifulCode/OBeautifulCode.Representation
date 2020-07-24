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

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ElementInit" />.
    /// </summary>
    public partial class ElementInitRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementInitRepresentation"/> class.
        /// </summary>
        /// <param name="type">Type with method.</param>
        /// <param name="addMethod">The add method.</param>
        /// <param name="arguments">The arguments.</param>
        public ElementInitRepresentation(
            TypeRepresentation type,
            MethodInfoRepresentation addMethod,
            IReadOnlyList<ExpressionRepresentationBase> arguments)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (addMethod == null)
            {
                throw new ArgumentNullException(nameof(addMethod));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (!arguments.Any())
            {
                throw new ArgumentException(Invariant($"'{nameof(arguments)}' is an empty enumerable"));
            }

            if (arguments.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(arguments)}' contains an element that is null"));
            }

            this.Type = type;
            this.AddMethod = addMethod;
            this.Arguments = arguments;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }

        /// <summary>
        /// Gets the add method.
        /// </summary>
        public MethodInfoRepresentation AddMethod { get; private set; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IReadOnlyList<ExpressionRepresentationBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ElementInitRepresentation" /> and <see cref="ElementInit" />.
    /// </summary>
    public static class ElementInitRepresentationExtensions
    {
        /// <summary>
        /// Gets the representation of the specified <see cref="ElementInit"/>.
        /// </summary>
        /// <param name="elementInit">The <see cref="ElementInit"/>.</param>
        /// <returns>
        /// The representation of the specified <see cref="ElementInit"/>.
        /// </returns>
        public static ElementInitRepresentation ToRepresentation(
            this ElementInit elementInit)
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

        /// <summary>
        /// Gets an <see cref="ElementInit"/> from its representation.
        /// </summary>
        /// <param name="elementInitRepresentation">The representation of the <see cref="ElementInit"/>.</param>
        /// <returns>
        /// The <see cref="ElementInit"/>.
        /// </returns>
        public static ElementInit FromRepresentation(
            this ElementInitRepresentation elementInitRepresentation)
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

        /// <summary>
        /// Converts to representation.
        /// </summary>
        /// <param name="elementInitList">The list of <see cref="ElementInit" />.</param>
        /// <returns>
        /// Converted list of <see cref="ElementInitRepresentation" />.
        /// </returns>
        public static IReadOnlyList<ElementInitRepresentation> ToRepresentation(
            this IReadOnlyList<ElementInit> elementInitList)
        {
            if (elementInitList == null)
            {
                throw new ArgumentNullException(nameof(elementInitList));
            }

            if (elementInitList.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(elementInitList)}' contains an element that is null"));
            }

            var result = elementInitList.Select(_ => _.ToRepresentation()).ToList();

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="elementInitRepresentationList">The elementInitRepresentation.</param>
        /// <returns>
        /// Converted version.
        /// </returns>
        public static IReadOnlyList<ElementInit> FromRepresentation(
            this IReadOnlyList<ElementInitRepresentation> elementInitRepresentationList)
        {
            if (elementInitRepresentationList == null)
            {
                throw new ArgumentNullException(nameof(elementInitRepresentationList));
            }

            if (elementInitRepresentationList.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(elementInitRepresentationList)}' contains an element that is null"));
            }

            var result = elementInitRepresentationList.Select(_ => _.FromRepresentation()).ToList();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
