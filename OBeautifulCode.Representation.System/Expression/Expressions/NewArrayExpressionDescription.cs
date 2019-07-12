// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewArrayExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using static System.FormattableString;

    /// <summary>
    /// Description of <see cref="NewArrayExpression" />.
    /// </summary>
    public class NewArrayExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="NewArrayExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="expressions">The expressions.</param>
        public NewArrayExpressionDescription(
            TypeDescription type,
            ExpressionType nodeType,
            IReadOnlyList<ExpressionDescriptionBase> expressions)
            : base(type, nodeType)
        {
            this.Expressions = expressions;
        }

        /// <summary>Gets the expressions.</summary>
        /// <value>The expressions.</value>
        public IReadOnlyList<ExpressionDescriptionBase> Expressions { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="NewArrayExpressionDescription" />.
    /// </summary>
    public static class NewArrayExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="newArrayExpression">The newArray expression.</param>
        /// <returns>Serializable expression.</returns>
        public static NewArrayExpressionDescription ToDescription(this NewArrayExpression newArrayExpression)
        {
            if (newArrayExpression == null)
            {
                throw new ArgumentNullException(nameof(newArrayExpression));
            }

            var type = newArrayExpression.Type.ToDescription();
            var nodeType = newArrayExpression.NodeType;
            var expressions = newArrayExpression.Expressions.ToDescription();
            var result = new NewArrayExpressionDescription(type, nodeType, expressions);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="newArrayExpressionDescription">The newArray expression.</param>
        /// <returns>Converted expression.</returns>
        public static NewArrayExpression FromDescription(this NewArrayExpressionDescription newArrayExpressionDescription)
        {
            if (newArrayExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(newArrayExpressionDescription));
            }

            NewArrayExpression result;
            var nodeType = newArrayExpressionDescription.NodeType;
            switch (nodeType)
            {
                case ExpressionType.NewArrayBounds:
                    result = Expression.NewArrayBounds(newArrayExpressionDescription.Type.ResolveFromLoadedTypes(), newArrayExpressionDescription.Expressions.FromDescription());
                    break;
                case ExpressionType.NewArrayInit:
                    result = Expression.NewArrayInit(newArrayExpressionDescription.Type.ResolveFromLoadedTypes(), newArrayExpressionDescription.Expressions.FromDescription());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(newArrayExpressionDescription.NodeType)} '{nodeType}' is not supported."));
            }

            return result;
        }
    }
}
