// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation.cs" company="OBeautifulCode">
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

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ConstantExpression" />.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public class ConstantExpressionRepresentation<T> : ExpressionRepresentationBase, IModel<ConstantExpressionRepresentation<T>>
        where T : ICloneable, IEquatable<T>
    {
        /// <summary>Initializes a new instance of the <see cref="ConstantExpressionRepresentation{T}"/> class.</summary>
        /// <param name="value">The value.</param>
        /// <param name="nodeType">The node type.</param>
        /// <param name="type">The type of expression.</param>
        public ConstantExpressionRepresentation(
            T                  value,
            ExpressionType     nodeType,
            TypeRepresentation type)
            : base(type, nodeType)
        {
            this.Value = value;
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
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

            var result = left.Value.Equals(right.Value)
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
            var result = new ConstantExpressionRepresentation<T>(
                                 (T)this.Value?.Clone(),
                                 this.NodeType,
                                 this.Type.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="value" />.
        /// </summary>
        /// <param name="value">The new <see cref="Value" />.</param>
        /// <returns>New <see cref="ConstantExpressionRepresentation{T}" /> using the specified <paramref name="value" /> for <see cref="Value" /> and a deep clone of every other property.</returns>
        public ConstantExpressionRepresentation<T> DeepCloneWithValue(T value)
        {
            var result = new ConstantExpressionRepresentation<T>(
                                 value,
                                 this.NodeType,
                                 this.Type);
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="nodeType" />.
        /// </summary>
        /// <param name="nodeType">The new <see cref="ExpressionType" />.</param>
        /// <returns>New <see cref="ConstantExpressionRepresentation{T}" /> using the specified <paramref name="nodeType" /> for <see cref="ExpressionType" /> and a deep clone of every other property.</returns>
        public ConstantExpressionRepresentation<T> DeepCloneWithNodeType(ExpressionType nodeType)
        {
            var result = new ConstantExpressionRepresentation<T>(
                                 this.Value,
                                 nodeType,
                                 this.Type);
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="type" />.
        /// </summary>
        /// <param name="type">The new <see cref="Type" />.</param>
        /// <returns>New <see cref="ConstantExpressionRepresentation{T}" /> using the specified <paramref name="type" /> for <see cref="Type" /> and a deep clone of every other property.</returns>
        public ConstantExpressionRepresentation<T> DeepCloneWithType(TypeRepresentation type)
        {
            var result = new ConstantExpressionRepresentation<T>(
                                 (T)this.Value?.Clone(),
                                 this.NodeType,
                                 type);
            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"{nameof(OBeautifulCode.Representation)}.{nameof(ConstantExpressionRepresentation<T>)}: Value = {this.Value?.ToString() ?? "<null>"}, NodeType = {this.NodeType.ToString() ?? "<null>"}, Type = {this.Type.ToString() ?? "<null>"}.");

            return result;
        }
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
            var result = resultType.Construct(value, ExpressionType.Constant, type);
            return (ExpressionRepresentationBase)result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="constantExpressionRepresentation">The constant expression.</param>
        /// <typeparam name="T">Type of constant.</typeparam>
        /// <returns>Converted expression.</returns>
        public static ConstantExpression FromRepresentation<T>(this ConstantExpressionRepresentation<T> constantExpressionRepresentation)
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
}