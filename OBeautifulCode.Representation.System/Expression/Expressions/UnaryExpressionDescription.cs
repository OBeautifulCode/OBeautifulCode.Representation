// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnaryExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="UnaryExpression" />.
    /// </summary>
    public class UnaryExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="UnaryExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="operand">The operand.</param>
        public UnaryExpressionDescription(TypeDescription type, ExpressionType nodeType, ExpressionDescriptionBase operand)
            : base(type, nodeType)
        {
            this.Operand = operand;
        }

        /// <summary>Gets the operand.</summary>
        /// <value>The operand.</value>
        public ExpressionDescriptionBase Operand { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="UnaryExpressionDescription" />.
    /// </summary>
    public static class UnaryExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="unaryExpression">The unary expression.</param>
        /// <returns>Serializable expression.</returns>
        public static UnaryExpressionDescription ToDescription(this UnaryExpression unaryExpression)
        {
            if (unaryExpression == null)
            {
                throw new ArgumentNullException(nameof(unaryExpression));
            }

            var type = unaryExpression.Type.ToDescription();
            var nodeType = unaryExpression.NodeType;
            var operand = unaryExpression.Operand.ToDescription();

            var result = new UnaryExpressionDescription(type, nodeType, operand);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="unaryExpressionDescription">The unary expression.</param>
        /// <returns>Converted expression.</returns>
        public static Expression FromDescription(this UnaryExpressionDescription unaryExpressionDescription)
        {
            if (unaryExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(unaryExpressionDescription));
            }

            var nodeType = unaryExpressionDescription.NodeType;
            switch (nodeType)
            {
                case ExpressionType.UnaryPlus:
                    return Expression.UnaryPlus(unaryExpressionDescription.Operand.FromDescription());
                default:
                    return Expression.MakeUnary(nodeType, unaryExpressionDescription.Operand.FromDescription(), unaryExpressionDescription.Type.ResolveFromLoadedTypes());
            }
        }
    }
}