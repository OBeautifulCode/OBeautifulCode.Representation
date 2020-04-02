// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInfoRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="MemberInfo" />.
    /// </summary>
    public partial class MemberInfoRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoRepresentation" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberHash">The member hash.</param>
        public MemberInfoRepresentation(
            TypeRepresentation type,
            string memberHash)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { memberHash }.AsArg().Must().NotBeNullNorWhiteSpace();

            this.Type = type;
            this.MemberHash = memberHash;
        }

        /// <summary>
        /// Gets the member hash.
        /// </summary>
        public string MemberHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Spelling/name is correct.")]
        public TypeRepresentation Type { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberInfoRepresentation" />.
    /// </summary>
    public static class MemberInfoRepresentationExtensions
    {
        /// <summary>
        /// Gets the member hash.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>
        /// Hash of the member.
        /// </returns>
        public static string GetSignatureHash(
            this MemberInfo memberInfo)
        {
            new { memberInfo }.AsArg().Must().NotBeNull();

            var memberName = memberInfo.Name;

            var result = Invariant($"{memberName})");

            return result;
        }

        /// <summary>
        /// Converts to representation.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>
        /// Converted <see cref="MemberInfoRepresentation" />.
        /// </returns>
        public static MemberInfoRepresentation ToRepresentation(
            this MemberInfo memberInfo)
        {
            new { memberInfo }.AsArg().Must().NotBeNull();

            var type = memberInfo.DeclaringType.ToRepresentation();

            var memberHash = memberInfo.GetSignatureHash();

            var result = new MemberInfoRepresentation(type, memberHash);

            return result;
        }

        /// <summary>
        /// Converts from representation.
        /// </summary>
        /// <param name="memberInfoRepresentation">The representation.</param>
        /// <returns>
        /// Converted <see cref="MemberInfo" />.
        /// </returns>
        public static MemberInfo FromRepresentation(
            this MemberInfoRepresentation memberInfoRepresentation)
        {
            new { memberInfoRepresentation }.AsArg().Must().NotBeNull();

            var type = memberInfoRepresentation.Type.ResolveFromLoadedTypes();
            var results = type.GetMembers()
                              .Where(_ => _.GetSignatureHash().Equals(memberInfoRepresentation.MemberHash, StringComparison.OrdinalIgnoreCase))
                              .ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find a member that matched hash '{memberInfoRepresentation.MemberHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.ToString()));
                throw new ArgumentException(Invariant($"Found too many members that matched hash '{memberInfoRepresentation.MemberHash}' on type '{type}'; {foundAddIn}."));
            }

            var result = results.Single();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}