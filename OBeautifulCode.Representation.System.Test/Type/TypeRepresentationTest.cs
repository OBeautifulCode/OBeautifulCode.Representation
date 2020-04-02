// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Representation.System.Test.Internal;

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
                                referenceObject.AssemblyQualifiedName,
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
                                referenceObject.AssemblyQualifiedName,
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
                                referenceObject.AssemblyQualifiedName,
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
                                referenceObject.AssemblyQualifiedName,
                                referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "name", "white space" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'assemblyQualifiedName' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                referenceObject.Namespace,
                                referenceObject.Name,
                                null,
                                referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "assemblyQualifiedName" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TypeRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'assemblyQualifiedName' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentation>();

                            var result = new TypeRepresentation(
                                referenceObject.Namespace,
                                referenceObject.Name,
                                Invariant($"  {Environment.NewLine}  "),
                                referenceObject.GenericArguments);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "assemblyQualifiedName", "white space" },
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
                                referenceObject.AssemblyQualifiedName,
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
                                referenceObject.AssemblyQualifiedName,
                                new TypeRepresentation[0].Concat(referenceObject.GenericArguments).Concat(new TypeRepresentation[] { null }).Concat(referenceObject.GenericArguments).ToList());

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "genericArguments", "contains at least one null element" },
                    });
        }
    }
}