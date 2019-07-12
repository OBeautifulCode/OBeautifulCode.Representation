// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDescriptionCacheKey.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using OBeautifulCode.Math.Recipes;

    /// <summary>
    /// Cache key used to key an already de-referenced type along with its settings.
    /// </summary>
    public class TypeDescriptionCacheKey : IEquatable<TypeDescriptionCacheKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDescriptionCacheKey"/> class.
        /// </summary>
        /// <param name="typeDescription"><see cref="TypeDescription"/> being referenced.</param>
        /// <param name="typeMatchStrategy"><see cref="TypeMatchStrategy"/> being referenced.</param>
        /// <param name="multipleMatchStrategy"><see cref="MultipleMatchStrategy"/> being referenced.</param>
        public TypeDescriptionCacheKey(
            TypeDescription typeDescription,
            TypeMatchStrategy typeMatchStrategy,
            MultipleMatchStrategy multipleMatchStrategy)
        {
            this.TypeDescription = typeDescription;
            this.TypeMatchStrategy = typeMatchStrategy;
            this.MultipleMatchStrategy = multipleMatchStrategy;
        }

        /// <summary>
        /// Gets the <see cref="TypeDescription"/> being referenced.
        /// </summary>
        public TypeDescription TypeDescription { get; private set; }

        /// <summary>
        /// Gets the <see cref="TypeMatchStrategy"/> being referenced.
        /// </summary>
        public TypeMatchStrategy TypeMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the <see cref="MultipleMatchStrategy"/> being referenced.
        /// </summary>
        public MultipleMatchStrategy MultipleMatchStrategy { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeDescriptionCacheKey"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; false otherwise.</returns>
        public static bool operator ==(
            TypeDescriptionCacheKey left,
            TypeDescriptionCacheKey right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result =
                (left.TypeDescription == right.TypeDescription) &&
                (left.TypeMatchStrategy == right.TypeMatchStrategy) &&
                (left.MultipleMatchStrategy == right.MultipleMatchStrategy);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeDescriptionCacheKey"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; false otherwise.</returns>
        public static bool operator !=(
            TypeDescriptionCacheKey left,
            TypeDescriptionCacheKey right)
            => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeDescriptionCacheKey other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeDescriptionCacheKey);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.TypeDescription)
                .Hash(this.TypeMatchStrategy)
                .Hash(this.MultipleMatchStrategy)
                .Value;
    }
}