// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionRepresentationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="Expression" />.
    /// </summary>
    public abstract partial class ExpressionRepresentationBase : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionRepresentationBase"/> class.
        /// </summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">The node type.</param>
        protected ExpressionRepresentationBase(
            TypeRepresentation type,
            ExpressionType nodeType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Type = type;
            this.NodeType = nodeType;
        }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        public ExpressionType NodeType { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ExpressionRepresentationBase" />.
    /// </summary>
    public static class ExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to a serializable.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by its very nature.")]
        public static ExpressionRepresentationBase ToRepresentation(
            this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (expression is BinaryExpression binaryExpression)
            {
                return binaryExpression.ToRepresentation();
            }
            else if (expression is ConditionalExpression conditionalExpression)
            {
                return conditionalExpression.ToRepresentation();
            }
            else if (expression is ConstantExpression constantExpression)
            {
                return constantExpression.ToRepresentation();
            }
            else if (expression is InvocationExpression invocationExpression)
            {
                return invocationExpression.ToRepresentation();
            }
            else if (expression is LambdaExpression lambdaExpression)
            {
                return lambdaExpression.ToRepresentation();
            }
            else if (expression is ListInitExpression listInitExpression)
            {
                return listInitExpression.ToRepresentation();
            }
            else if (expression is MemberExpression memberExpression)
            {
                return memberExpression.ToRepresentation();
            }
            else if (expression is MemberInitExpression memberInitExpression)
            {
                return memberInitExpression.ToRepresentation();
            }
            else if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.ToRepresentation();
            }
            else if (expression is NewArrayExpression newArrayExpression)
            {
                return newArrayExpression.ToRepresentation();
            }
            else if (expression is NewExpression newExpression)
            {
                return newExpression.ToRepresentation();
            }
            else if (expression is ParameterExpression parameterExpression)
            {
                return parameterExpression.ToRepresentation();
            }
            else if (expression is TypeBinaryExpression typeBinaryExpression)
            {
                return typeBinaryExpression.ToRepresentation();
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                return unaryExpression.ToRepresentation();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Expression type '{expression.GetType()}' is not supported."));
            }
        }

        /// <summary>
        /// Converts from serializable.
        /// </summary>
        /// <param name="expressionRepresentation">The serializable expression.</param>
        /// <returns>
        /// Converted version.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by its very nature.")]
        public static Expression FromRepresentation(
            this ExpressionRepresentationBase expressionRepresentation)
        {
            if (expressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(expressionRepresentation));
            }

            if (expressionRepresentation is BinaryExpressionRepresentation binaryExpression)
            {
                return binaryExpression.FromRepresentation();
            }
            else if (expressionRepresentation is ConditionalExpressionRepresentation conditionalExpression)
            {
                return conditionalExpression.FromRepresentation();
            }
            else if (expressionRepresentation.GetType().IsGenericType && expressionRepresentation.GetType().GetGenericTypeDefinition() == typeof(ConstantExpressionRepresentation<>))
            {
                var type = expressionRepresentation.Type.ResolveFromLoadedTypes();
                var conversionMethodGeneric =
                    typeof(ConstantExpressionRepresentationExtensions).GetMethod(
                        nameof(ConstantExpressionRepresentationExtensions.FromRepresentation)) ??
                    throw new ArgumentException(Invariant(
                        $"Method '{nameof(ConstantExpressionRepresentationExtensions)}.{nameof(ConstantExpressionRepresentationExtensions.FromRepresentation)}' should be there."));

                var conversionMethodReal = conversionMethodGeneric.MakeGenericMethod(type);
                var resultRaw = conversionMethodReal.Invoke(null, new[] { expressionRepresentation });
                return (ConstantExpression)resultRaw;
            }
            else if (expressionRepresentation is InvocationExpressionRepresentation invocationExpression)
            {
                return invocationExpression.FromRepresentation();
            }
            else if (expressionRepresentation is LambdaExpressionRepresentation lambdaExpression)
            {
                return lambdaExpression.FromRepresentation();
            }
            else if (expressionRepresentation is ListInitExpressionRepresentation listInitExpression)
            {
                return listInitExpression.FromRepresentation();
            }
            else if (expressionRepresentation is MemberExpressionRepresentation memberExpression)
            {
                return memberExpression.FromRepresentation();
            }
            else if (expressionRepresentation is MemberInitExpressionRepresentation memberInitExpression)
            {
                return memberInitExpression.FromRepresentation();
            }
            else if (expressionRepresentation is MethodCallExpressionRepresentation methodCallExpression)
            {
                return methodCallExpression.FromRepresentation();
            }
            else if (expressionRepresentation is NewArrayExpressionRepresentation newArrayExpression)
            {
                return newArrayExpression.FromRepresentation();
            }
            else if (expressionRepresentation is NewExpressionRepresentation newExpression)
            {
                return newExpression.FromRepresentation();
            }
            else if (expressionRepresentation is ParameterExpressionRepresentation parameterExpression)
            {
                return parameterExpression.FromRepresentation();
            }
            else if (expressionRepresentation is TypeBinaryExpressionRepresentation typeBinaryExpression)
            {
                return typeBinaryExpression.FromRepresentation();
            }
            else if (expressionRepresentation is UnaryExpressionRepresentation unaryExpression)
            {
                return unaryExpression.FromRepresentation();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Expression type '{expressionRepresentation.GetType()}' is not supported."));
            }
        }

        /// <summary>Converts to serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<ExpressionRepresentationBase> ToRepresentation(
            this IReadOnlyList<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            if (expressions.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(expressions)}' contains at least one null element"));
            }

            var result = expressions.Select(_ => _.ToRepresentation()).ToList();

            return result;
        }

        /// <summary>Froms the serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<Expression> FromRepresentation(
            this IReadOnlyList<ExpressionRepresentationBase> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            if (expressions.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(expressions)}' contains at least one null element"));
            }

            var result = expressions.Select(_ => _.FromRepresentation()).ToList();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
