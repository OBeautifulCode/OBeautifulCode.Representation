// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAssignmentRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="MemberAssignment" />.
    /// </summary>
    public class MemberAssignmentRepresentation : MemberBindingRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberAssignmentRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="expressionRepresentation">The expression.</param>
        public MemberAssignmentRepresentation(TypeRepresentation type, MemberInfoRepresentation memberInfo, ExpressionRepresentationBase expressionRepresentation)
            : base(type, memberInfo, MemberBindingType.Assignment)
        {
            this.ExpressionRepresentation = expressionRepresentation;
        }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public ExpressionRepresentationBase ExpressionRepresentation { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberAssignmentRepresentation" />.
                              /// </summary>
    public static class MemberAssignmentRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberAssignment">The memberAssignment.</param>
        /// <returns>Serializable version.</returns>
        public static MemberAssignmentRepresentation ToRepresentation(this System.Linq.Expressions.MemberAssignment memberAssignment)
        {
            if (memberAssignment == null)
            {
                throw new ArgumentNullException(nameof(memberAssignment));
            }

            var type = memberAssignment.Member.DeclaringType.ToRepresentation();
            var expression = memberAssignment.Expression.ToRepresentation();
            var memberInfoRepresentation = memberAssignment.Member.ToRepresentation();
            var result = new MemberAssignmentRepresentation(type, memberInfoRepresentation, expression);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberAssignmentRepresentation">The memberAssignment.</param>
        /// <returns>Converted version.</returns>
        public static MemberAssignment FromRepresentation(this MemberAssignmentRepresentation memberAssignmentRepresentation)
        {
            if (memberAssignmentRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberAssignmentRepresentation));
            }

            var type = memberAssignmentRepresentation.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToRepresentation().Equals(memberAssignmentRepresentation.MemberInfo));
            var expression = memberAssignmentRepresentation.ExpressionRepresentation.FromRepresentation();

            var result = Expression.Bind(member, expression);
            return result;
        }
    }
}
