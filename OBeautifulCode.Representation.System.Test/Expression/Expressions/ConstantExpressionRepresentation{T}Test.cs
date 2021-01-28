// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation{T}Test.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;

    public static partial class ConstantExpressionRepresentationTTest
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ConstantExpressionRepresentationTTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ConstantExpressionRepresentation<Version>>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'type' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ConstantExpressionRepresentation<Version>>();

                            var result = new ConstantExpressionRepresentation<Version>(
                                                 null,
                                                 referenceObject.NodeType,
                                                 referenceObject.Value);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "type" },
                    });
        }
    }
}