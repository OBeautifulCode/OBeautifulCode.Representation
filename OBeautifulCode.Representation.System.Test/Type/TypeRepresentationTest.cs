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

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;

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
                        Name = "constructor should throw ArgumentException when parameter 'genericArguments' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>().ThatIs(_ => _.IsClosedGenericType());

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

            ConstructorPropertyAssignmentTestScenarios
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<TypeRepresentation>
                    {
                        Name = "GenericArguments should return null, when 'genericArguments' parameter passed to constructor is null, when getting",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new SystemUnderTestExpectedPropertyValue<TypeRepresentation>
                            {
                                SystemUnderTest = new TypeRepresentation(
                                    referenceObject.Namespace,
                                    referenceObject.Name,
                                    referenceObject.AssemblyName,
                                    referenceObject.AssemblyVersion,
                                    null),
                                ExpectedPropertyValue = null,
                            };

                            return result;
                        },
                        PropertyName = "GenericArguments",
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
            var actual = Record.Exception(() => new TypeRepresentation(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>(), null, Some.ReadOnlyDummies<TypeRepresentation>()));

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Constructor___Should_not_throw___When_parameter_genericArguments_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new TypeRepresentation(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>(), null, null));

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Constructor___Should_not_throw___When_parameter_genericArguments_is_empty()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new TypeRepresentation(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>(), null, new TypeRepresentation[0]));

            // Assert
            actual.AsTest().Must().BeNull();
        }
    }
}