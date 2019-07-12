// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="MemberExpression" />.
    /// </summary>
    public class MemberExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="memberInfo">The member info description.</param>
        public MemberExpressionDescription(TypeDescription type, ExpressionDescriptionBase expression, MemberInfoDescription memberInfo)
            : base(type, ExpressionType.MemberAccess)
        {
            this.Expression = expression;
            this.MemberInfo = memberInfo;
        }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public ExpressionDescriptionBase Expression { get; private set; }

        /// <summary>Gets the member hash.</summary>
        /// <value>The member hash.</value>
        public MemberInfoDescription MemberInfo { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberExpressionDescription" />.
                              /// </summary>
    public static class MemberExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MemberExpressionDescription ToDescription(this MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                throw new ArgumentNullException(nameof(memberExpression));
            }

            var type = memberExpression.Type.ToDescription();
            var expression = memberExpression.Expression.ToDescription();
            var memberInfoDescription = memberExpression.Member.ToDescription();
            var result = new MemberExpressionDescription(type, expression, memberInfoDescription);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberExpressionDescription">The member expression.</param>
        /// <returns>Converted expression.</returns>
        public static MemberExpression FromDescription(this MemberExpressionDescription memberExpressionDescription)
        {
            if (memberExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(memberExpressionDescription));
            }

            var expression = memberExpressionDescription.Expression.FromDescription();
            var type = memberExpressionDescription.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToDescription().Equals(memberExpressionDescription.MemberInfo));
            var result = Expression.MakeMemberAccess(expression, member);

            return result;
        }
    }
}
