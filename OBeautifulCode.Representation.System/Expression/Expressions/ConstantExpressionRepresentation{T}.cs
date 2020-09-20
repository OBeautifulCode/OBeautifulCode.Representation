// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ConstantExpression" />.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public partial class ConstantExpressionRepresentation<T> : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>Initializes a new instance of the <see cref="ConstantExpressionRepresentation{T}"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">The node type.</param>
        /// <param name="value">The value.</param>
        public ConstantExpressionRepresentation(
            TypeRepresentation type,
            ExpressionType nodeType,
            T value)
            : base(type, nodeType)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ConstantExpressionRepresentation{T}" />.
    /// </summary>
    public static class ConstantExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static ExpressionRepresentationBase ToRepresentation(
            this ConstantExpression constantExpression)
        {
            if (constantExpression == null)
            {
                throw new ArgumentNullException(nameof(constantExpression));
            }

            var type = constantExpression.Type.ToRepresentation();

            var value = constantExpression.Value;

            var resultType = typeof(ConstantExpressionRepresentation<>).MakeGenericType(value.GetType());

            var result = (ExpressionRepresentationBase)resultType.Construct(type, ExpressionType.Constant, value);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="constantExpressionRepresentation">The constant expression.</param>
        /// <typeparam name="T">Type of constant.</typeparam>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static ConstantExpression FromRepresentation<T>(
            this ConstantExpressionRepresentation<T> constantExpressionRepresentation)
            where T : ICloneable, IEquatable<T>
        {
            if (constantExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(constantExpressionRepresentation));
            }

            var result = Expression.Constant(constantExpressionRepresentation.Value);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}