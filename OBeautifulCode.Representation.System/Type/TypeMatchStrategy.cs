// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeMatchStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    /// <summary>
    /// Matching strategies on a type (allows for mismatch version to be compared or not).
    /// </summary>
    public enum TypeMatchStrategy
    {
        /// <summary>
        /// Match the name and namespace of the type.
        /// </summary>
        NamespaceAndName,

        /// <summary>
        /// Match the assembly qualified name of the type (this will include the version).
        /// </summary>
        AssemblyQualifiedName,
    }
}