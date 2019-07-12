// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMemberBindingDescription.cs" company="OBeautifulCode">
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
    /// Description of <see cref="MemberMemberBinding" />.
    /// </summary>
    public class MemberMemberBindingDescription : MemberBindingDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberMemberBindingDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="bindings">The bindings.</param>
        public MemberMemberBindingDescription(TypeDescription type, MemberInfoDescription memberInfo, IReadOnlyCollection<MemberBindingDescriptionBase> bindings)
        : base(type, memberInfo, MemberBindingType.MemberBinding)
        {
            this.Bindings = bindings;
        }

        /// <summary>Gets the bindings.</summary>
        /// <value>The bindings.</value>
        public IReadOnlyCollection<MemberBindingDescriptionBase> Bindings { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberMemberBindingDescription" />.
                              /// </summary>
    public static class MemberMemberBindingDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberMemberBinding">The memberMemberBindingDescription.</param>
        /// <returns>Serializable version.</returns>
        public static MemberMemberBindingDescription ToDescription(this MemberMemberBinding memberMemberBinding)
        {
            if (memberMemberBinding == null)
            {
                throw new ArgumentNullException(nameof(memberMemberBinding));
            }

            var type = memberMemberBinding.Member.DeclaringType.ToDescription();
            var memberInfoDescription = memberMemberBinding.Member.ToDescription();
            var bindings = memberMemberBinding.Bindings.ToDescription();
            var result = new MemberMemberBindingDescription(type, memberInfoDescription, bindings);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberMemberBindingDescription">The memberMemberBindingDescription.</param>
        /// <returns>Converted version.</returns>
        public static MemberMemberBinding FromDescription(this MemberMemberBindingDescription memberMemberBindingDescription)
        {
            if (memberMemberBindingDescription == null)
            {
                throw new ArgumentNullException(nameof(memberMemberBindingDescription));
            }

            var type = memberMemberBindingDescription.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToDescription().Equals(memberMemberBindingDescription.MemberInfo));
            var bindings = memberMemberBindingDescription.Bindings.FromDescription();

            var result = Expression.MemberBind(member, bindings);
            return result;
        }
    }
}
