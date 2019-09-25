// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LambdaExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="LambdaExpression" />.
    /// </summary>
    public class LambdaExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="LambdaExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="body">The body.</param>
        /// <param name="parameters">The parameters.</param>
        public LambdaExpressionRepresentation(TypeRepresentation type, ExpressionRepresentationBase body, IReadOnlyList<ParameterExpressionRepresentation> parameters)
        : base(type, ExpressionType.Lambda)
        {
            this.Body = body;
            this.Parameters = parameters;
        }

        /// <summary>Gets the body.</summary>
        /// <value>The body.</value>
        public ExpressionRepresentationBase Body { get; private set; }

        /// <summary>Gets the parameters.</summary>
        /// <value>The parameters.</value>
        public IReadOnlyList<ParameterExpressionRepresentation> Parameters { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="LambdaExpressionRepresentation" />.
                              /// </summary>
    public static class LambdaExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="lambdaExpression">The lambda expression.</param>
        /// <returns>Serializable expression.</returns>
        public static LambdaExpressionRepresentation ToRepresentation(this LambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpression));
            }

            var type = lambdaExpression.Type.ToRepresentation();
            var body = lambdaExpression.Body.ToRepresentation();
            var parameters = lambdaExpression.Parameters.ToRepresentation();
            var result = new LambdaExpressionRepresentation(type, body, parameters);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="lambdaExpressionRepresentation">The lambda expression.</param>
        /// <returns>Converted expression.</returns>
        public static LambdaExpression FromRepresentation(this LambdaExpressionRepresentation lambdaExpressionRepresentation)
        {
            if (lambdaExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpressionRepresentation));
            }

            var type = lambdaExpressionRepresentation.Type.ResolveFromLoadedTypes();
            var body = lambdaExpressionRepresentation.Body.FromRepresentation();
            var parameters = lambdaExpressionRepresentation.Parameters.FromRepresentation().ToList();

            var allParametersFromBody = body.VisitAllNodes().Where(_ => _ is ParameterExpression).Cast<ParameterExpression>().ToList();

            var matchingParametersFromBody = new List<ParameterExpression>();
            foreach (var parameter in parameters)
            {
                var parameterFromBody = allParametersFromBody.SingleOrDefault(allParameter =>
                    parameter.Name == allParameter.Name && parameter.Type == allParameter.Type);

                if (parameterFromBody != null)
                {
                    matchingParametersFromBody.Add(parameterFromBody);
                }
            }

            LambdaExpression result;
            if (matchingParametersFromBody.Any())
            {
                result = Expression.Lambda(type, body, matchingParametersFromBody);
            }
            else
            {
                result = Expression.Lambda(type, body);
            }

            return result;
        }
    }
}
