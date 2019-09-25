// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleMatchStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    /// <summary>
    /// Instructions on how to handle multiple matches found during resolution.
    /// </summary>
    public enum MultipleMatchStrategy
    {
        /// <summary>
        /// Match the name and namespace of the type.
        /// </summary>
        ThrowOnMultiple,

        /// <summary>
        /// Return the newest version found.
        /// </summary>
        NewestVersion,

        /// <summary>
        /// Return the oldest version found.
        /// </summary>
        OldestVersion,
    }
}