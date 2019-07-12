// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="ConditionalExpression" />.
    /// </summary>
    public class ConditionalExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="ConditionalExpressionDescription"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="test">The test expression.</param>
        /// <param name="expressionIfTrue">If true expression.</param>
        /// <param name="expressionIfFalse">If false expression.</param>
        public ConditionalExpressionDescription(TypeDescription type, ExpressionType nodeType, ExpressionDescriptionBase test, ExpressionDescriptionBase expressionIfTrue, ExpressionDescriptionBase expressionIfFalse)
        : base(type, nodeType)
        {
            this.Test = test;
            this.IfTrue = expressionIfTrue;
            this.IfFalse = expressionIfFalse;
        }

        /// <summary>Gets the test expression.</summary>
        /// <value>The test expression.</value>
        public ExpressionDescriptionBase Test { get; private set; }

        /// <summary>Gets if true expression.</summary>
        /// <value>If true expression.</value>
        public ExpressionDescriptionBase IfTrue { get; private set; }

        /// <summary>Gets if false expression.</summary>
        /// <value>If false expression.</value>
        public ExpressionDescriptionBase IfFalse { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="ConditionalExpressionDescription" />.
    /// </summary>
    public static class ConditionalExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="conditionalExpression">The conditional expression.</param>
        /// <returns>Serializable expression.</returns>
        public static ConditionalExpressionDescription ToDescription(this ConditionalExpression conditionalExpression)
        {
            if (conditionalExpression == null)
            {
                throw new ArgumentNullException(nameof(conditionalExpression));
            }

            var type = conditionalExpression.Type.ToDescription();
            var nodeType = conditionalExpression.NodeType;
            var test = conditionalExpression.Test.ToDescription();
            var expressionIfTrue = conditionalExpression.IfTrue.ToDescription();
            var expressionIfFalse = conditionalExpression.IfFalse.ToDescription();
            var result = new ConditionalExpressionDescription(type, nodeType, test, expressionIfTrue, expressionIfFalse);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="conditionalExpressionDescription">The conditional expression.</param>
        /// <returns>Converted expression.</returns>
        public static ConditionalExpression FromDescription(this ConditionalExpressionDescription conditionalExpressionDescription)
        {
            if (conditionalExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(conditionalExpressionDescription));
            }

            var result = Expression.Condition(
                conditionalExpressionDescription.Test.FromDescription(),
                conditionalExpressionDescription.IfTrue.FromDescription(),
                conditionalExpressionDescription.IfFalse.FromDescription());

            return result;
        }
    }
}