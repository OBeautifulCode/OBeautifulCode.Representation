﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberListBindingRepresentation.cs" company="OBeautifulCode">
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
    /// Representation of <see cref="MemberListBinding" />.
    /// </summary>
    public class MemberListBindingRepresentation : MemberBindingRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberListBindingRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="initializers">The initializers.</param>
        public MemberListBindingRepresentation(TypeRepresentation type, MemberInfoRepresentation memberInfo, IReadOnlyList<ElementInitRepresentation> initializers)
            : base(type, memberInfo, MemberBindingType.ListBinding)
        {
            this.Initializers = initializers;
        }

        /// <summary>Gets the initializers.</summary>
        /// <value>The initializers.</value>
        public IReadOnlyList<ElementInitRepresentation> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberListBindingRepresentation" />.
                              /// </summary>
    public static class MemberListBindingRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberListBinding">The member list binding.</param>
        /// <returns>Serializable version.</returns>
        public static MemberListBindingRepresentation ToRepresentation(this MemberListBinding memberListBinding)
        {
            if (memberListBinding == null)
            {
                throw new ArgumentNullException(nameof(memberListBinding));
            }

            var type = memberListBinding.Member.DeclaringType.ToRepresentation();
            var memberInfoRepresentation = memberListBinding.Member.ToRepresentation();
            var initializers = memberListBinding.Initializers.ToRepresentation();
            var result = new MemberListBindingRepresentation(type, memberInfoRepresentation, initializers);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberListBindingRepresentation">The memberListBindingRepresentation.</param>
        /// <returns>Converted version.</returns>
        public static MemberListBinding FromRepresentation(this MemberListBindingRepresentation memberListBindingRepresentation)
        {
            if (memberListBindingRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberListBindingRepresentation));
            }

            var type = memberListBindingRepresentation.Type.ResolveFromLoadedTypes();
            var member = type.GetMembers().Single(_ => _.ToRepresentation().Equals(memberListBindingRepresentation.MemberInfo));
            var initializers = memberListBindingRepresentation.Initializers.FromRepresentation();

            var result = Expression.ListBind(member, initializers);
            return result;
        }
    }
}