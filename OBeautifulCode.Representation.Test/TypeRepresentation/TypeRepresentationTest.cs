// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test.TypeRepresentationTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

    public partial class TypeRepresentationTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TypeRepresentationTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GenerateModel()
        {
            var results = CodeGenerator.GenerateForModel<TypeRepresentation>(CodeGenerator.GenerateFor.ModelImplementationPartialClass);
            this.testOutputHelper.WriteLine(results);
        }

        [Fact]
        public void GenerateTests()
        {
            var results = CodeGenerator.GenerateForModel<TypeRepresentation>(CodeGenerator.GenerateFor.ModelImplementationTestsPartialClassWithSerialization);
            this.testOutputHelper.WriteLine(results);
        }
    }

    public partial class TypeRepresentationTest
    {
        private static readonly TypeRepresentation ObjectForEquatableTests = A.Dummy<TypeRepresentation>();

        private static readonly TypeRepresentation ObjectThatIsEqualToButNotTheSameAsObjectForEquatableTests =
            new TypeRepresentation(
                ObjectForEquatableTests.Namespace,
                ObjectForEquatableTests.Name,
                ObjectForEquatableTests.AssemblyQualifiedName,
                ObjectForEquatableTests.GenericArguments);

        private static readonly TypeRepresentation[] ObjectsThatAreNotEqualToObjectForEquatableTests =
        {
            new TypeRepresentation(
                ObjectForEquatableTests.Namespace,
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.AssemblyQualifiedName),
                A.Dummy<IReadOnlyList<TypeRepresentation>>().ThatIsNot(ObjectForEquatableTests.GenericArguments)),
            new TypeRepresentation(
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Namespace),
                ObjectForEquatableTests.Name,
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.AssemblyQualifiedName),
                A.Dummy<IReadOnlyList<TypeRepresentation>>().ThatIsNot(ObjectForEquatableTests.GenericArguments)),
            new TypeRepresentation(
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Namespace),
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                ObjectForEquatableTests.AssemblyQualifiedName,
                A.Dummy<IReadOnlyList<TypeRepresentation>>().ThatIsNot(ObjectForEquatableTests.GenericArguments)),
            new TypeRepresentation(
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Namespace),
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.Name),
                A.Dummy<string>().ThatIsNot(ObjectForEquatableTests.AssemblyQualifiedName),
                ObjectForEquatableTests.GenericArguments),
        };

        private static readonly string ObjectThatIsNotTheSameTypeAsObjectForEquatableTests = A.Dummy<string>();

        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<TypeRepresentation>();

            var expected = Invariant($"Representation.TypeRepresentation: Namespace = {systemUnderTest.Namespace}, Name = {systemUnderTest.Name}, AssemblyQualifiedName = {systemUnderTest.AssemblyQualifiedName}, GenericArguments = {systemUnderTest.GenericArguments}.");

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Constructing
        {
            [Fact]
            public static void TypeRepresentation___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(TypeRepresentation);
                var expectedModelMethods = typeof(IModel<TypeRepresentation>)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<TypeRepresentation>));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }

            [Fact]
            public static void Namespace___Should_return_same_namespace_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<TypeRepresentation>();
                var systemUnderTest = new TypeRepresentation(
                                 referenceObject.Namespace,
                                 referenceObject.Name,
                                 referenceObject.AssemblyQualifiedName,
                                 referenceObject.GenericArguments);
                var expected = referenceObject.Namespace;

                // Act
                var actual = systemUnderTest.Namespace;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void Name___Should_return_same_name_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<TypeRepresentation>();
                var systemUnderTest = new TypeRepresentation(
                                 referenceObject.Namespace,
                                 referenceObject.Name,
                                 referenceObject.AssemblyQualifiedName,
                                 referenceObject.GenericArguments);
                var expected = referenceObject.Name;

                // Act
                var actual = systemUnderTest.Name;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void AssemblyQualifiedName___Should_return_same_assemblyQualifiedName_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<TypeRepresentation>();
                var systemUnderTest = new TypeRepresentation(
                                 referenceObject.Namespace,
                                 referenceObject.Name,
                                 referenceObject.AssemblyQualifiedName,
                                 referenceObject.GenericArguments);
                var expected = referenceObject.AssemblyQualifiedName;

                // Act
                var actual = systemUnderTest.AssemblyQualifiedName;

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void GenericArguments___Should_return_same_genericArguments_parameter_passed_to_constructor___When_getting()
            {
                // Arrange,
                var referenceObject = A.Dummy<TypeRepresentation>();
                var systemUnderTest = new TypeRepresentation(
                                 referenceObject.Namespace,
                                 referenceObject.Name,
                                 referenceObject.AssemblyQualifiedName,
                                 referenceObject.GenericArguments);
                var expected = referenceObject.GenericArguments;

                // Act
                var actual = systemUnderTest.GenericArguments;

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
                var systemUnderTest = A.Dummy<TypeRepresentation>();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
                actual.Should().Be(systemUnderTest);
                actual.Should().NotBeSameAs(systemUnderTest);

                if (actual.GenericArguments?.Any() ?? false)
                {
                    actual.GenericArguments.Should().NotBeSameAs(systemUnderTest.GenericArguments);
                }
            }

            [Fact]
            public static void DeepCloneWithNamespace___Should_deep_clone_object_and_replace_Namespace_with_the_provided_namespace___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<TypeRepresentation>();
                var referenceObject = A.Dummy<TypeRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithNamespace(referenceObject.Namespace);

                // Assert
                actual.Namespace.Should().Be(referenceObject.Namespace);
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.AssemblyQualifiedName.Should().Be(systemUnderTest.AssemblyQualifiedName);

                if (actual.GenericArguments?.Any() ?? false)
                {
                    actual.GenericArguments.Should().Equal(systemUnderTest.GenericArguments);
                }
            }

            [Fact]
            public static void DeepCloneWithName___Should_deep_clone_object_and_replace_Name_with_the_provided_name___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<TypeRepresentation>();
                var referenceObject = A.Dummy<TypeRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithName(referenceObject.Name);

                // Assert
                actual.Namespace.Should().Be(systemUnderTest.Namespace);
                actual.Name.Should().Be(referenceObject.Name);
                actual.AssemblyQualifiedName.Should().Be(systemUnderTest.AssemblyQualifiedName);
                actual.GenericArguments.Should().Equal(systemUnderTest.GenericArguments);
            }

            [Fact]
            public static void DeepCloneWithAssemblyQualifiedName___Should_deep_clone_object_and_replace_AssemblyQualifiedName_with_the_provided_assemblyQualifiedName___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<TypeRepresentation>();
                var referenceObject = A.Dummy<TypeRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithAssemblyQualifiedName(referenceObject.AssemblyQualifiedName);

                // Assert
                actual.Namespace.Should().Be(systemUnderTest.Namespace);
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.AssemblyQualifiedName.Should().Be(referenceObject.AssemblyQualifiedName);
                actual?.GenericArguments.Should().Equal(systemUnderTest.GenericArguments);
            }

            [Fact]
            public static void DeepCloneWithGenericArguments___Should_deep_clone_object_and_replace_GenericArguments_with_the_provided_genericArguments___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<TypeRepresentation>();
                var referenceObject = A.Dummy<TypeRepresentation>().ThatIsNot(systemUnderTest);

                // Act
                var actual = systemUnderTest.DeepCloneWithGenericArguments(referenceObject.GenericArguments);

                // Assert
                actual.Namespace.Should().Be(systemUnderTest.Namespace);
                actual.Name.Should().Be(systemUnderTest.Name);
                actual.AssemblyQualifiedName.Should().Be(systemUnderTest.AssemblyQualifiedName);

                if (actual?.GenericArguments.Any() ?? false)
                {
                    actual.GenericArguments.Should().Equal(referenceObject.GenericArguments);
                    actual.GenericArguments.Should().NotBeSameAs(referenceObject.GenericArguments);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Grouping construct for unit test runner.")]
        public static class Equality
        {
            [Fact]
            public static void EqualsOperator___Should_return_true___When_both_sides_of_operator_are_null()
            {
                // Arrange
                TypeRepresentation systemUnderTest1 = null;
                TypeRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 == systemUnderTest2;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                TypeRepresentation systemUnderTest = null;

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
                TypeRepresentation systemUnderTest1 = null;
                TypeRepresentation systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 != systemUnderTest2;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                TypeRepresentation systemUnderTest = null;

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
            public static void Equals_with_TypeRepresentation___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange
                TypeRepresentation systemUnderTest = null;

                // Act
                var result = ObjectForEquatableTests.Equals(systemUnderTest);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_TypeRepresentation___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_TypeRepresentation___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals(_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select(_ => _.First().Equals(_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_TypeRepresentation___Should_return_true___When_objects_being_compared_have_same_property_values()
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

        // ReSharper restore InconsistentNaming
    }
}
