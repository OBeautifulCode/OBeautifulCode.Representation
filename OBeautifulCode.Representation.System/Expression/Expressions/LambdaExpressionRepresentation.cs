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

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="LambdaExpression" />.
    /// </summary>
    public partial class LambdaExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="body">The body.</param>
        /// <param name="parameters">The parameters.</param>
        public LambdaExpressionRepresentation(
            TypeRepresentation type,
            ExpressionRepresentationBase body,
            IReadOnlyList<ParameterExpressionRepresentation> parameters)
        : base(type, ExpressionType.Lambda)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (!parameters.Any())
            {
                throw new ArgumentException(Invariant($"'{nameof(parameters)}' is an empty enumerable"));
            }

            if (parameters.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(parameters)}' contains at least one null element"));
            }

            this.Body = body;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public ExpressionRepresentationBase Body { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IReadOnlyList<ParameterExpressionRepresentation> Parameters { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="LambdaExpressionRepresentation" />.
    /// </summary>
    public static class LambdaExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static LambdaExpressionRepresentation ToRepresentation(
            this LambdaExpression lambdaExpression)
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

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="lambdaExpressionRepresentation">The lambda expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static LambdaExpression FromRepresentation(
            this LambdaExpressionRepresentation lambdaExpressionRepresentation)
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

            var result = matchingParametersFromBody.Any()
                ? Expression.Lambda(type, body, matchingParametersFromBody)
                : Expression.Lambda(type, body);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
