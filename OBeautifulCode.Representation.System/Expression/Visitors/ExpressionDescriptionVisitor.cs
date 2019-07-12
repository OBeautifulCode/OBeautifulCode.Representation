// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionDescriptionVisitor.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System.Collections.Generic;

    /// <summary>
    /// Expression tree visitor for <see cref="ExpressionDescriptionBase" />'s.
    /// </summary>
    public static class ExpressionDescriptionVisitor
    {
        /// <summary>Visits all connected nodes.</summary>
        /// <param name="root">The root.</param>
        /// <returns>Collection of the connected nodes.</returns>
        public static IReadOnlyCollection<ExpressionDescriptionBase> VisitAllConnectedNodes(this ExpressionDescriptionBase root)
        {
            var result = new List<ExpressionDescriptionBase>();
            switch (root)
            {
                case LambdaExpressionDescription lambdaExpressionDescription:
                    result.Add(lambdaExpressionDescription.Body);
                    result.AddRange(lambdaExpressionDescription.Parameters);

                    break;
                case BinaryExpressionDescription binaryExpressionDescription:
                    result.Add(binaryExpressionDescription.Left);
                    result.Add(binaryExpressionDescription.Right);
                    break;
                case ConditionalExpressionDescription conditionalExpressionDescription:
                    result.Add(conditionalExpressionDescription.IfTrue);
                    result.Add(conditionalExpressionDescription.IfFalse);
                    result.Add(conditionalExpressionDescription.Test);
                    break;
                case InvocationExpressionDescription invocationExpressionDescription:
                {
                    result.AddRange(invocationExpressionDescription.Arguments);

                    break;
                }

                case ListInitExpressionDescription listInitExpressionDescription:
                    result.Add(listInitExpressionDescription.NewExpressionDescription);
                    foreach (var x in listInitExpressionDescription.Initializers)
                    {
                        result.AddRange(x.Arguments);
                    }

                    break;
                case MemberExpressionDescription memberExpressionDescription:
                    result.Add(memberExpressionDescription.Expression);
                    break;
                case MemberInitExpressionDescription memberInitExpressionDescription:
                    result.Add(memberInitExpressionDescription.NewExpressionDescription);
                    break;
                case MethodCallExpressionDescription methodCallExpressionDescription:
                    result.AddRange(methodCallExpressionDescription.Arguments);
                    result.Add(methodCallExpressionDescription.ParentObject);
                    break;
                case NewArrayExpressionDescription newArrayExpressionDescription:
                    result.AddRange(newArrayExpressionDescription.Expressions);
                    break;

                case NewExpressionDescription newExpressionDescription:
                {
                    result.AddRange(newExpressionDescription.Arguments);

                    break;
                }

                case TypeBinaryExpressionDescription typeBinaryExpressionDescription:
                    result.Add(typeBinaryExpressionDescription.Expression);
                    break;
                case UnaryExpressionDescription unaryExpressionDescription:
                    result.Add(unaryExpressionDescription.Operand);
                    break;
            }

            return result;
        }

        /// <summary>Visits all nodes in the tree.</summary>
        /// <param name="root">Node traverse from.</param>
        /// <returns>Collection of the <see cref="ExpressionDescriptionBase" /> from the tree.</returns>
        public static IReadOnlyCollection<ExpressionDescriptionBase> VisitAllNodes(this ExpressionDescriptionBase root)
        {
            var result = new List<ExpressionDescriptionBase>();
            foreach (var linkNode in root.VisitAllConnectedNodes())
            {
                result.AddRange(linkNode.VisitAllNodes());
            }

            result.Add(root);
            return result;
        }
    }
}
