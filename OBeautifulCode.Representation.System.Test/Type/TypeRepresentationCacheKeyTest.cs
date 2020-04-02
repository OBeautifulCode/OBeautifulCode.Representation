// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationCacheKeyTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;

    using Xunit;

    public static partial class TypeRepresentationCacheKeyTest
    {
        [Fact]
        public static void Functions_as_dictionary_key()
        {
            // Arrange
            var dictionary = new Dictionary<TypeRepresentationCacheKey, string>();
            var key1 = A.Dummy<TypeRepresentationCacheKey>();
            var key1a = new TypeRepresentationCacheKey(A.Dummy<TypeRepresentation>(), key1.TypeMatchStrategy, key1.MultipleMatchStrategy);
            var key1b = new TypeRepresentationCacheKey(key1.TypeRepresentation, A.Dummy<TypeMatchStrategy>().ThatIsNot(key1.TypeMatchStrategy), key1.MultipleMatchStrategy);
            var key1c = new TypeRepresentationCacheKey(key1.TypeRepresentation, key1.TypeMatchStrategy, A.Dummy<MultipleMatchStrategy>().ThatIsNot(key1.MultipleMatchStrategy));
            var key2 = A.Dummy<TypeRepresentationCacheKey>();

            // Act
            dictionary.Add(key1, nameof(key1));
            dictionary.Add(key1a, nameof(key1a));
            dictionary.Add(key1b, nameof(key1b));
            dictionary.Add(key1c, nameof(key1c));
            dictionary.Add(key2, nameof(key2));

            // Assert
            dictionary[key1].AsTest().Must().BeEqualTo(nameof(key1));
            dictionary[key1a].AsTest().Must().BeEqualTo(nameof(key1a));
            dictionary[key1b].AsTest().Must().BeEqualTo(nameof(key1b));
            dictionary[key1c].AsTest().Must().BeEqualTo(nameof(key1c));
            dictionary[key2].AsTest().Must().BeEqualTo(nameof(key2));
        }
    }
}