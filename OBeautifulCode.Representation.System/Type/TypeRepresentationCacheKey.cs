// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationCacheKey.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using OBeautifulCode.Math.Recipes;

    /// <summary>
    /// Cache key used to key an already de-referenced type along with its settings.
    /// </summary>
    public class TypeRepresentationCacheKey : IEquatable<TypeRepresentationCacheKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepresentationCacheKey"/> class.
        /// </summary>
        /// <param name="typeRepresentation"><see cref="TypeRepresentation"/> being referenced.</param>
        /// <param name="typeMatchStrategy"><see cref="TypeMatchStrategy"/> being referenced.</param>
        /// <param name="multipleMatchStrategy"><see cref="MultipleMatchStrategy"/> being referenced.</param>
        public TypeRepresentationCacheKey(
            TypeRepresentation typeRepresentation,
            TypeMatchStrategy typeMatchStrategy,
            MultipleMatchStrategy multipleMatchStrategy)
        {
            this.TypeRepresentation = typeRepresentation;
            this.TypeMatchStrategy = typeMatchStrategy;
            this.MultipleMatchStrategy = multipleMatchStrategy;
        }

        /// <summary>
        /// Gets the <see cref="TypeRepresentation"/> being referenced.
        /// </summary>
        public TypeRepresentation TypeRepresentation { get; private set; }

        /// <summary>
        /// Gets the <see cref="TypeMatchStrategy"/> being referenced.
        /// </summary>
        public TypeMatchStrategy TypeMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the <see cref="MultipleMatchStrategy"/> being referenced.
        /// </summary>
        public MultipleMatchStrategy MultipleMatchStrategy { get; private set; }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentationCacheKey"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; false otherwise.</returns>
        public static bool operator ==(
            TypeRepresentationCacheKey left,
            TypeRepresentationCacheKey right)
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
                (left.TypeRepresentation == right.TypeRepresentation) &&
                (left.TypeMatchStrategy == right.TypeMatchStrategy) &&
                (left.MultipleMatchStrategy == right.MultipleMatchStrategy);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentationCacheKey"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; false otherwise.</returns>
        public static bool operator !=(
            TypeRepresentationCacheKey left,
            TypeRepresentationCacheKey right)
            => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeRepresentationCacheKey other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeRepresentationCacheKey);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.TypeRepresentation)
                .Hash(this.TypeMatchStrategy)
                .Hash(this.MultipleMatchStrategy)
                .Value;
    }
}