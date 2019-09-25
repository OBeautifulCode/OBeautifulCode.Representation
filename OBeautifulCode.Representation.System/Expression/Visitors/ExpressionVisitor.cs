// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionVisitor.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;
    using global::System.Linq.Expressions;

    /// <summary>
    /// Expression tree visitor for <see cref="Expression" />'s.
    /// </summary>
    public static class ExpressionVisitor
    {
        /// <summary>Visits all connected nodes.</summary>
        /// <param name="root">The root.</param>
        /// <returns>Collection of the connected nodes.</returns>
        public static IReadOnlyCollection<Expression> VisitAllConnectedNodes(this Expression root)
        {
            var result = new List<Expression>();
            switch (root)
            {
                case LambdaExpression lambdaExpression:
                    result.Add(lambdaExpression.Body);
                    result.AddRange(lambdaExpression.Parameters);

                    break;
                case BinaryExpression binaryExpression:
                    result.Add(binaryExpression.Left);
                    result.Add(binaryExpression.Right);
                    break;
                case ConditionalExpression conditionalExpression:
                    result.Add(conditionalExpression.IfTrue);
                    result.Add(conditionalExpression.IfFalse);
                    result.Add(conditionalExpression.Test);
                    break;
                case InvocationExpression invocationExpression:
                    {
                        result.AddRange(invocationExpression.Arguments);

                        break;
                    }

                case ListInitExpression listInitExpression:
                    result.Add(listInitExpression.NewExpression);
                    foreach (var x in listInitExpression.Initializers)
                    {
                        result.AddRange(x.Arguments);
                    }

                    break;
                case MemberExpression memberExpression:
                    result.Add(memberExpression.Expression);
                    break;
                case MemberInitExpression memberInitExpression:
                    result.Add(memberInitExpression.NewExpression);
                    break;
                case MethodCallExpression methodCallExpression:
                    result.AddRange(methodCallExpression.Arguments);
                    result.Add(methodCallExpression.Object);
                    break;
                case NewArrayExpression newArrayExpression:
                    result.AddRange(newArrayExpression.Expressions);
                    break;

                case NewExpression newExpression:
                    {
                        result.AddRange(newExpression.Arguments);

                        break;
                    }

                case TypeBinaryExpression typeBinaryExpression:
                    result.Add(typeBinaryExpression.Expression);
                    break;
                case UnaryExpression unaryExpression:
                    result.Add(unaryExpression.Operand);
                    break;
            }

            return result;
        }

        /// <summary>Visits all nodes in the tree.</summary>
        /// <param name="root">Node traverse from.</param>
        /// <returns>Collection of the <see cref="Expression" /> from the tree.</returns>
        public static IReadOnlyCollection<Expression> VisitAllNodes(this Expression root)
        {
            var result = new List<Expression>();
            foreach (var linkNode in root.VisitAllConnectedNodes())
            {
                result.AddRange(linkNode.VisitAllNodes());
            }

            result.Add(root);
            return result;
        }
    }
}
