// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="MemberExpression" />.
    /// </summary>
    public partial class MemberExpressionRepresentation : ExpressionRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberExpressionRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="memberInfo">The member info representation.</param>
        public MemberExpressionRepresentation(
            TypeRepresentation type,
            ExpressionRepresentationBase expression,
            MemberInfoRepresentation memberInfo)
            : base(type, ExpressionType.MemberAccess)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            this.Expression = expression;
            this.MemberInfo = memberInfo;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionRepresentationBase Expression { get; private set; }

        /// <summary>
        /// Gets the member hash.
        /// </summary>
        public MemberInfoRepresentation MemberInfo { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberExpressionRepresentation" />.
    /// </summary>
    public static class MemberExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>
        /// Serializable expression.
        /// </returns>
        public static MemberExpressionRepresentation ToRepresentation(
            this MemberExpression memberExpression)
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

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="memberExpressionRepresentation">The member expression.</param>
        /// <returns>
        /// Converted expression.
        /// </returns>
        public static MemberExpression FromRepresentation(
            this MemberExpressionRepresentation memberExpressionRepresentation)
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
