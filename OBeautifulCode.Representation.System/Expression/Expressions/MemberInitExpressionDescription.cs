// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInitExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="MemberInitExpression" />.
    /// </summary>
    public class MemberInitExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberInitExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="newExpressionDescription">The new expression.</param>
        /// <param name="bindings">The bindings.</param>
        public MemberInitExpressionDescription(TypeDescription type, NewExpressionDescription newExpressionDescription, IReadOnlyCollection<MemberBindingDescriptionBase> bindings)
            : base(type, ExpressionType.MemberInit)
        {
            this.NewExpressionDescription = newExpressionDescription;
            this.Bindings = bindings;
        }

        /// <summary>Gets the new expression.</summary>
        /// <value>The new expression.</value>
        public NewExpressionDescription NewExpressionDescription { get; private set; }

        /// <summary>Gets the bindings.</summary>
        /// <value>The bindings.</value>
        public IReadOnlyCollection<MemberBindingDescriptionBase> Bindings { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberInitExpressionDescription" />.
                              /// </summary>
    public static class MemberInitExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberInitExpression">The memberInit expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MemberInitExpressionDescription ToDescription(this MemberInitExpression memberInitExpression)
        {
            if (memberInitExpression == null)
            {
                throw new ArgumentNullException(nameof(memberInitExpression));
            }

            var type = memberInitExpression.Type.ToDescription();
            var newExpression = memberInitExpression.NewExpression.ToDescription();
            var bindings = memberInitExpression.Bindings.ToDescription();
            var result = new MemberInitExpressionDescription(type, newExpression, bindings);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberInitExpressionDescription">The memberInit expression.</param>
        /// <returns>Converted expression.</returns>
        public static MemberInitExpression FromDescription(this MemberInitExpressionDescription memberInitExpressionDescription)
        {
            if (memberInitExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(memberInitExpressionDescription));
            }

            var newExpression = memberInitExpressionDescription.NewExpressionDescription.FromDescription();
            var bindings = memberInitExpressionDescription.Bindings.FromDescription();
            var result = Expression.MemberInit(newExpression, bindings);

            return result;
        }
    }
}
