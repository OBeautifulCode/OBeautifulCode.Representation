// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Description of <see cref="ConstantExpression" />.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class ConstantExpressionDescription<T> : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="ConstantExpressionDescription{T}"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="value">The value.</param>
        public ConstantExpressionDescription(TypeDescription type, T value)
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
    /// Extensions to <see cref="ConstantExpressionDescription{T}" />.
    /// </summary>
    public static class ConstantExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>Converted expression.</returns>
        public static ExpressionDescriptionBase ToDescription(this ConstantExpression constantExpression)
        {
            if (constantExpression == null)
            {
                throw new ArgumentNullException(nameof(constantExpression));
            }

            var type = constantExpression.Type.ToDescription();
            var value = constantExpression.Value;
            var resultType = typeof(ConstantExpressionDescription<>).MakeGenericType(value.GetType());
            var result = resultType.Construct(type, value);
            return (ExpressionDescriptionBase)result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="constantExpressionDescription">The constant expression.</param>
        /// <typeparam name="T">Type of constant.</typeparam>
        /// <returns>Converted expression.</returns>
        public static ConstantExpression FromDescription<T>(this ConstantExpressionDescription<T> constantExpressionDescription)
        {
            if (constantExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(constantExpressionDescription));
            }

            var result = Expression.Constant(constantExpressionDescription.Value);
            return result;
        }
    }
}