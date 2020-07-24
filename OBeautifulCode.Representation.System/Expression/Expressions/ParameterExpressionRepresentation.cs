// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterExpressionRepresentation.cs" company="OBeautifulCode">
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
    /// Representation of <see cref="ParameterExpression" />.
    /// </summary>
    public partial class ParameterExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public ParameterExpressionRepresentation(
            TypeRepresentation type,
            string name)
            : base(type, ExpressionType.Parameter)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Invariant($"'{nameof(name)}' is white space"));
            }

            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ParameterExpressionRepresentation" />.
    /// </summary>
    public static class ParameterExpressionRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static ParameterExpressionRepresentation ToRepresentation(this ParameterExpression parameterExpression)
        {
            if (parameterExpression == null)
            {
                throw new ArgumentNullException(nameof(parameterExpression));
            }

            var type = parameterExpression.Type.ToRepresentation();

            var name = parameterExpression.Name;

            var result = new ParameterExpressionRepresentation(type, name);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="parameterExpressionRepresentation">The parameter expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static ParameterExpression FromRepresentation(
            this ParameterExpressionRepresentation parameterExpressionRepresentation)
        {
            if (parameterExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(parameterExpressionRepresentation));
            }

            var type = parameterExpressionRepresentation.Type.ResolveFromLoadedTypes();

            var name = parameterExpressionRepresentation.Name;

            var result = Expression.Parameter(type, name);
            return result;
        }

        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>
        /// Converted expressions.
        /// </returns>
        public static IReadOnlyList<ParameterExpressionRepresentation> ToRepresentation(
            this IReadOnlyList<ParameterExpression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            if (expressions.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(expressions)}' contains an element that is null"));
            }

            var result = expressions.Select(_ => _.ToRepresentation()).ToList();

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>
        /// Converted expressions.
        /// </returns>
        public static IReadOnlyList<ParameterExpression> FromRepresentation(
            this IReadOnlyList<ParameterExpressionRepresentation> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            if (expressions.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(expressions)}' contains an element that is null"));
            }

            var result = expressions.Select(_ => _.FromRepresentation()).ToList();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
