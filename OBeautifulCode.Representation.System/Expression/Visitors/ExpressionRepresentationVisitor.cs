// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionRepresentationVisitor.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;

    /// <summary>
    /// Expression tree visitor for <see cref="ExpressionRepresentationBase" />'s.
    /// </summary>
    public static class ExpressionRepresentationVisitor
    {
        /// <summary>Visits all connected nodes.</summary>
        /// <param name="root">The root.</param>
        /// <returns>Collection of the connected nodes.</returns>
        public static IReadOnlyCollection<ExpressionRepresentationBase> VisitAllConnectedNodes(this ExpressionRepresentationBase root)
        {
            var result = new List<ExpressionRepresentationBase>();
            switch (root)
            {
                case LambdaExpressionRepresentation lambdaExpressionRepresentation:
                    result.Add(lambdaExpressionRepresentation.Body);
                    result.AddRange(lambdaExpressionRepresentation.Parameters);

                    break;
                case BinaryExpressionRepresentation binaryExpressionRepresentation:
                    result.Add(binaryExpressionRepresentation.Left);
                    result.Add(binaryExpressionRepresentation.Right);
                    break;
                case ConditionalExpressionRepresentation conditionalExpressionRepresentation:
                    result.Add(conditionalExpressionRepresentation.IfTrue);
                    result.Add(conditionalExpressionRepresentation.IfFalse);
                    result.Add(conditionalExpressionRepresentation.Test);
                    break;
                case InvocationExpressionRepresentation invocationExpressionRepresentation:
                {
                    result.AddRange(invocationExpressionRepresentation.Arguments);

                    break;
                }

                case ListInitExpressionRepresentation listInitExpressionRepresentation:
                    result.Add(listInitExpressionRepresentation.NewExpressionRepresentation);
                    foreach (var x in listInitExpressionRepresentation.Initializers)
                    {
                        result.AddRange(x.Arguments);
                    }

                    break;
                case MemberExpressionRepresentation memberExpressionRepresentation:
                    result.Add(memberExpressionRepresentation.Expression);
                    break;
                case MemberInitExpressionRepresentation memberInitExpressionRepresentation:
                    result.Add(memberInitExpressionRepresentation.NewExpressionRepresentation);
                    break;
                case MethodCallExpressionRepresentation methodCallExpressionRepresentation:
                    result.AddRange(methodCallExpressionRepresentation.Arguments);
                    result.Add(methodCallExpressionRepresentation.ParentObject);
                    break;
                case NewArrayExpressionRepresentation newArrayExpressionRepresentation:
                    result.AddRange(newArrayExpressionRepresentation.Expressions);
                    break;

                case NewExpressionRepresentation newExpressionRepresentation:
                {
                    result.AddRange(newExpressionRepresentation.Arguments);

                    break;
                }

                case TypeBinaryExpressionRepresentation typeBinaryExpressionRepresentation:
                    result.Add(typeBinaryExpressionRepresentation.Expression);
                    break;
                case UnaryExpressionRepresentation unaryExpressionRepresentation:
                    result.Add(unaryExpressionRepresentation.Operand);
                    break;
            }

            return result;
        }

        /// <summary>Visits all nodes in the tree.</summary>
        /// <param name="root">Node traverse from.</param>
        /// <returns>Collection of the <see cref="ExpressionRepresentationBase" /> from the tree.</returns>
        public static IReadOnlyCollection<ExpressionRepresentationBase> VisitAllNodes(this ExpressionRepresentationBase root)
        {
            var result = new List<ExpressionRepresentationBase>();
            foreach (var linkNode in root.VisitAllConnectedNodes())
            {
                result.AddRange(linkNode.VisitAllNodes());
            }

            result.Add(root);
            return result;
        }
    }
}
