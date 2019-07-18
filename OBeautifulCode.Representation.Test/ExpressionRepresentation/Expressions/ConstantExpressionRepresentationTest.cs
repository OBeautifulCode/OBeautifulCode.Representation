// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test.ConstantExpressionRepresentationTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
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

    public class ConstantExpressionRepresentationTest
    {
        private static readonly ConstantExpressionRepresentation<string> ObjectForEquatableTests = A.Dummy<ConstantExpressionRepresentation<string>>();

        private static readonly ConstantExpressionRepresentation<string> ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests =
            new ConstantExpressionRepresentation<string>(
                                 ObjectForEquatableTests.Value,
                                 ObjectForEquatableTests.NodeType,
                                 ObjectForEquatableTests.Type);

        private static readonly ConstantExpressionRepresentation<string>[] ObjectsThatAreNotEqualToObjectForEquatableTests =
        {
            new ConstantExpressionRepresentation<string>(
                                 ObjectForEquatableTests.Value,
                                 A.Dummy<ExpressionType>().ThatIsNot(ObjectForEquatableTests.NodeType),
                                 A.Dummy<TypeRepresentation>().ThatIsNot(ObjectForEquatableTests.Type)),
            new ConstantExpressionRepresentation<string>(
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Value),
                                 ObjectForEquatableTests.NodeType,
                                 A.Dummy<TypeRepresentation>().ThatIsNot(ObjectForEquatableTests.Type)),
            new ConstantExpressionRepresentation<string>(
                                 A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Value),
                                 A.Dummy<ExpressionType>().ThatIsNot(ObjectForEquatableTests.NodeType),
                                 ObjectForEquatableTests.Type),
        };

        private static readonly string ObjectThatIsNotTheSameTypeAsObjectForEquatableTests = A.Dummy<string>();

        private readonly ITestOutputHelper testOutputHelper;

        public ConstantExpressionRepresentationTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<ConstantExpressionRepresentation<string>>();

            var expected = Invariant($"Representation.ConstantExpressionRepresentation: Value = {systemUnderTest.Value}, NodeType = {systemUnderTest.NodeType}, Type = {systemUnderTest.Type}.");

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void GenerateModel()
        {
            var results = CodeGenerator.GenerateForModel<ConstantExpressionRepresentation<string>>(CodeGenerator.GenerateFor.ModelImplementationPartialClass);
            this.testOutputHelper.WriteLine(results);
        }

        [Fact]
        public void GenerateTests()
        {
            var results = CodeGenerator.GenerateForModel<ConstantExpressionRepresentation<string>>(CodeGenerator.GenerateFor.ModelImplementationTestsPartialClassWithoutSerialization);
            this.testOutputHelper.WriteLine(results);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Constructing
        {
            [Fact]
            public static void ConstantExpressionRepresentation___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(ConstantExpressionRepresentation<string>);
                var expectedModelMethods = typeof(IModel<ConstantExpressionRepresentation<string>>)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<ConstantExpressionRepresentation<string>>));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }

            [Fact]
            public static void Value___Should_return_same_value_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>();
                var systemUnderTest = new ConstantExpressionRepresentation<string>(
                                 referenceObject.Value,
                                 referenceObject.NodeType,
                                 referenceObject.Type);
                var expected = referenceObject.Value;

                // Act
                var actual = systemUnderTest.Value;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void NodeType___Should_return_same_nodeType_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>();
                var systemUnderTest = new ConstantExpressionRepresentation<string>(
                                 referenceObject.Value,
                                 referenceObject.NodeType,
                                 referenceObject.Type);
                var expected = referenceObject.NodeType;

                // Act
                var actual = systemUnderTest.NodeType;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void Type___Should_return_same_type_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>();
                var systemUnderTest = new ConstantExpressionRepresentation<string>(
                                 referenceObject.Value,
                                 referenceObject.NodeType,
                                 referenceObject.Type);
                var expected = referenceObject.Type;

                // Act
                var actual = systemUnderTest.Type;

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
                var systemUnderTest = A.Dummy<ConstantExpressionRepresentation<string>>();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
                actual.Should().Be(systemUnderTest);
                actual.Should().NotBeSameAs(systemUnderTest);
                actual.Type.Should().NotBeSameAs(systemUnderTest.Type);
            }

            [Fact]
            public static void DeepCloneWithValue___Should_deep_clone_object_and_replace_Value_with_the_provided_value___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ConstantExpressionRepresentation<string>>();
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithValue(referenceObject.Value);

                // Assert
                actual.Value.Should().Be(referenceObject.Value);
                actual.NodeType.Should().Be(systemUnderTest.NodeType);
                actual.Type.Should().Be(systemUnderTest.Type);
            }

            [Fact]
            public static void DeepCloneWithNodeType___Should_deep_clone_object_and_replace_NodeType_with_the_provided_nodeType___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ConstantExpressionRepresentation<string>>();
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithNodeType(referenceObject.NodeType);

                // Assert
                actual.Value.Should().Be(systemUnderTest.Value);
                actual.NodeType.Should().Be(referenceObject.NodeType);
                actual.Type.Should().Be(systemUnderTest.Type);
            }

            [Fact]
            public static void DeepCloneWithType___Should_deep_clone_object_and_replace_Type_with_the_provided_type___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<ConstantExpressionRepresentation<string>>();
                var referenceObject = A.Dummy<ConstantExpressionRepresentation<string>>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithType(referenceObject.Type);

                // Assert
                actual.Value.Should().Be(systemUnderTest.Value);
                actual.NodeType.Should().Be(systemUnderTest.NodeType);
                actual.Type.Should().Be(referenceObject.Type);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Equality
        {
            [Fact]
            public static void EqualsOperator___Should_return_true___When_both_sides_of_operator_are_null()
            {
                // Arrange
                ConstantExpressionRepresentation<string> systemUnderTest1 = null;
                ConstantExpressionRepresentation<string> systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 == systemUnderTest2;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                ConstantExpressionRepresentation<string> systemUnderTest = null;

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
                ConstantExpressionRepresentation<string> systemUnderTest1 = null;
                ConstantExpressionRepresentation<string> systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 != systemUnderTest2;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                ConstantExpressionRepresentation<string> systemUnderTest = null;

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
            public static void Equals_with_ConstantExpressionRepresentation___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange
                ConstantExpressionRepresentation<string> systemUnderTest = null;

                // Act
                var result = ObjectForEquatableTests.Equals(systemUnderTest);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_ConstantExpressionRepresentation___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_ConstantExpressionRepresentation___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals(_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().Equals(_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_ConstantExpressionRepresentation___Should_return_true___When_objects_being_compared_have_same_property_values()
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
