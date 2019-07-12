// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBindingRepresentationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using static System.FormattableString;

    /// <summary>
    /// Representation of <see cref="MemberBinding" />.
    /// </summary>
    public abstract class MemberBindingRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberBindingRepresentationBase"/> class.</summary>
        /// <param name="type">The type with member.</param>
        /// <param name="memberInfo">The member info representation.</param>
        /// <param name="bindingType">Type of the binding.</param>
        protected MemberBindingRepresentationBase(TypeRepresentation type, MemberInfoRepresentation memberInfo, MemberBindingType bindingType)
        {
            this.Type = type;
            this.MemberInfo = memberInfo;
            this.BindingType = bindingType;
        }

        /// <summary>Gets the type with member.</summary>
        /// <value>The type with member.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }

        /// <summary>Gets  the member info representation.</summary>
        /// <value>The member hash.</value>
        public MemberInfoRepresentation MemberInfo { get; private set; }

        /// <summary>Gets the type of the binding.</summary>
        /// <value>The type of the binding.</value>
        public MemberBindingType BindingType { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberBindingRepresentationBase" />.
                              /// </summary>
    public static class MemberBindingRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberBinding">The memberBindings.</param>
        /// <returns>Serializable version.</returns>
        public static MemberBindingRepresentationBase ToRepresentation(this MemberBinding memberBinding)
        {
            if (memberBinding == null)
            {
                throw new ArgumentNullException(nameof(memberBinding));
            }

            if (memberBinding is MemberAssignment memberAssignment)
            {
                return memberAssignment.ToRepresentation();
            }
            else if (memberBinding is MemberListBinding memberListBinding)
            {
                return memberListBinding.ToRepresentation();
            }
            else if (memberBinding is MemberMemberBinding memberMemberBinding)
            {
                return memberMemberBinding.ToRepresentation();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Type of {nameof(MemberBinding)} '{memberBinding.GetType()}' is not supported."));
            }
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberBindingRepresentation">The memberBindings.</param>
        /// <returns>Converted version.</returns>
        public static MemberBinding FromRepresentation(this MemberBindingRepresentationBase memberBindingRepresentation)
        {
            if (memberBindingRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberBindingRepresentation));
            }

            if (memberBindingRepresentation is MemberAssignmentRepresentation memberAssignment)
            {
                return memberAssignment.FromRepresentation();
            }
            else if (memberBindingRepresentation is MemberListBindingRepresentation memberListBinding)
            {
                return memberListBinding.FromRepresentation();
            }
            else if (memberBindingRepresentation is MemberMemberBindingRepresentation memberMemberBinding)
            {
                return memberMemberBinding.FromRepresentation();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Type of {nameof(MemberBindingRepresentationBase)} '{memberBindingRepresentation.GetType()}' is not supported."));
            }
        }

        /// <summary>Converts to serializable.</summary>
        /// <param name="memberBindings">The memberBindings.</param>
        /// <returns>Serializable version.</returns>
        public static IReadOnlyCollection<MemberBindingRepresentationBase> ToRepresentation(this IReadOnlyCollection<MemberBinding> memberBindings)
        {
            var result = memberBindings.Select(_ => _.ToRepresentation()).ToList();
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberBindings">The memberBindings.</param>
        /// <returns>Converted version.</returns>
        public static IReadOnlyCollection<MemberBinding> FromRepresentation(this IReadOnlyCollection<MemberBindingRepresentationBase> memberBindings)
        {
            var result = memberBindings.Select(_ => _.FromRepresentation()).ToList();
            return result;
        }
    }
}
