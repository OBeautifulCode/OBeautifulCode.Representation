// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationCacheKey.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Cache key used to key an already de-referenced type along with its settings.
    /// </summary>
    public partial class TypeRepresentationCacheKey : IModelViaCodeGen
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
            new { typeRepresentation }.AsArg().Must().NotBeNull();

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
    }
}