// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeBinaryExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="TypeBinaryExpression" />.
    /// </summary>
    public class TypeBinaryExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="TypeBinaryExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        public TypeBinaryExpressionDescription(TypeDescription type, ExpressionDescriptionBase expression)
            : base(type, ExpressionType.TypeIs)
        {
            this.Expression = expression;
        }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public ExpressionDescriptionBase Expression { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="TypeBinaryExpressionDescription" />.
    /// </summary>
    public static class TypeBinaryExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="typeBinaryExpression">The typeBinary expression.</param>
        /// <returns>Serializable expression.</returns>
        public static TypeBinaryExpressionDescription ToDescription(this TypeBinaryExpression typeBinaryExpression)
        {
            if (typeBinaryExpression == null)
            {
                throw new ArgumentNullException(nameof(typeBinaryExpression));
            }

            var type = typeBinaryExpression.Type.ToDescription();
            var expression = typeBinaryExpression.Expression.ToDescription();
            var result = new TypeBinaryExpressionDescription(type, expression);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="typeBinaryExpressionDescription">The typeBinary expression.</param>
        /// <returns>Converted expression.</returns>
        public static Expression FromDescription(this TypeBinaryExpressionDescription typeBinaryExpressionDescription)
        {
            if (typeBinaryExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(typeBinaryExpressionDescription));
            }

            var type = typeBinaryExpressionDescription.Type.ResolveFromLoadedTypes();
            var expression = typeBinaryExpressionDescription.Expression.FromDescription();
            var result = Expression.TypeIs(expression, type);

            return result;
        }
    }
}
