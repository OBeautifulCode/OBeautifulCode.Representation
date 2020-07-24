// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionlessTypeRepresentationEqualityComparer.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;

    using OBeautifulCode.Equality.Recipes;

    /// <summary>
    /// Compares two objects of type <see cref="TypeRepresentation"/> for equality, ignoring <see cref="TypeRepresentation.AssemblyVersion" />.
    /// </summary>
    public class VersionlessTypeRepresentationEqualityComparer : IEqualityComparer<TypeRepresentation>
    {
        /// <inheritdoc />
        public bool Equals(
            TypeRepresentation x,
            TypeRepresentation y)
        {
            x = x?.RemoveAssemblyVersions();
            y = y?.RemoveAssemblyVersions();

            var result = x == y;

            return result;
        }

        /// <inheritdoc />
        public int GetHashCode(
            TypeRepresentation obj)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var result = obj == null
                ? HashCodeHelper.Initialize().Hash<TypeRepresentation>(null).Value
                : obj.RemoveAssemblyVersions().GetHashCode();

            return result;
        }
    }
}
