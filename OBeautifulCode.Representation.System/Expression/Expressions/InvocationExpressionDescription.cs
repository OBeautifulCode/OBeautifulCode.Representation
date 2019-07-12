// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvocationExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="InvocationExpression" />.
    /// </summary>
    public class InvocationExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InvocationExpressionDescription"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="expressionDescription">The expression to invoke.</param>
        /// <param name="arguments">The arguments to invoke with.</param>
        public InvocationExpressionDescription(TypeDescription type, ExpressionDescriptionBase expressionDescription, IReadOnlyList<ExpressionDescriptionBase> arguments)
            : base(type, ExpressionType.Invoke)
        {
            this.ExpressionDescription = expressionDescription;
            this.Arguments = arguments;
        }

        /// <summary>Gets the expression to invoke.</summary>
        /// <value>The expression to invoke.</value>
        public ExpressionDescriptionBase ExpressionDescription { get; private set; }

        /// <summary>Gets the arguments for the expression.</summary>
        /// <value>The arguments for the expression.</value>
        public IReadOnlyList<ExpressionDescriptionBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="InvocationExpressionDescription" />.
                              /// </summary>
    public static class InvocationExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="invocationExpression">The invocation expression.</param>
        /// <returns>Serializable expression.</returns>
        public static InvocationExpressionDescription ToDescription(this InvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
            {
                throw new ArgumentNullException(nameof(invocationExpression));
            }

            var type = invocationExpression.Type.ToDescription();
            var expression = invocationExpression.Expression.ToDescription();
            var arguments = invocationExpression.Arguments.ToDescription();
            var result = new InvocationExpressionDescription(type, expression, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="invocationExpressionDescription">The invocation expression.</param>
        /// <returns>Converted expression.</returns>
        public static InvocationExpression FromDescription(this InvocationExpressionDescription invocationExpressionDescription)
        {
            if (invocationExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(invocationExpressionDescription));
            }

            var expression = invocationExpressionDescription.ExpressionDescription.FromDescription();
            var arguments = invocationExpressionDescription.Arguments.FromDescription();
            var result = Expression.Invoke(expression, arguments);

            return result;
        }
    }
}
