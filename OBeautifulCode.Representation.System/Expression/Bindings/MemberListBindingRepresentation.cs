﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberListBindingRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="MemberListBinding" />.
    /// </summary>
    public partial class MemberListBindingRepresentation : MemberBindingRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberListBindingRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="initializers">The initializers.</param>
        public MemberListBindingRepresentation(
            TypeRepresentation type,
            MemberInfoRepresentation memberInfo,
            IReadOnlyList<ElementInitRepresentation> initializers)
            : base(type, memberInfo, MemberBindingType.ListBinding)
        {
            if (initializers == null)
            {
                throw new ArgumentNullException(nameof(initializers));
            }

            if (!initializers.Any())
            {
                throw new ArgumentException(Invariant($"'{nameof(initializers)}' is an empty enumerable"));
            }

            if (initializers.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(initializers)}' contains at least one null element"));
            }

            this.Initializers = initializers;
        }

        /// <summary>
        /// Gets the initializers.
        /// </summary>
        public IReadOnlyList<ElementInitRepresentation> Initializers { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberListBindingRepresentation" />.
    /// </summary>
    public static class MemberListBindingRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="memberListBinding">The member list binding.</param>
        /// <returns>
        /// Serializable version.
        /// </returns>
        public static MemberListBindingRepresentation ToRepresentation(
            this MemberListBinding memberListBinding)
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

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="memberListBindingRepresentation">The memberListBindingRepresentation.</param>
        /// <returns>
        /// Converted version.
        /// </returns>
        public static MemberListBinding FromRepresentation(
            this MemberListBindingRepresentation memberListBindingRepresentation)
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
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
