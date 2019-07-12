// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="ConditionalExpression" />.
    /// </summary>
    public class ConditionalExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="ConditionalExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="test">The test expression.</param>
        /// <param name="expressionIfTrue">If true expression.</param>
        /// <param name="expressionIfFalse">If false expression.</param>
        public ConditionalExpressionRepresentation(TypeRepresentation type, ExpressionType nodeType, ExpressionRepresentationBase test, ExpressionRepresentationBase expressionIfTrue, ExpressionRepresentationBase expressionIfFalse)
        : base(type, nodeType)
        {
            this.Test = test;
            this.IfTrue = expressionIfTrue;
            this.IfFalse = expressionIfFalse;
        }

        /// <summary>Gets the test expression.</summary>
        /// <value>The test expression.</value>
        public ExpressionRepresentationBase Test { get; private set; }

        /// <summary>Gets if true expression.</summary>
        /// <value>If true expression.</value>
        public ExpressionRepresentationBase IfTrue { get; private set; }

        /// <summary>Gets if false expression.</summary>
        /// <value>If false expression.</value>
        public ExpressionRepresentationBase IfFalse { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="ConditionalExpressionRepresentation" />.
    /// </summary>
    public static class ConditionalExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="conditionalExpression">The conditional expression.</param>
        /// <returns>Serializable expression.</returns>
        public static ConditionalExpressionRepresentation ToRepresentation(this ConditionalExpression conditionalExpression)
        {
            if (conditionalExpression == null)
            {
                throw new ArgumentNullException(nameof(conditionalExpression));
            }

            var type = conditionalExpression.Type.ToRepresentation();
            var nodeType = conditionalExpression.NodeType;
            var test = conditionalExpression.Test.ToRepresentation();
            var expressionIfTrue = conditionalExpression.IfTrue.ToRepresentation();
            var expressionIfFalse = conditionalExpression.IfFalse.ToRepresentation();
            var result = new ConditionalExpressionRepresentation(type, nodeType, test, expressionIfTrue, expressionIfFalse);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="conditionalExpressionRepresentation">The conditional expression.</param>
        /// <returns>Converted expression.</returns>
        public static ConditionalExpression FromRepresentation(this ConditionalExpressionRepresentation conditionalExpressionRepresentation)
        {
            if (conditionalExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(conditionalExpressionRepresentation));
            }

            var result = Expression.Condition(
                conditionalExpressionRepresentation.Test.FromRepresentation(),
                conditionalExpressionRepresentation.IfTrue.FromRepresentation(),
                conditionalExpressionRepresentation.IfFalse.FromRepresentation());

            return result;
        }
    }
}