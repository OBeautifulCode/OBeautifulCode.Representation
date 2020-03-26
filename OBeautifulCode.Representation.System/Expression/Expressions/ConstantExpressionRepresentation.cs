// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ConstantExpression" />.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class ConstantExpressionRepresentation<T> : ExpressionRepresentationBase, IModel<ConstantExpressionRepresentation<T>>
        where T : ICloneable, IEquatable<T>
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

        /// <summary>
        /// Determines whether two objects of type <see cref="ConstantExpressionRepresentation{T}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; otherwise false.</returns>
        public static bool operator ==(ConstantExpressionRepresentation<T> left, ConstantExpressionRepresentation<T> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Value.IsEqualTo(right.Value)
                      && left.NodeType == right.NodeType
                      && left.Type == right.Type;

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="ConstantExpressionRepresentation{T}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; otherwise false.</returns>
        public static bool operator !=(ConstantExpressionRepresentation<T> left, ConstantExpressionRepresentation<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ConstantExpressionRepresentation<T> other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ConstantExpressionRepresentation<T>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Value)
            .Hash(this.NodeType)
            .Hash(this.Type)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public ConstantExpressionRepresentation<T> DeepClone()
        {
            var type = typeof(T);

            T deepClonedValue;

            if (type.IsValueType)
            {
                deepClonedValue = this.Value;
            }
            else
            {
                if (ReferenceEquals(this.Value, null))
                {
                    deepClonedValue = default(T);
                }
                else if (this.Value is IDeepCloneable<T> deepCloneable)
                {
                    deepClonedValue = deepCloneable.DeepClone();
                }
                else
                {
                    throw new NotSupportedException("I do not know how to deep clone object of type: " + type);
                }
            }

            // var result = new ConstantExpressionRepresentation<T>(
            //    this.Type?.DeepClone(),
            //    this.NodeType,
            //    deepClonedValue);
            return null;
        }
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
            new { constantExpression }.AsArg().Must().NotBeNull();

            var type = constantExpression.Type.ToRepresentation();

            var value = constantExpression.Value;

            var resultType = typeof(ConstantExpressionRepresentation<>).MakeGenericType(value.GetType());

            var result = (ExpressionRepresentationBase)resultType.Construct(value, ExpressionType.Constant, type);

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
            new { constantExpressionRepresentation }.AsArg().Must().NotBeNull();

            var result = Expression.Constant(constantExpressionRepresentation.Value);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}