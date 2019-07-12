// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInitExpressionDescription.cs" company="OBeautifulCode">
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
    /// Description of <see cref="ListInitExpression" />.
    /// </summary>
    /// <seealso cref="ExpressionDescriptionBase" />
    public class ListInitExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="ListInitExpressionDescription"/> class.</summary>
        /// <param name="type">The type of expression.</param>
        /// <param name="newExpressionDescription">The new expression.</param>
        /// <param name="initializers">The initializers.</param>
        public ListInitExpressionDescription(
            TypeDescription type,
            NewExpressionDescription newExpressionDescription,
            IReadOnlyList<ElementInitDescription> initializers)
            : base(type, ExpressionType.ListInit)
        {
            this.NewExpressionDescription = newExpressionDescription;
            this.Initializers = initializers;
        }

        /// <summary>Gets the new expression description.</summary>
        /// <value>The new expression description.</value>
        public NewExpressionDescription NewExpressionDescription { get; private set; }

        /// <summary>Gets the initializers.</summary>
        /// <value>The initializers.</value>
        public IReadOnlyList<ElementInitDescription> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="ListInitExpressionDescription" />.
                              /// </summary>
    public static class ListInitExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="listInitExpression">The listInit expression.</param>
        /// <returns>Serializable expression.</returns>
        public static ListInitExpressionDescription ToDescription(this ListInitExpression listInitExpression)
        {
            if (listInitExpression == null)
            {
                throw new ArgumentNullException(nameof(listInitExpression));
            }

            var type = listInitExpression.Type.ToDescription();
            var newExpression = listInitExpression.NewExpression.ToDescription();
            var initializers = listInitExpression.Initializers.ToDescription();
            var result = new ListInitExpressionDescription(type, newExpression, initializers);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="listInitExpressionDescription">The listInit expression.</param>
        /// <returns>Converted expression.</returns>
        public static ListInitExpression FromDescription(this ListInitExpressionDescription listInitExpressionDescription)
        {
            if (listInitExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(listInitExpressionDescription));
            }

            var result = Expression.ListInit(
                listInitExpressionDescription.NewExpressionDescription.FromDescription(),
                listInitExpressionDescription.Initializers.FromDescription().ToArray());

            return result;
        }
    }
}
