// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionlessTypeRepresentationEqualityComparerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;

    using Xunit;

    public static partial class VersionlessTypeRepresentationEqualityComparerTest
    {
        [Fact]
        public static void Equals___Should_return_true___When_parameters_are_null()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_parameter_x_is_null_and_parameter_y_is_not_null_and_vice_versa()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var representation = A.Dummy<TypeRepresentation>();

            // Act
            var actual1 = systemUnderTest.Equals(representation, null);
            var actual2 = systemUnderTest.Equals(null, representation);

            // Assert
            actual1.AsTest().Must().BeFalse();
            actual2.AsTest().Must().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_parameters_are_not_equal()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var representation1 = A.Dummy<TypeRepresentation>();
            var representation2 = A.Dummy<TypeRepresentation>().ThatIsNot(representation1);

            // Act
            var actual = systemUnderTest.Equals(representation1, representation2);

            // Assert
            actual.AsTest().Must().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_parameters_are_equal_and_have_the_same_versions()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var representation = A.Dummy<TypeRepresentation>();

            // Act
            var actual1 = systemUnderTest.Equals(representation, representation);
            var actual2 = systemUnderTest.Equals(representation, representation.DeepClone());

            // Assert
            actual1.AsTest().Must().BeTrue();
            actual2.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_parameters_are_equal_and_have_different_versions()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var representation1 = A.Dummy<TypeRepresentation>();
            var representation2 = ChangeVersion(representation1, "6.3.2.3");

            // Act
            var actual = systemUnderTest.Equals(representation1, representation2);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_parameters_are_equal_and_one_is_versioned_and_the_other_versionless()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var representation1 = A.Dummy<TypeRepresentation>();
            var representation2 = representation1.DeepClone().RemoveAssemblyVersions();

            // Act
            var actual = systemUnderTest.Equals(representation1, representation2);

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void GetHashCode___Should_return_the_same_hash_code_for_null___When_called()
        {
            // Arrange
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

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
            var systemUnderTest = new VersionlessTypeRepresentationEqualityComparer();

            var referenceObject = A.Dummy<TypeRepresentation>();

            var representations = new[]
            {
                referenceObject,
                referenceObject.DeepClone(),
                ChangeVersion(referenceObject, "6.3.2.3"),
                referenceObject.RemoveAssemblyVersions(),
            };

            // Act
            var actual = representations.Select(_ => systemUnderTest.GetHashCode(_)).ToList();

            // Assert
            actual.Distinct().AsTest().Must().HaveCount(1);
        }

        public static TypeRepresentation ChangeVersion(
            TypeRepresentation representation,
            string newVersion)
        {
            var result = representation.DeepCloneWithAssemblyVersion(newVersion);

            result = result.DeepCloneWithGenericArguments(result.GenericArguments.Select(_ => ChangeVersion(_, newVersion)).ToList());

            return result;
        }
    }
}