// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionlessTypeEqualityComparerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.IO;
    using global::System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Reflection.Recipes;
    using Xunit;

    public static partial class VersionlessTypeEqualityComparerTest
    {
        [Fact]
        public static void Equals___Should_return_true___When_parameter_x_and_parameter_y_are_null()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_parameter_x_is_null_and_parameter_y_is_not_null_and_vice_versa()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var type = A.Dummy<Type>();

            // Act
            var actual1 = systemUnderTest.Equals(type, null);
            var actual2 = systemUnderTest.Equals(null, type);

            // Assert
            actual1.AsTest().Must().BeFalse();
            actual2.AsTest().Must().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_parameter_x_and_parameter_y_are_not_equal()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var type1 = A.Dummy<Type>();
            var type2 = A.Dummy<Type>().ThatIsNot(type1);

            // Act
            var actual = systemUnderTest.Equals(type1, type2);

            // Assert
            actual.AsTest().Must().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_parameter_x_and_parameter_y_are_equal_and_have_the_same_versions()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var type = A.Dummy<Type>();

            // Act
            var actual = systemUnderTest.Equals(type, type);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_parameter_x_and_parameter_y_are_equal_and_have_different_versions()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var oldConditionsAssembly = TypeGenerator.LoadOlderVersionOfConditions();

            var type1 = typeof(Conditions.Condition);
            var type2 = oldConditionsAssembly.GetTypes().Single(_ => _.Name == "Condition");

            // Act
            var actual = systemUnderTest.Equals(type1, type2);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void GetHashCode___Should_return_the_same_hash_code_for_null___When_called()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var expected = systemUnderTest.GetHashCode(null);

            // Act
            var actual = systemUnderTest.GetHashCode(null);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetHashCode___Should_return_the_same_hash_code_for_representations_that_are_equal___When_called()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeEqualityComparer();

            var oldConditionsAssembly = TypeGenerator.LoadOlderVersionOfConditions();

            var types = new[]
            {
                typeof(Conditions.Condition),
                oldConditionsAssembly.GetTypes().Single(_ => _.Name == "Condition"),
            };

            // Act
            var actual = types.Select(_ => systemUnderTest.GetHashCode(_)).ToList();

            // Assert
            actual.Distinct().AsTest().Must().HaveCount(1);
        }
    }
}