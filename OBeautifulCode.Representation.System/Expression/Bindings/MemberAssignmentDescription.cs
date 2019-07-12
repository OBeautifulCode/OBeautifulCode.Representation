// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAssignmentDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="MemberAssignment" />.
    /// </summary>
    public class MemberAssignmentDescription : MemberBindingDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberAssignmentDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="expressionDescription">The expression.</param>
        public MemberAssignmentDescription(TypeDescription type, MemberInfoDescription memberInfo, ExpressionDescriptionBase expressionDescription)
            : base(type, memberInfo, MemberBindingType.Assignment)
        {
            this.ExpressionDescription = expressionDescription;
        }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public ExpressionDescriptionBase ExpressionDescription { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberAssignmentDescription" />.
                              /// </summary>
    public static class MemberAssignmentDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberAssignment">The memberAssignment.</param>
        /// <returns>Serializable version.</returns>
        public static MemberAssignmentDescription ToDescription(this System.Linq.Expressions.MemberAssignment memberAssignment)
        {
            if (memberAssignment == null)
            {
                throw new ArgumentNullException(nameof(memberAssignment));
            }

            var type = memberAssignment.Member.DeclaringType.ToDescription();
            var expression = memberAssignment.Expression.ToDescription();
            var memberInfoDescription = memberAssignment.Member.ToDescription();
            var result = new MemberAssignmentDescription(type, memberInfoDescription, expression);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberAssignmentDescription">The memberAssignment.</param>
        /// <returns>Converted version.</returns>
        public static MemberAssignment FromDescription(this MemberAssignmentDescription memberAssignmentDescription)
        {
            if (memberAssignmentDescription == null)
            {
                throw new ArgumentNullException(nameof(memberAssignmentDescription));
            }

            var type = memberAssignmentDescription.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToDescription().Equals(memberAssignmentDescription.MemberInfo));
            var expression = memberAssignmentDescription.ExpressionDescription.FromDescription();

            var result = Expression.Bind(member, expression);
            return result;
        }
    }
}
