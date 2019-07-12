// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>Description of <see cref="BinaryExpression" />.</summary>
    public class BinaryExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="BinaryExpressionDescription"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        public BinaryExpressionDescription(TypeDescription type, ExpressionType nodeType, ExpressionDescriptionBase left, ExpressionDescriptionBase right)
            : base(type, nodeType)
        {
            this.Left = left;
            this.Right = right;
        }

        /// <summary>Gets the left expression.</summary>
        /// <value>The left expression.</value>
        public ExpressionDescriptionBase Left { get; private set; }

        /// <summary>Gets the right expression.</summary>
        /// <value>The right expression.</value>
        public ExpressionDescriptionBase Right { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="BinaryExpressionDescription" />.
    /// </summary>
    public static class BinaryExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="binaryExpression">The binary expression.</param>
        /// <returns>The real expression.</returns>
        public static BinaryExpressionDescription ToDescription(this BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
            {
                throw new ArgumentNullException(nameof(binaryExpression));
            }

            var type = binaryExpression.Type.ToDescription();
            var nodeType = binaryExpression.NodeType;
            var left = binaryExpression.Left.ToDescription();
            var right = binaryExpression.Right.ToDescription();
            var result = new BinaryExpressionDescription(type, nodeType, left, right);
            return result;
        }

        /// <summary>
        /// Converts from serializable.
        /// </summary>
        /// <param name="binaryExpressionDescription">The binary expression.</param>
        /// <returns>The real expression.</returns>
        public static BinaryExpression FromDescription(this BinaryExpressionDescription binaryExpressionDescription)
        {
            if (binaryExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(binaryExpressionDescription));
            }

            return Expression.MakeBinary(binaryExpressionDescription.NodeType, binaryExpressionDescription.Left.FromDescription(), binaryExpressionDescription.Right.FromDescription());
        }
    }
}
