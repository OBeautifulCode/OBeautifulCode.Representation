// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementInitRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using Castle.DynamicProxy.Internal;

    using FakeItEasy;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Bootstrapper.Test.CodeGeneration;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type;

    using Xunit;
    using Xunit.Abstractions;

    using static global::System.FormattableString;

    public class ElementInitRepresentationTest
    {
        private static readonly ElementInitRepresentation ObjectForEquatableTests = A.Dummy<ElementInitRepresentation>();

        private static readonly ElementInitRepresentation ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests =
            new ElementInitRepresentation(
                                 ObjectForEquatableTests.Type,
                                 ObjectForEquatableTests.AddMethod,
                                 ObjectForEquatableTests.Arguments);

        private static readonly ElementInitRepresentation[] ObjectsThatAreNotEqualToObjectForEquatableTests =
        {
            new ElementInitRepresentation(
                                 ObjectForEquatableTests.Type,
                                 A.Dummy<MethodInfoRepresentation>().ThatIsNot(ObjectForEquatableTests.AddMethod),
                                 A.Dummy<IReadOnlyList<ExpressionRepresentationBase>>().ThatIsNot(ObjectForEquatableTests.Arguments)),
            new ElementInitRepresentation(
                                 A.Dummy<TypeRepresentation>().ThatIsNot(ObjectForEquatableTests.Type),
                                 ObjectForEquatableTests.AddMethod,
                                 A.Dummy<IReadOnlyList<ExpressionRepresentationBase>>().ThatIsNot(ObjectForEquatableTests.Arguments)),
            new ElementInitRepresentation(
                                 A.Dummy<TypeRepresentation>().ThatIsNot(ObjectForEquatableTests.Type),
                                 A.Dummy<MethodInfoRepresentation>().ThatIsNot(ObjectForEquatableTests.AddMethod),
                                 ObjectForEquatableTests.Arguments),
        };

        private static readonly string ObjectThatIsNotTheSameTypeAsObjectForEquatableTests = A.Dummy<string>();

        private readonly ITestOutputHelper testOutputHelper;

        public ElementInitRepresentationTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<ElementInitRepresentation>();

            var expected = Invariant($"Representation.ElementInitRepresentation: Type = {systemUnderTest.Type}, AddMethod = {systemUnderTest.AddMethod}, Arguments = {systemUnderTest.Arguments}.");

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void GenerateModel()
        {
            var results = CodeGenerator.GenerateForModel<ElementInitRepresentation>(CodeGenerator.GenerateFor.ModelImplementationPartialClass);
            this.testOutputHelper.WriteLine(results);
        }

        [Fact]
        public void GenerateTests()
        {
            var results = CodeGenerator.GenerateForModel<ElementInitRepresentation>(CodeGenerator.GenerateFor.ModelImplementationTestsPartialClassWithoutSerialization);
            this.testOutputHelper.WriteLine(results);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Constructing
        {
            [Fact]
            public static void ElementInitRepresentation___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(ElementInitRepresentation);
                var expectedModelMethods = typeof(IModel<ElementInitRepresentation>)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<ElementInitRepresentation>));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }

            [Fact]
            public static void Type___Should_return_same_type_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ElementInitRepresentation>();
                var systemUnderTest = new ElementInitRepresentation(
                                 referenceObject.Type,
                                 referenceObject.AddMethod,
                                 referenceObject.Arguments);
                var expected = referenceObject.Type;

                // Act
                var actual = systemUnderTest.Type;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void AddMethod___Should_return_same_addMethod_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ElementInitRepresentation>();
                var systemUnderTest = new ElementInitRepresentation(
                                 referenceObject.Type,
                                 referenceObject.AddMethod,
                                 referenceObject.Arguments);
                var expected = referenceObject.AddMethod;

                // Act
                var actual = systemUnderTest.AddMethod;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void Arguments___Should_return_same_arguments_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ElementInitRepresentation>();
                var systemUnderTest = new ElementInitRepresentation(
                                 referenceObject.Type,
                                 referenceObject.AddMethod,
                                 referenceObject.Arguments);
                var expected = referenceObject.Arguments;

                // Act
                var actual = systemUnderTest.Arguments;

                // Assert
                actual.Should().Equal(expected);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Cloning
        {
            [Fact]
            public static void DeepClone___Should_deep_clone_object___When_called()
            {
                // Arrange
                var systemUnderTest = A.Dummy<ElementInitRepresentation>();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
                actual.Should().Be(systemUnderTest);
                actual.Should().NotBeSameAs(systemUnderTest);
            }

            [Fact]
            public static void DeepCloneWithType___Should_deep_clone_object_and_replace_Type_with_the_provided_type___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ElementInitRepresentation>();
                var referenceObject = A.Dummy<ElementInitRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithType(referenceObject.Type);

                // Assert
                actual.Type.Should().Be(referenceObject.Type);
                actual.AddMethod.Should().Be(systemUnderTest.AddMethod);
                actual.Arguments.Should().Equal(systemUnderTest.Arguments);
            }

            [Fact]
            public static void DeepCloneWithAddMethod___Should_deep_clone_object_and_replace_AddMethod_with_the_provided_addMethod___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ElementInitRepresentation>();
                var referenceObject = A.Dummy<ElementInitRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithAddMethod(referenceObject.AddMethod);

                // Assert
                actual.Type.Should().Be(systemUnderTest.Type);
                actual.AddMethod.Should().Be(referenceObject.AddMethod);
                actual.Arguments.Should().Equal(systemUnderTest.Arguments);
            }

            [Fact]
            public static void DeepCloneWithArguments___Should_deep_clone_object_and_replace_Arguments_with_the_provided_arguments___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ElementInitRepresentation>();
                var referenceObject = A.Dummy<ElementInitRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithArguments(referenceObject.Arguments);

                // Assert
                actual.Type.Should().Be(systemUnderTest.Type);
                actual.AddMethod.Should().Be(systemUnderTest.AddMethod);
                actual.Arguments.Should().Equal(referenceObject.Arguments);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Equality
        {
            [Fact]
            public static void EqualsOperator___Should_return_true___When_both_sides_of_operator_are_null()
            {
                // Arrange
                ElementInitRepresentation systemUnderTest1 = null;
                ElementInitRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 == systemUnderTest2;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                ElementInitRepresentation systemUnderTest = null;

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
                ElementInitRepresentation systemUnderTest1 = null;
                ElementInitRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 != systemUnderTest2;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                ElementInitRepresentation systemUnderTest = null;

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
            public static void Equals_with_ElementInitRepresentation___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange
                ElementInitRepresentation systemUnderTest = null;

                // Act
                var result = ObjectForEquatableTests.Equals(systemUnderTest);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_ElementInitRepresentation___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_ElementInitRepresentation___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals(_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().Equals(_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_ElementInitRepresentation___Should_return_true___When_objects_being_compared_have_same_property_values()
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
