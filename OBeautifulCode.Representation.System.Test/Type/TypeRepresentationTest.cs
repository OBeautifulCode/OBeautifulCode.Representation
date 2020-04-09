// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Text.RegularExpressions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Representation.System.Test.Internal;

    using Xunit;

    using static global::System.FormattableString;

    public static partial class TypeRepresentationTest
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static TypeRepresentationTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'namespace' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 null,
                                                 referenceObject.Name,
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "namespace" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'namespace' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.Name,
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "namespace", "white space" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'name' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 null,
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "name" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'name' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "name", "white space" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'assemblyName' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 referenceObject.Name,
                                                 null,
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "assemblyName" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'assemblyName' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 referenceObject.Name,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.AssemblyVersion,
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "assemblyName", "white space" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'assemblyVersion' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 referenceObject.Name,
                                                 referenceObject.AssemblyName,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "assemblyVersion", "white space" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'genericArguments' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 referenceObject.Name,
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 null);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "genericArguments" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'genericArguments' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                                 referenceObject.Namespace,
                                                 referenceObject.Name,
                                                 referenceObject.AssemblyName,
                                                 referenceObject.AssemblyVersion,
                                                 new TypeRepresentation[0].Concat(referenceObject.GenericArguments).Concat(new TypeRepresentation[] { null }).Concat(referenceObject.GenericArguments).ToList());

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "genericArguments", "contains at least one null element" },
                    });

            StringRepresentationTestScenarios
                .AddScenario(() =>
                    new StringRepresentationTestScenario<TypeRepresentation>
                    {
                        Name = "ToString should return the same string as BuildAssemblyQualifiedName() when called",
                        SystemUnderTestExpectedStringRepresentationFunc = () =>
                        {
                            var systemUnderTest = A.Dummy<TypeRepresentation>();

                            var result = new SystemUnderTestExpectedStringRepresentation<TypeRepresentation>
                            {
                                SystemUnderTest = systemUnderTest,
                                ExpectedStringRepresentation = systemUnderTest.BuildAssemblyQualifiedName(),
                            };

                            return result;
                        },
                    })
                .AddScenario(() =>
                    new StringRepresentationTestScenario<TypeRepresentation>
                    {
                        Name = "ToString should return the Assembly Qualified Name when called",
                        SystemUnderTestExpectedStringRepresentationFunc = () =>
                        {
                            var systemUnderTest = typeof(IReadOnlyCollection<string>[,]).ToRepresentation();

                            var result = new SystemUnderTestExpectedStringRepresentation<TypeRepresentation>
                            {
                                SystemUnderTest = systemUnderTest,
                                ExpectedStringRepresentation = "System.Collections.Generic.IReadOnlyCollection`1[[System.String, mscorlib, Version=4.0.0.0]][,], mscorlib, Version=4.0.0.0",
                            };

                            return result;
                        },
                    });
        }

        [Fact]
        public static void Constructor___Should_not_throw___When_parameter_assemblyVersion_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new TypeRepresentation(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>(), null, new TypeRepresentation[0]));

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void BuildAssemblyQualifiedName___Should_build_the_expected_assembly_qualified_name___When_includeVersion_is_true()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting();

            var expected = types.Select(_ => Regex.Replace(_.AssemblyQualifiedName, ", Culture=.*?, PublicKeyToken=[a-z0-9]*", string.Empty)).ToList();

            // Act
            var actual = types.Select(_ => _.ToRepresentation().BuildAssemblyQualifiedName(includeVersion: true)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void BuildAssemblyQualifiedName___Should_build_the_expected_assembly_qualified_name___When_includeVersion_is_false()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting();

            var expected = types.Select(_ => Regex.Replace(_.AssemblyQualifiedName, ", Version=.*?, Culture=.*?, PublicKeyToken=[a-z0-9]*", string.Empty)).ToList();

            // Act
            var actual = types.Select(_ => _.ToRepresentation().BuildAssemblyQualifiedName(includeVersion: false)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void BuildAssemblyQualifiedName___Should_build_assembly_qualified_name_without_version___When_includeVersion_is_true_but_AssemblyVersion_is_null()
        {
            // Arrange
            var intRepresentation = new TypeRepresentation("System", "Int32", "ass1", "1.0.0.0", new TypeRepresentation[0]);

            var stringRepresentation = new TypeRepresentation("System", "String", "ass2", "2.0.0.0", new TypeRepresentation[0]);

            var guidRepresentation = new TypeRepresentation("System", "Guid", "ass3", null, new TypeRepresentation[0]);

            var dictionaryRepresentation = new TypeRepresentation("System", "IReadOnlyDictionary`2", "ass4", "3.0.0.0", new TypeRepresentation[] { stringRepresentation, guidRepresentation });

            var systemUnderTest = new TypeRepresentation("System", "Dictionary`2", "ass5", null, new[] { dictionaryRepresentation, intRepresentation });

            // Act
            var actual = systemUnderTest.BuildAssemblyQualifiedName(includeVersion: true);

            // Assert
            actual.AsTest().Must().BeEqualTo("System.Dictionary`2[[System.IReadOnlyDictionary`2[[System.String, ass2, Version=2.0.0.0],[System.Guid, ass3]], ass4, Version=3.0.0.0],[System.Int32, ass1, Version=1.0.0.0]], ass5");
        }

        [Fact]
        public static void BuildAssemblyQualifiedName___Should_build_assembly_qualified_name_without_version___When_includeVersion_is_false_and_AssemblyVersion_is_null()
        {
            // Arrange
            var intRepresentation = new TypeRepresentation("System", "Int32", "ass1", "1.0.0.0", new TypeRepresentation[0]);

            var stringRepresentation = new TypeRepresentation("System", "String", "ass2", "2.0.0.0", new TypeRepresentation[0]);

            var guidRepresentation = new TypeRepresentation("System", "Guid", "ass3", null, new TypeRepresentation[0]);

            var dictionaryRepresentation = new TypeRepresentation("System", "IReadOnlyDictionary`2", "ass4", "3.0.0.0", new TypeRepresentation[] { stringRepresentation, guidRepresentation });

            var systemUnderTest = new TypeRepresentation("System", "Dictionary`2", "ass5", null, new[] { dictionaryRepresentation, intRepresentation });

            // Act
            var actual = systemUnderTest.BuildAssemblyQualifiedName(includeVersion: false);

            // Assert
            actual.AsTest().Must().BeEqualTo("System.Dictionary`2[[System.IReadOnlyDictionary`2[[System.String, ass2],[System.Guid, ass3]], ass4],[System.Int32, ass1]], ass5");
        }
    }
}