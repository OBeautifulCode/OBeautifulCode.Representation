// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Representation of <see cref="ConstantExpression" />.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class ConstantExpressionRepresentation<T> : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="ConstantExpressionRepresentation{T}"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="value">The value.</param>
        public ConstantExpressionRepresentation(TypeRepresentation type, T value)
            : base(type, ExpressionType.Constant)
        {
            this.Value = value;
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public T Value { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="ConstantExpressionRepresentation{T}" />.
    /// </summary>
    public static class ConstantExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>Converted expression.</returns>
        public static ExpressionRepresentationBase ToRepresentation(this ConstantExpression constantExpression)
        {
            if (constantExpression == null)
            {
                throw new ArgumentNullException(nameof(constantExpression));
            }

            var type = constantExpression.Type.ToRepresentation();
            var value = constantExpression.Value;
            var resultType = typeof(ConstantExpressionRepresentation<>).MakeGenericType(value.GetType());
            var result = resultType.Construct(type, value);
            return (ExpressionRepresentationBase)result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="constantExpressionRepresentation">The constant expression.</param>
        /// <typeparam name="T">Type of constant.</typeparam>
        /// <returns>Converted expression.</returns>
        public static ConstantExpression FromRepresentation<T>(this ConstantExpressionRepresentation<T> constantExpressionRepresentation)
        {
            if (constantExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(constantExpressionRepresentation));
            }

            var result = Expression.Constant(constantExpressionRepresentation.Value);
            return result;
        }
    }
}