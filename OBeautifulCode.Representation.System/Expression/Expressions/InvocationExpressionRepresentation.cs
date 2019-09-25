// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvocationExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="InvocationExpression" />.
    /// </summary>
    public class InvocationExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="InvocationExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="expressionRepresentation">The expression to invoke.</param>
        /// <param name="arguments">The arguments to invoke with.</param>
        public InvocationExpressionRepresentation(TypeRepresentation type, ExpressionRepresentationBase expressionRepresentation, IReadOnlyList<ExpressionRepresentationBase> arguments)
            : base(type, ExpressionType.Invoke)
        {
            this.ExpressionRepresentation = expressionRepresentation;
            this.Arguments = arguments;
        }

        /// <summary>Gets the expression to invoke.</summary>
        /// <value>The expression to invoke.</value>
        public ExpressionRepresentationBase ExpressionRepresentation { get; private set; }

        /// <summary>Gets the arguments for the expression.</summary>
        /// <value>The arguments for the expression.</value>
        public IReadOnlyList<ExpressionRepresentationBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="InvocationExpressionRepresentation" />.
                              /// </summary>
    public static class InvocationExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="invocationExpression">The invocation expression.</param>
        /// <returns>Serializable expression.</returns>
        public static InvocationExpressionRepresentation ToRepresentation(this InvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
            {
                throw new ArgumentNullException(nameof(invocationExpression));
            }

            var type = invocationExpression.Type.ToRepresentation();
            var expression = invocationExpression.Expression.ToRepresentation();
            var arguments = invocationExpression.Arguments.ToRepresentation();
            var result = new InvocationExpressionRepresentation(type, expression, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="invocationExpressionRepresentation">The invocation expression.</param>
        /// <returns>Converted expression.</returns>
        public static InvocationExpression FromRepresentation(this InvocationExpressionRepresentation invocationExpressionRepresentation)
        {
            if (invocationExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(invocationExpressionRepresentation));
            }

            var expression = invocationExpressionRepresentation.ExpressionRepresentation.FromRepresentation();
            var arguments = invocationExpressionRepresentation.Arguments.FromRepresentation();
            var result = Expression.Invoke(expression, arguments);

            return result;
        }
    }
}
