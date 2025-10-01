// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInfoExtensions.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Reflection;
    using static global::System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="MemberInfo" />.
    /// </summary>
    public static class MemberInfoExtensions
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
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var memberName = memberInfo.Name;

            var result = Invariant($"{memberName})");

            return result;
        }
    }
}