// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBindingDescriptionBase.cs" company="OBeautifulCode">
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
    /// Description of <see cref="MemberBinding" />.
    /// </summary>
    public abstract class MemberBindingDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberBindingDescriptionBase"/> class.</summary>
        /// <param name="type">The type with member.</param>
        /// <param name="memberInfo">The member info description.</param>
        /// <param name="bindingType">Type of the binding.</param>
        protected MemberBindingDescriptionBase(TypeDescription type, MemberInfoDescription memberInfo, MemberBindingType bindingType)
        {
            this.Type = type;
            this.MemberInfo = memberInfo;
            this.BindingType = bindingType;
        }

        /// <summary>Gets the type with member.</summary>
        /// <value>The type with member.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeDescription Type { get; private set; }

        /// <summary>Gets  the member info description.</summary>
        /// <value>The member hash.</value>
        public MemberInfoDescription MemberInfo { get; private set; }

        /// <summary>Gets the type of the binding.</summary>
        /// <value>The type of the binding.</value>
        public MemberBindingType BindingType { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberBindingDescriptionBase" />.
                              /// </summary>
    public static class MemberBindingDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberBinding">The memberBindings.</param>
        /// <returns>Serializable version.</returns>
        public static MemberBindingDescriptionBase ToDescription(this MemberBinding memberBinding)
        {
            if (memberBinding == null)
            {
                throw new ArgumentNullException(nameof(memberBinding));
            }

            if (memberBinding is MemberAssignment memberAssignment)
            {
                return memberAssignment.ToDescription();
            }
            else if (memberBinding is MemberListBinding memberListBinding)
            {
                return memberListBinding.ToDescription();
            }
            else if (memberBinding is MemberMemberBinding memberMemberBinding)
            {
                return memberMemberBinding.ToDescription();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Type of {nameof(MemberBinding)} '{memberBinding.GetType()}' is not supported."));
            }
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberBindingDescription">The memberBindings.</param>
        /// <returns>Converted version.</returns>
        public static MemberBinding FromDescription(this MemberBindingDescriptionBase memberBindingDescription)
        {
            if (memberBindingDescription == null)
            {
                throw new ArgumentNullException(nameof(memberBindingDescription));
            }

            if (memberBindingDescription is MemberAssignmentDescription memberAssignment)
            {
                return memberAssignment.FromDescription();
            }
            else if (memberBindingDescription is MemberListBindingDescription memberListBinding)
            {
                return memberListBinding.FromDescription();
            }
            else if (memberBindingDescription is MemberMemberBindingDescription memberMemberBinding)
            {
                return memberMemberBinding.FromDescription();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Type of {nameof(MemberBindingDescriptionBase)} '{memberBindingDescription.GetType()}' is not supported."));
            }
        }

        /// <summary>Converts to serializable.</summary>
        /// <param name="memberBindings">The memberBindings.</param>
        /// <returns>Serializable version.</returns>
        public static IReadOnlyCollection<MemberBindingDescriptionBase> ToDescription(this IReadOnlyCollection<MemberBinding> memberBindings)
        {
            var result = memberBindings.Select(_ => _.ToDescription()).ToList();
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberBindings">The memberBindings.</param>
        /// <returns>Converted version.</returns>
        public static IReadOnlyCollection<MemberBinding> FromDescription(this IReadOnlyCollection<MemberBindingDescriptionBase> memberBindings)
        {
            var result = memberBindings.Select(_ => _.FromDescription()).ToList();
            return result;
        }
    }
}
