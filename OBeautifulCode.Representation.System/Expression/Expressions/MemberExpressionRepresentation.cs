// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="MemberExpression" />.
    /// </summary>
    public class MemberExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="memberInfo">The member info description.</param>
        public MemberExpressionRepresentation(TypeRepresentation type, ExpressionRepresentationBase expression, MemberInfoRepresentation memberInfo)
            : base(type, ExpressionType.MemberAccess)
        {
            this.Expression = expression;
            this.MemberInfo = memberInfo;
        }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public ExpressionRepresentationBase Expression { get; private set; }

        /// <summary>Gets the member hash.</summary>
        /// <value>The member hash.</value>
        public MemberInfoRepresentation MemberInfo { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberExpressionRepresentation" />.
                              /// </summary>
    public static class MemberExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MemberExpressionRepresentation ToRepresentation(this MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                throw new ArgumentNullException(nameof(memberExpression));
            }

            var type = memberExpression.Type.ToRepresentation();
            var expression = memberExpression.Expression.ToRepresentation();
            var memberInfoRepresentation = memberExpression.Member.ToRepresentation();
            var result = new MemberExpressionRepresentation(type, expression, memberInfoRepresentation);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberExpressionRepresentation">The member expression.</param>
        /// <returns>Converted expression.</returns>
        public static MemberExpression FromRepresentation(this MemberExpressionRepresentation memberExpressionRepresentation)
        {
            if (memberExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberExpressionRepresentation));
            }

            var expression = memberExpressionRepresentation.Expression.FromRepresentation();
            var type = memberExpressionRepresentation.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToRepresentation().Equals(memberExpressionRepresentation.MemberInfo));
            var result = Expression.MakeMemberAccess(expression, member);

            return result;
        }
    }
}
