// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInfoDescription.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OBeautifulCode.Representation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using OBeautifulCode.Math.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Description of <see cref="MemberInfo" />.
    /// </summary>
    public class MemberInfoDescription : IEquatable<MemberInfoDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoDescription" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberHash">The member hash.</param>
        public MemberInfoDescription(TypeDescription type, string memberHash)
        {
            this.Type = type;
            this.MemberHash = memberHash;
        }

        /// <summary>
        /// Gets the member hash.
        /// </summary>
        /// <value>The member hash.</value>
        public string MemberHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Spelling/name is correct.")]
        public TypeDescription Type { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="MemberInfoDescription" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            MemberInfoDescription left,
            MemberInfoDescription right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = (left.Type == right.Type)
                      && (left.MemberHash == right.MemberHash);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="MemberInfoDescription" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            MemberInfoDescription left,
            MemberInfoDescription right)
            => !(left == right);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(MemberInfoDescription other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as MemberInfoDescription);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                          .Hash(this.Type)
                          .Hash(this.MemberHash)
                          .Value;
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="MemberInfoDescription" />.
    /// </summary>
    public static class MemberInfoDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Gets the member hash.</summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>Hash of the member.</returns>
        public static string GetSignatureHash(this MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var memberName = memberInfo.Name;
            var result = Invariant($"{memberName})");
            return result;
        }

        /// <summary>
        /// Converts to description.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>Converted <see cref="MemberInfoDescription" />.</returns>
        public static MemberInfoDescription ToDescription(this MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var type = memberInfo.DeclaringType.ToDescription();
            var memberHash = memberInfo.GetSignatureHash();
            var result = new MemberInfoDescription(type, memberHash);
            return result;
        }

        /// <summary>
        /// Converts from description.
        /// </summary>
        /// <param name="memberInfoDescription">The description.</param>
        /// <returns>Converted <see cref="MemberInfo" />.</returns>
        public static MemberInfo FromDescription(this MemberInfoDescription memberInfoDescription)
        {
            if (memberInfoDescription == null)
            {
                throw new ArgumentNullException(nameof(memberInfoDescription));
            }

            var type = memberInfoDescription.Type.ResolveFromLoadedTypes();
            var results = type.GetMembers()
                              .Where(_ => _.GetSignatureHash().Equals(memberInfoDescription.MemberHash, StringComparison.OrdinalIgnoreCase))
                              .ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find a member that matched hash '{memberInfoDescription.MemberHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.ToString()));
                throw new ArgumentException(Invariant($"Found too many members that matched hash '{memberInfoDescription.MemberHash}' on type '{type}'; {foundAddIn}."));
            }

            return results.Single();
        }
    }
}