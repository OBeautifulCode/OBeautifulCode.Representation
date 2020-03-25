// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeBinaryExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="TypeBinaryExpression" />.
    /// </summary>
    public partial class TypeBinaryExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBinaryExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        public TypeBinaryExpressionRepresentation(
            TypeRepresentation type,
            ExpressionRepresentationBase expression)
            : base(type, ExpressionType.TypeIs)
        {
            this.Expression = expression;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionRepresentationBase Expression { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="TypeBinaryExpressionRepresentation" />.
    /// </summary>
    public static class TypeBinaryExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="typeBinaryExpression">The typeBinary expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static TypeBinaryExpressionRepresentation ToRepresentation(
            this TypeBinaryExpression typeBinaryExpression)
        {
            new { typeBinaryExpression }.AsArg().Must().NotBeNull();

            var type = typeBinaryExpression.Type.ToRepresentation();

            var expression = typeBinaryExpression.Expression.ToRepresentation();

            var result = new TypeBinaryExpressionRepresentation(type, expression);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="typeBinaryExpressionRepresentation">The typeBinary expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static Expression FromRepresentation(
            this TypeBinaryExpressionRepresentation typeBinaryExpressionRepresentation)
        {
            new { typeBinaryExpressionRepresentation }.AsArg().Must().NotBeNull();

            var type = typeBinaryExpressionRepresentation.Type.ResolveFromLoadedTypes();

            var expression = typeBinaryExpressionRepresentation.Expression.FromRepresentation();

            var result = Expression.TypeIs(expression, type);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
