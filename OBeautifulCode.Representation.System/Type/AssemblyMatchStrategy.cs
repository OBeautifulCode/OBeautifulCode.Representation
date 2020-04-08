// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyMatchStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    /// <summary>
    /// Matching strategies on assemblies (allows for mismatch version to be compared or not).
    /// </summary>
    public enum AssemblyMatchStrategy
    {
        /// <summary>
        /// Match the type with the same assembly qualified name, ignoring version.
        /// If multiple versions found then there's no match.
        /// </summary>
        AnySingleVersion,

        /// <summary>
        /// Match the type with the same assembly qualified name, ignoring version.
        /// If multiple versions found then return the maximum version.
        /// </summary>
        MaxVersion,

        /// <summary>
        /// Match the type with the same assembly qualified name, ignoring version.
        /// If multiple versions found then return the minimum version.
        /// </summary>
        MinVersion,

        /// <summary>
        /// Match the type with the same assembly qualified name, including version.
        /// </summary>
        SpecifiedVersion,
    }
}