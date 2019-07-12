// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionDescriptionBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using static System.FormattableString;

    /// <summary>
    /// Description of <see cref="Expression" />.
    /// </summary>
    public abstract class ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="ExpressionDescriptionBase"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">The node type.</param>
        protected ExpressionDescriptionBase(TypeDescription type, ExpressionType nodeType)
        {
            this.Type = type;
            this.NodeType = nodeType;
        }

        /// <summary>Gets the type of the node.</summary>
        /// <value>The type of the node.</value>
        public ExpressionType NodeType { get; private set; }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeDescription Type { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="ExpressionDescriptionBase" />.
                              /// </summary>
    public static class ExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to a serializable.</summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Serializable expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by its very nature.")]
        public static ExpressionDescriptionBase ToDescription(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (expression is BinaryExpression binaryExpression)
            {
                return binaryExpression.ToDescription();
            }
            else if (expression is ConditionalExpression conditionalExpression)
            {
                return conditionalExpression.ToDescription();
            }
            else if (expression is ConstantExpression constantExpression)
            {
                return constantExpression.ToDescription();
            }
            else if (expression is InvocationExpression invocationExpression)
            {
                return invocationExpression.ToDescription();
            }
            else if (expression is LambdaExpression lambdaExpression)
            {
                return lambdaExpression.ToDescription();
            }
            else if (expression is ListInitExpression listInitExpression)
            {
                return listInitExpression.ToDescription();
            }
            else if (expression is MemberExpression memberExpression)
            {
                return memberExpression.ToDescription();
            }
            else if (expression is MemberInitExpression memberInitExpression)
            {
                return memberInitExpression.ToDescription();
            }
            else if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.ToDescription();
            }
            else if (expression is NewArrayExpression newArrayExpression)
            {
                return newArrayExpression.ToDescription();
            }
            else if (expression is NewExpression newExpression)
            {
                return newExpression.ToDescription();
            }
            else if (expression is ParameterExpression parameterExpression)
            {
                return parameterExpression.ToDescription();
            }
            else if (expression is TypeBinaryExpression typeBinaryExpression)
            {
                return typeBinaryExpression.ToDescription();
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                return unaryExpression.ToDescription();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Expression type '{expression.GetType()}' is not supported."));
            }
        }

        /// <summary>Converts from serializable.</summary>
        /// <param name="expressionDescription">The serializable expression.</param>
        /// <returns>Converted version.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by its very nature.")]
        public static Expression FromDescription(this ExpressionDescriptionBase expressionDescription)
        {
            if (expressionDescription == null)
            {
                throw new ArgumentNullException(nameof(expressionDescription));
            }

            if (expressionDescription is BinaryExpressionDescription binaryExpression)
            {
                return binaryExpression.FromDescription();
            }
            else if (expressionDescription is ConditionalExpressionDescription conditionalExpression)
            {
                return conditionalExpression.FromDescription();
            }
            else if (expressionDescription.GetType().IsGenericType && expressionDescription.GetType().GetGenericTypeDefinition() == typeof(ConstantExpressionDescription<>))
            {
                var type = expressionDescription.Type.ResolveFromLoadedTypes();
                var conversionMethodGeneric =
                    typeof(ConstantExpressionDescriptionExtensions).GetMethod(
                        nameof(ConstantExpressionDescriptionExtensions.FromDescription)) ??
                    throw new ArgumentException(Invariant(
                        $"Method '{nameof(ConstantExpressionDescriptionExtensions)}.{nameof(ConstantExpressionDescriptionExtensions.FromDescription)}' should be there."));

                var conversionMethodReal = conversionMethodGeneric.MakeGenericMethod(type);
                var resultRaw = conversionMethodReal.Invoke(null, new[] { expressionDescription });
                return (ConstantExpression)resultRaw;
            }
            else if (expressionDescription is InvocationExpressionDescription invocationExpression)
            {
                return invocationExpression.FromDescription();
            }
            else if (expressionDescription is LambdaExpressionDescription lambdaExpression)
            {
                return lambdaExpression.FromDescription();
            }
            else if (expressionDescription is ListInitExpressionDescription listInitExpression)
            {
                return listInitExpression.FromDescription();
            }
            else if (expressionDescription is MemberExpressionDescription memberExpression)
            {
                return memberExpression.FromDescription();
            }
            else if (expressionDescription is MemberInitExpressionDescription memberInitExpression)
            {
                return memberInitExpression.FromDescription();
            }
            else if (expressionDescription is MethodCallExpressionDescription methodCallExpression)
            {
                return methodCallExpression.FromDescription();
            }
            else if (expressionDescription is NewArrayExpressionDescription newArrayExpression)
            {
                return newArrayExpression.FromDescription();
            }
            else if (expressionDescription is NewExpressionDescription newExpression)
            {
                return newExpression.FromDescription();
            }
            else if (expressionDescription is ParameterExpressionDescription parameterExpression)
            {
                return parameterExpression.FromDescription();
            }
            else if (expressionDescription is TypeBinaryExpressionDescription typeBinaryExpression)
            {
                return typeBinaryExpression.FromDescription();
            }
            else if (expressionDescription is UnaryExpressionDescription unaryExpression)
            {
                return unaryExpression.FromDescription();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Expression type '{expressionDescription.GetType()}' is not supported."));
            }
        }

        /// <summary>Converts to serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<ExpressionDescriptionBase> ToDescription(
            this IReadOnlyList<Expression> expressions)
        {
            var result = expressions.Select(_ => _.ToDescription()).ToList();
            return result;
        }

        /// <summary>Froms the serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<Expression> FromDescription(
            this IReadOnlyList<ExpressionDescriptionBase> expressions)
        {
            var result = expressions.Select(_ => _.FromDescription()).ToList();
            return result;
        }
    }
}
