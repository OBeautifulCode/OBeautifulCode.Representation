// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test.AssemblyRepresentationTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy.Internal;
    using FakeItEasy;
    using FluentAssertions;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Bootstrapper.Test.CodeGeneration;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    public partial class AssemblyRepresentationTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public AssemblyRepresentationTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GenerateModel()
        {
            var results = CodeGenerator.GenerateForModel<AssemblyRepresentation>(CodeGenerator.GenerateFor.ModelImplementationPartialClass);
            this.testOutputHelper.WriteLine(results);
        }

        [Fact]
        public void GenerateTests()
        {
            var results = CodeGenerator.GenerateForModel<AssemblyRepresentation>(
                CodeGenerator.GenerateFor.ModelImplementationTestsPartialClassWithoutSerialization);
            this.testOutputHelper.WriteLine(results);
        }
    }

    public partial class AssemblyRepresentationTest
    {
        private static readonly AssemblyRepresentation ObjectForEquatableTests = A.Dummy<AssemblyRepresentation>();

        private static readonly AssemblyRepresentation ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests =
            new AssemblyRepresentation(
                                 ObjectForEquatableTests.Name,
                                 ObjectForEquatableTests.Version,
                                 ObjectForEquatableTests.FilePath,
                                 ObjectForEquatableTests.FrameworkVersion);

        private static readonly AssemblyRepresentation[] ObjectsThatAreNotEqualToObjectForEquatableTests =
        {
            new AssemblyRepresentation(
                                 ObjectForEquatableTests.Name,
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Version),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FilePath),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FrameworkVersion)),
            new AssemblyRepresentation(
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                                 ObjectForEquatableTests.Version,
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FilePath),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FrameworkVersion)),
            new AssemblyRepresentation(
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Version),
                                 ObjectForEquatableTests.FilePath,
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FrameworkVersion)),
            new AssemblyRepresentation(
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Version),
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.FilePath),
                                 ObjectForEquatableTests.FrameworkVersion),
        };

        private static readonly string ObjectThatIsNotTheSameTypeAsObjectForEquatableTests = A.Dummy<string>();

        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<AssemblyRepresentation>();

            var expected = Invariant($"Representation.AssemblyRepresentation: Name = {systemUnderTest.Name}, Version = {systemUnderTest.Version}, FilePath = {systemUnderTest.FilePath}, FrameworkVersion = {systemUnderTest.FrameworkVersion}.");

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Constructing
        {
            [Fact]
            public static void AssemblyRepresentation___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(AssemblyRepresentation);
                var expectedModelMethods = typeof(IModel<AssemblyRepresentation>)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<AssemblyRepresentation>));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }

            [Fact]
            public static void Constructor___Should_throw_ArgumentNullException___When_parameter_name_is_null()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();

                // Act
                var actual = Record.Exception(() => new AssemblyRepresentation(
                                 null,
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion));

                // Assert
                actual.Should().BeOfType<ArgumentNullException>();
                actual.Message.Should().Contain("name");
            }

            [Fact]
            public static void Constructor___Should_throw_ArgumentException___When_parameter_name_is_white_space()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();

                // Act
                var actual = Record.Exception(() => new AssemblyRepresentation(
                                 Invariant($"  {Environment.NewLine}  "),
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion));

                // Assert
                actual.Should().BeOfType<ArgumentException>();
                actual.Message.Should().Contain("name");
                actual.Message.Should().Contain("white space");
            }

            [Fact]
            public static void Name___Should_return_same_name_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();
                var systemUnderTest = new AssemblyRepresentation(
                                 referenceObject.Name,
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion);
                var expected = referenceObject.Name;

                // Act
                var actual = systemUnderTest.Name;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void Version___Should_return_same_version_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();
                var systemUnderTest = new AssemblyRepresentation(
                                 referenceObject.Name,
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion);
                var expected = referenceObject.Version;

                // Act
                var actual = systemUnderTest.Version;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void FilePath___Should_return_same_filePath_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();
                var systemUnderTest = new AssemblyRepresentation(
                                 referenceObject.Name,
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion);
                var expected = referenceObject.FilePath;

                // Act
                var actual = systemUnderTest.FilePath;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void FrameworkVersion___Should_return_same_frameworkVersion_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<AssemblyRepresentation>();
                var systemUnderTest = new AssemblyRepresentation(
                                 referenceObject.Name,
                                 referenceObject.Version,
                                 referenceObject.FilePath,
                                 referenceObject.FrameworkVersion);
                var expected = referenceObject.FrameworkVersion;

                // Act
                var actual = systemUnderTest.FrameworkVersion;

                // Assert
                actual.Should().Be(expected);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Cloning
        {
            [Fact]
            public static void DeepClone___Should_deep_clone_object___When_called()
            {
                // Arrange
                var systemUnderTest = A.Dummy<AssemblyRepresentation>();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
                actual.Should().Be(systemUnderTest);
                actual.Should().NotBeSameAs(systemUnderTest);
            }

            [Fact]
            public static void DeepCloneWithName___Should_deep_clone_object_and_replace_Name_with_the_provided_name___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<AssemblyRepresentation>();
                var referenceObject = A.Dummy<AssemblyRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithName(referenceObject.Name);

                // Assert
                actual.Name.Should().Be(referenceObject.Name);
                actual.Version.Should().Be(systemUnderTest.Version);
                actual.FilePath.Should().Be(systemUnderTest.FilePath);
                actual.FrameworkVersion.Should().Be(systemUnderTest.FrameworkVersion);
            }

            [Fact]
            public static void DeepCloneWithVersion___Should_deep_clone_object_and_replace_Version_with_the_provided_version___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<AssemblyRepresentation>();
                var referenceObject = A.Dummy<AssemblyRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithVersion(referenceObject.Version);

                // Assert
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.Version.Should().Be(referenceObject.Version);
                actual.FilePath.Should().Be(systemUnderTest.FilePath);
                actual.FrameworkVersion.Should().Be(systemUnderTest.FrameworkVersion);
            }

            [Fact]
            public static void DeepCloneWithFilePath___Should_deep_clone_object_and_replace_FilePath_with_the_provided_filePath___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<AssemblyRepresentation>();
                var referenceObject = A.Dummy<AssemblyRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithFilePath(referenceObject.FilePath);

                // Assert
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.Version.Should().Be(systemUnderTest.Version);
                actual.FilePath.Should().Be(referenceObject.FilePath);
                actual.FrameworkVersion.Should().Be(systemUnderTest.FrameworkVersion);
            }

            [Fact]
            public static void DeepCloneWithFrameworkVersion___Should_deep_clone_object_and_replace_FrameworkVersion_with_the_provided_frameworkVersion___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<AssemblyRepresentation>();
                var referenceObject = A.Dummy<AssemblyRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithFrameworkVersion(referenceObject.FrameworkVersion);

                // Assert
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.Version.Should().Be(systemUnderTest.Version);
                actual.FilePath.Should().Be(systemUnderTest.FilePath);
                actual.FrameworkVersion.Should().Be(referenceObject.FrameworkVersion);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Equality
        {
            [Fact]
            public static void EqualsOperator___Should_return_true___When_both_sides_of_operator_are_null()
            {
                // Arrange
                AssemblyRepresentation systemUnderTest1 = null;
                AssemblyRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 == systemUnderTest2;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                AssemblyRepresentation systemUnderTest = null;

                // Act
                var result1 = systemUnderTest == ObjectForEquatableTests;
                var result2 = ObjectForEquatableTests == systemUnderTest;

                // Assert
                result1.Should().BeFalse();
                result2.Should().BeFalse();
            }

            [Fact]
            public static void EqualsOperator___Should_return_true___When_same_object_is_on_both_sides_of_operator()
            {
                // Arrange, Act
#pragma warning disable CS1718 // Comparison made to same variable
                var result = ObjectForEquatableTests == ObjectForEquatableTests;
#pragma warning restore CS1718 // Comparison made to same variable

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests == _).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First() == _.Last()).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void EqualsOperator___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests == ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_both_sides_of_operator_are_null()
            {
                // Arrange
                AssemblyRepresentation systemUnderTest1 = null;
                AssemblyRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 != systemUnderTest2;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                AssemblyRepresentation systemUnderTest = null;

                // Act
                var result1 = systemUnderTest != ObjectForEquatableTests;
                var result2 = ObjectForEquatableTests != systemUnderTest;

                // Assert
                result1.Should().BeTrue();
                result2.Should().BeTrue();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_same_object_is_on_both_sides_of_operator()
            {
                // Arrange, Act
#pragma warning disable CS1718 // Comparison made to same variable
                var result = ObjectForEquatableTests != ObjectForEquatableTests;
#pragma warning restore CS1718 // Comparison made to same variable

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests != _).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First() != _.Last()).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeTrue());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeTrue());
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests != ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_AssemblyRepresentation___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange
                AssemblyRepresentation systemUnderTest = null;

                // Act
                var result = ObjectForEquatableTests.Equals(systemUnderTest);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_AssemblyRepresentation___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_AssemblyRepresentation___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals(_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().Equals(_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_AssemblyRepresentation___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(null);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_parameter_other_is_not_of_the_same_type()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectThatIsNotTheSameTypeAsObjectForEquatableTests);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals((object)_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().Equals((object)_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_Object___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void GetHashCode___Should_not_be_equal_for_two_objects___When_objects_have_different_property_values()
            {
                // Arrange, Act
                var actualHashCodeOfReference = ObjectForEquatableTests.GetHashCode();
                var actualHashCodesInNotEqualSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => _.GetHashCode()).ToList();
                var actualEqualityCheckOfHashCodesAgainstOthersInNotEqualSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().GetHashCode() == _.Last().GetHashCode()).ToList();

                // Assert
                actualHashCodesInNotEqualSet.Should().NotContain(actualHashCodeOfReference);
                actualEqualityCheckOfHashCodesAgainstOthersInNotEqualSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void GetHashCode___Should_be_equal_for_two_objects___When_objects_have_the_same_property_values()
            {
                // Arrange, Act
                var hash1 = ObjectForEquatableTests.GetHashCode();
                var hash2 = ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests.GetHashCode();

                // Assert
                hash1.Should().Be(hash2);
            }
        }
    }
}