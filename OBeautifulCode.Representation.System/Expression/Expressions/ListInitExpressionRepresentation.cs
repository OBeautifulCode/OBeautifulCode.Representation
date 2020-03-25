// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInitExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="ListInitExpression" />.
    /// </summary>
    /// <seealso cref="ExpressionRepresentationBase" />
    public partial class ListInitExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListInitExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="newExpressionRepresentation">The new expression.</param>
        /// <param name="initializers">The initializers.</param>
        public ListInitExpressionRepresentation(
            TypeRepresentation type,
            NewExpressionRepresentation newExpressionRepresentation,
            IReadOnlyList<ElementInitRepresentation> initializers)
            : base(type, ExpressionType.ListInit)
        {
            this.NewExpressionRepresentation = newExpressionRepresentation;
            this.Initializers = initializers;
        }

        /// <summary>
        /// Gets the new expression representation.
        /// </summary>
        public NewExpressionRepresentation NewExpressionRepresentation { get; private set; }

        /// <summary>
        /// Gets the initializers.
        /// </summary>
        public IReadOnlyList<ElementInitRepresentation> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ListInitExpressionRepresentation" />.
    /// </summary>
    public static class ListInitExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="listInitExpression">The listInit expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static ListInitExpressionRepresentation ToRepresentation(
            this ListInitExpression listInitExpression)
        {
            new { listInitExpression }.AsArg().Must().NotBeNull();

            var type = listInitExpression.Type.ToRepresentation();

            var newExpression = listInitExpression.NewExpression.ToRepresentation();

            var initializers = listInitExpression.Initializers.ToRepresentation();

            var result = new ListInitExpressionRepresentation(type, newExpression, initializers);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="listInitExpressionRepresentation">The listInit expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static ListInitExpression FromRepresentation(this ListInitExpressionRepresentation listInitExpressionRepresentation)
        {
            new { listInitExpressionRepresentation }.AsArg().Must().NotBeNull();

            var result = Expression.ListInit(
                listInitExpressionRepresentation.NewExpressionRepresentation.FromRepresentation(),
                listInitExpressionRepresentation.Initializers.FromRepresentation().ToArray());

            return result;
        }
#pragma warning restore SA1204 // Static elements should appear before instance elements
    }
}
