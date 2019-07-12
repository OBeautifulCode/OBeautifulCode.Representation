// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberListBindingDescription.cs" company="OBeautifulCode">
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
    /// Description of <see cref="MemberListBinding" />.
    /// </summary>
    public class MemberListBindingDescription : MemberBindingDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberListBindingDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="initializers">The initializers.</param>
        public MemberListBindingDescription(TypeDescription type, MemberInfoDescription memberInfo, IReadOnlyList<ElementInitDescription> initializers)
            : base(type, memberInfo, MemberBindingType.ListBinding)
        {
            this.Initializers = initializers;
        }

        /// <summary>Gets the initializers.</summary>
        /// <value>The initializers.</value>
        public IReadOnlyList<ElementInitDescription> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberListBindingDescription" />.
                              /// </summary>
    public static class MemberListBindingDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberListBinding">The member list binding.</param>
        /// <returns>Serializable version.</returns>
        public static MemberListBindingDescription ToDescription(this MemberListBinding memberListBinding)
        {
            if (memberListBinding == null)
            {
                throw new ArgumentNullException(nameof(memberListBinding));
            }

            var type = memberListBinding.Member.DeclaringType.ToDescription();
            var memberInfoDescription = memberListBinding.Member.ToDescription();
            var initializers = memberListBinding.Initializers.ToDescription();
            var result = new MemberListBindingDescription(type, memberInfoDescription, initializers);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberListBindingDescription">The memberListBindingDescription.</param>
        /// <returns>Converted version.</returns>
        public static MemberListBinding FromDescription(this MemberListBindingDescription memberListBindingDescription)
        {
            if (memberListBindingDescription == null)
            {
                throw new ArgumentNullException(nameof(memberListBindingDescription));
            }

            var type = memberListBindingDescription.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToDescription().Equals(memberListBindingDescription.MemberInfo));
            var initializers = memberListBindingDescription.Initializers.FromDescription();

            var result = Expression.ListBind(member, initializers);
            return result;
        }
    }
}
