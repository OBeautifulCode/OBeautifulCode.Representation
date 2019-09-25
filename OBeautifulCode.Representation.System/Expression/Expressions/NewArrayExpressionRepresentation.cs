// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewArrayExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq.Expressions;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="NewArrayExpression" />.
    /// </summary>
    public class NewArrayExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="NewArrayExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="expressions">The expressions.</param>
        public NewArrayExpressionRepresentation(
            TypeRepresentation type,
            ExpressionType nodeType,
            IReadOnlyList<ExpressionRepresentationBase> expressions)
            : base(type, nodeType)
        {
            this.Expressions = expressions;
        }

        /// <summary>Gets the expressions.</summary>
        /// <value>The expressions.</value>
        public IReadOnlyList<ExpressionRepresentationBase> Expressions { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="NewArrayExpressionRepresentation" />.
    /// </summary>
    public static class NewArrayExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="newArrayExpression">The newArray expression.</param>
        /// <returns>Serializable expression.</returns>
        public static NewArrayExpressionRepresentation ToRepresentation(this NewArrayExpression newArrayExpression)
        {
            if (newArrayExpression == null)
            {
                throw new ArgumentNullException(nameof(newArrayExpression));
            }

            var type = newArrayExpression.Type.ToRepresentation();
            var nodeType = newArrayExpression.NodeType;
            var expressions = newArrayExpression.Expressions.ToRepresentation();
            var result = new NewArrayExpressionRepresentation(type, nodeType, expressions);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="newArrayExpressionRepresentation">The newArray expression.</param>
        /// <returns>Converted expression.</returns>
        public static NewArrayExpression FromRepresentation(this NewArrayExpressionRepresentation newArrayExpressionRepresentation)
        {
            if (newArrayExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(newArrayExpressionRepresentation));
            }

            NewArrayExpression result;
            var nodeType = newArrayExpressionRepresentation.NodeType;
            switch (nodeType)
            {
                case ExpressionType.NewArrayBounds:
                    result = Expression.NewArrayBounds(newArrayExpressionRepresentation.Type.ResolveFromLoadedTypes(), newArrayExpressionRepresentation.Expressions.FromRepresentation());
                    break;
                case ExpressionType.NewArrayInit:
                    result = Expression.NewArrayInit(newArrayExpressionRepresentation.Type.ResolveFromLoadedTypes(), newArrayExpressionRepresentation.Expressions.FromRepresentation());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(newArrayExpressionRepresentation.NodeType)} '{nodeType}' is not supported."));
            }

            return result;
        }
    }
}
