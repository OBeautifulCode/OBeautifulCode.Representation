// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="BinaryExpression" />.
    /// </summary>
    public partial class BinaryExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        public BinaryExpressionRepresentation(
            TypeRepresentation type,
            ExpressionType nodeType,
            ExpressionRepresentationBase left,
            ExpressionRepresentationBase right)
            : base(type, nodeType)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Gets the left expression.
        /// </summary>
        public ExpressionRepresentationBase Left { get; private set; }

        /// <summary>
        /// Gets the right expression.
        /// </summary>
        public ExpressionRepresentationBase Right { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="BinaryExpressionRepresentation" />.
    /// </summary>
    public static class BinaryExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="binaryExpression">The binary expression.</param>
        /// <returns>
        /// The real expression.
        /// </returns>
        public static BinaryExpressionRepresentation ToRepresentation(
            this BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
            {
                throw new ArgumentNullException(nameof(binaryExpression));
            }

            var type = binaryExpression.Type.ToRepresentation();

            var nodeType = binaryExpression.NodeType;

            var left = binaryExpression.Left.ToRepresentation();

            var right = binaryExpression.Right.ToRepresentation();

            var result = new BinaryExpressionRepresentation(type, nodeType, left, right);

            return result;
        }

        /// <summary>
        /// Converts from serializable.
        /// </summary>
        /// <param name="binaryExpressionRepresentation">The binary expression.</param>
        /// <returns>
        /// The real expression.
        /// </returns>
        public static BinaryExpression FromRepresentation(
            this BinaryExpressionRepresentation binaryExpressionRepresentation)
        {
            if (binaryExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(binaryExpressionRepresentation));
            }

            var result = Expression.MakeBinary(binaryExpressionRepresentation.NodeType, binaryExpressionRepresentation.Left.FromRepresentation(), binaryExpressionRepresentation.Right.FromRepresentation());

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
