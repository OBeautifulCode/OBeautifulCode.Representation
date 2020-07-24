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
    [Serializable]
    public class ConstantExpressionRepresentation<T> : ExpressionRepresentationBase, IModel<ConstantExpressionRepresentation<T>>
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
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
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

            var result = left.Equals(right);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="ConstantExpressionRepresentation{T}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(ConstantExpressionRepresentation<T> left, ConstantExpressionRepresentation<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ConstantExpressionRepresentation<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.NodeType.IsEqualTo(other.NodeType)
                      && this.Type.IsEqualTo(other.Type)
                      && this.Value.IsEqualTo(other.Value);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ConstantExpressionRepresentation<T>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Value)
            .Hash(this.NodeType)
            .Hash(this.Type)
            .Value;

        /// <inheritdoc />
        public new ConstantExpressionRepresentation<T> DeepClone() => (ConstantExpressionRepresentation<T>)this.DeepCloneInternal();

        /// <inheritdoc />
        public override ExpressionRepresentationBase DeepCloneWithNodeType(ExpressionType nodeType)
        {
            var result = new ConstantExpressionRepresentation<T>(
                this.Type?.DeepClone(),
                nodeType,
                this.DeepCloneValue());

            return result;
        }

        /// <inheritdoc />
        public override ExpressionRepresentationBase DeepCloneWithType(TypeRepresentation type)
        {
            var result = new ConstantExpressionRepresentation<T>(
                type,
                this.NodeType,
                this.DeepCloneValue());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Value" />.
        /// </summary>
        /// <param name="value">The new <see cref="Value" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ConstantExpressionRepresentation{T}" /> using the specified <paramref name="value" /> for <see cref="Value" /> and a deep clone of every other property.</returns>
        public ConstantExpressionRepresentation<T> DeepCloneWithValue(T value)
        {
            var result = new ConstantExpressionRepresentation<T>(
                                 this.Type?.DeepClone(),
                                 this.NodeType,
                                 value);

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Representation.System.ConstantExpressionRepresentation{typeof(T).ToStringCompilable()}: NodeType = {this.NodeType.ToString() ?? "<null>"}, Type = {this.Type?.ToString() ?? "<null>"}, Value = {this.Value?.ToString() ?? "<null>"}.");

            return result;
        }

        /// <inheritdoc />
        protected override ExpressionRepresentationBase DeepCloneInternal()
        {
            var deepClonedValue = this.DeepCloneValue();

            var result = new ConstantExpressionRepresentation<T>(
               this.Type?.DeepClone(),
               this.NodeType,
               deepClonedValue);

            return result;
        }

        private T DeepCloneValue()
        {
            T result;

            var type = typeof(T);

            if (type.IsValueType)
            {
                result = this.Value;
            }
            else
            {
                if (ReferenceEquals(this.Value, null))
                {
                    result = default(T);
                }
                else if (this.Value is IDeepCloneable<T> deepCloneableValue)
                {
                    result = deepCloneableValue.DeepClone();
                }
                else if (this.Value is string stringValue)
                {
                    result = (T)(object)stringValue.Clone().ToString();
                }
                else
                {
                    throw new NotSupportedException("I do not know how to deep clone object of type: " + type);
                }
            }

            return result;
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