// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAssignmentRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Representation of <see cref="MemberAssignment" />.
    /// </summary>
    public partial class MemberAssignmentRepresentation : MemberBindingRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAssignmentRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="expressionRepresentation">The expression.</param>
        public MemberAssignmentRepresentation(
            TypeRepresentation type,
            MemberInfoRepresentation memberInfo,
            ExpressionRepresentationBase expressionRepresentation)
            : base(type, memberInfo, MemberBindingType.Assignment)
        {
            this.ExpressionRepresentation = expressionRepresentation;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionRepresentationBase ExpressionRepresentation { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberAssignmentRepresentation" />.
    /// </summary>
    public static class MemberAssignmentRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="memberAssignment">The memberAssignment.</param>
        /// <returns>
        /// Serializable version.
        /// </returns>
        public static MemberAssignmentRepresentation ToRepresentation(
            this MemberAssignment memberAssignment)
        {
            new { memberAssignment }.AsArg().Must().NotBeNull();

            var type = memberAssignment.Member.DeclaringType.ToRepresentation();

            var expression = memberAssignment.Expression.ToRepresentation();

            var memberInfoRepresentation = memberAssignment.Member.ToRepresentation();

            var result = new MemberAssignmentRepresentation(type, memberInfoRepresentation, expression);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="memberAssignmentRepresentation">The memberAssignment.</param>
        /// <returns>
        /// Converted version.
        /// </returns>
        public static MemberAssignment FromRepresentation(
            this MemberAssignmentRepresentation memberAssignmentRepresentation)
        {
            new { memberAssignmentRepresentation }.AsArg().Must().NotBeNull();

            var type = memberAssignmentRepresentation.Type.ResolveFromLoadedTypes();

            var member = type.GetMembers().Single(_ => _.ToRepresentation().Equals(memberAssignmentRepresentation.MemberInfo));

            var expression = memberAssignmentRepresentation.ExpressionRepresentation.FromRepresentation();

            var result = Expression.Bind(member, expression);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
