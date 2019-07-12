// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="ParameterExpression" />.
    /// </summary>
    public class ParameterExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="ParameterExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        public ParameterExpressionDescription(TypeDescription type, string name)
            : base(type, ExpressionType.Parameter)
        {
            this.Name = name;
        }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="ParameterExpressionDescription" />.
    /// </summary>
    public static class ParameterExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns>Serializable expression.</returns>
        public static ParameterExpressionDescription ToDescription(this ParameterExpression parameterExpression)
        {
            if (parameterExpression == null)
            {
                throw new ArgumentNullException(nameof(parameterExpression));
            }

            var type = parameterExpression.Type.ToDescription();
            var name = parameterExpression.Name;

            var result = new ParameterExpressionDescription(type, name);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="parameterExpressionDescription">The parameter expression.</param>
        /// <returns>Converted expression.</returns>
        public static ParameterExpression FromDescription(this ParameterExpressionDescription parameterExpressionDescription)
        {
            if (parameterExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(parameterExpressionDescription));
            }

            var type = parameterExpressionDescription.Type.ResolveFromLoadedTypes();
            var name = parameterExpressionDescription.Name;

            var result = Expression.Parameter(type, name);
            return result;
        }

        /// <summary>Converts to serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<ParameterExpressionDescription> ToDescription(
            this IReadOnlyList<ParameterExpression> expressions)
        {
            var result = expressions.Select(_ => _.ToDescription()).ToList();
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>Converted expressions.</returns>
        public static IReadOnlyList<ParameterExpression> FromDescription(
            this IReadOnlyList<ParameterExpressionDescription> expressions)
        {
            var result = expressions.Select(_ => _.FromDescription()).ToList();
            return result;
        }
    }
}
