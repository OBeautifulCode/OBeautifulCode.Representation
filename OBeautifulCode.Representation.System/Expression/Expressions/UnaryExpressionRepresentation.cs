// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnaryExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="UnaryExpression" />.
    /// </summary>
    public partial class UnaryExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="operand">The operand.</param>
        public UnaryExpressionRepresentation(
            TypeRepresentation type,
            ExpressionType nodeType,
            ExpressionRepresentationBase operand)
            : base(type, nodeType)
        {
            new { operand }.AsArg().Must().NotBeNull();

            this.Operand = operand;
        }

        /// <summary>
        /// Gets the operand.
        /// </summary>
        public ExpressionRepresentationBase Operand { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="UnaryExpressionRepresentation" />.
    /// </summary>
    public static class UnaryExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="unaryExpression">The unary expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static UnaryExpressionRepresentation ToRepresentation(
            this UnaryExpression unaryExpression)
        {
            new { unaryExpression }.AsArg().Must().NotBeNull();

            var type = unaryExpression.Type.ToRepresentation();

            var nodeType = unaryExpression.NodeType;

            var operand = unaryExpression.Operand.ToRepresentation();

            var result = new UnaryExpressionRepresentation(type, nodeType, operand);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="unaryExpressionRepresentation">The unary expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static Expression FromRepresentation(
            this UnaryExpressionRepresentation unaryExpressionRepresentation)
        {
            new { unaryExpressionRepresentation }.AsArg().Must().NotBeNull();

            var nodeType = unaryExpressionRepresentation.NodeType;

            switch (nodeType)
            {
                case ExpressionType.UnaryPlus:
                    return Expression.UnaryPlus(unaryExpressionRepresentation.Operand.FromRepresentation());
                default:
                    return Expression.MakeUnary(nodeType, unaryExpressionRepresentation.Operand.FromRepresentation(), unaryExpressionRepresentation.Type.ResolveFromLoadedTypes());
            }
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}