// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMemberBindingRepresentation.cs" company="OBeautifulCode">
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
    /// Representation of <see cref="MemberMemberBinding" />.
    /// </summary>
    public partial class MemberMemberBindingRepresentation : MemberBindingRepresentationBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMemberBindingRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberInfo">The member hash.</param>
        /// <param name="bindings">The bindings.</param>
        public MemberMemberBindingRepresentation(
            TypeRepresentation type,
            MemberInfoRepresentation memberInfo,
            IReadOnlyCollection<MemberBindingRepresentationBase> bindings)
        : base(type, memberInfo, MemberBindingType.MemberBinding)
        {
            if (bindings == null)
            {
                throw new ArgumentNullException(nameof(bindings));
            }

            if (!bindings.Any())
            {
                throw new ArgumentException(Invariant($"'{nameof(bindings)}' is an empty enumerable"));
            }

            if (bindings.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(bindings)}' contains an element that is null"));
            }

            this.Bindings = bindings;
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        public IReadOnlyCollection<MemberBindingRepresentationBase> Bindings { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberMemberBindingRepresentation" />.
    /// </summary>
    public static class MemberMemberBindingRepresentationExtensions
    {
        /// <summary>
        /// Converts to serializable.
        /// </summary>
        /// <param name="memberMemberBinding">The memberMemberBindingRepresentation.</param>
        /// <returns>
        /// Serializable version.
        /// </returns>
        public static MemberMemberBindingRepresentation ToRepresentation(
            this MemberMemberBinding memberMemberBinding)
        {
            if (memberMemberBinding == null)
            {
                throw new ArgumentNullException(nameof(memberMemberBinding));
            }

            var type = memberMemberBinding.Member.DeclaringType.ToRepresentation();

            var memberInfoRepresentation = memberMemberBinding.Member.ToRepresentation();

            var bindings = memberMemberBinding.Bindings.ToRepresentation();

            var result = new MemberMemberBindingRepresentation(type, memberInfoRepresentation, bindings);

            return result;
        }

        /// <summary>
        /// From the serializable.
        /// </summary>
        /// <param name="memberMemberBindingRepresentation">The memberMemberBindingRepresentation.</param>
        /// <returns>
        /// Converted version.
        /// </returns>
        public static MemberMemberBinding FromRepresentation(
            this MemberMemberBindingRepresentation memberMemberBindingRepresentation)
        {
            if (memberMemberBindingRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberMemberBindingRepresentation));
            }

            var type = memberMemberBindingRepresentation.Type.ResolveFromLoadedTypes();

            var member = type.GetMembers().Single(_ => _.ToRepresentation().Equals(memberMemberBindingRepresentation.MemberInfo));

            var bindings = memberMemberBindingRepresentation.Bindings.FromRepresentation();

            var result = Expression.MemberBind(member, bindings);

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
