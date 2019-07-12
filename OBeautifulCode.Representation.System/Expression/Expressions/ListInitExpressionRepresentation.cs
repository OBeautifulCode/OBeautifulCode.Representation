// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInitExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="ListInitExpression" />.
    /// </summary>
    /// <seealso cref="ExpressionRepresentationBase" />
    public class ListInitExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="ListInitExpressionRepresentation"/> class.</summary>
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

        /// <summary>Gets the new expression description.</summary>
        /// <value>The new expression description.</value>
        public NewExpressionRepresentation NewExpressionRepresentation { get; private set; }

        /// <summary>Gets the initializers.</summary>
        /// <value>The initializers.</value>
        public IReadOnlyList<ElementInitRepresentation> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="ListInitExpressionRepresentation" />.
                              /// </summary>
    public static class ListInitExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="listInitExpression">The listInit expression.</param>
        /// <returns>Serializable expression.</returns>
        public static ListInitExpressionRepresentation ToRepresentation(this ListInitExpression listInitExpression)
        {
            if (listInitExpression == null)
            {
                throw new ArgumentNullException(nameof(listInitExpression));
            }

            var type = listInitExpression.Type.ToRepresentation();
            var newExpression = listInitExpression.NewExpression.ToRepresentation();
            var initializers = listInitExpression.Initializers.ToRepresentation();
            var result = new ListInitExpressionRepresentation(type, newExpression, initializers);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="listInitExpressionRepresentation">The listInit expression.</param>
        /// <returns>Converted expression.</returns>
        public static ListInitExpression FromRepresentation(this ListInitExpressionRepresentation listInitExpressionRepresentation)
        {
            if (listInitExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(listInitExpressionRepresentation));
            }

            var result = Expression.ListInit(
                listInitExpressionRepresentation.NewExpressionRepresentation.FromRepresentation(),
                listInitExpressionRepresentation.Initializers.FromRepresentation().ToArray());

            return result;
        }
    }
}
