// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionlessTypeEqualityComparer.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;

    /// <summary>
    /// Compares two objects of type <see cref="Type"/> for equality, ignoring assembly version.
    /// </summary>
    public class VersionlessTypeEqualityComparer : IEqualityComparer<Type>
    {
        private static readonly VersionlessTypeRepresentationEqualityComparer EqualityComparerToUse = new VersionlessTypeRepresentationEqualityComparer();

        /// <inheritdoc />
        public bool Equals(
            Type x,
            Type y)
        {
            var result = EqualityComparerToUse.Equals(x?.ToRepresentation(), y?.ToRepresentation());

            return result;
        }

        /// <inheritdoc />
        public int GetHashCode(
            Type obj)
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            var result = EqualityComparerToUse.GetHashCode(obj?.ToRepresentation());

            return result;
        }
    }
}
