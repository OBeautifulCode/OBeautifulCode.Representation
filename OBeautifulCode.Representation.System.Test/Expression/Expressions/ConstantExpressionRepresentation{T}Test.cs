// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionRepresentation{T}Test.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;

    public static partial class ConstantExpressionRepresentationTest
    {
        static ConstantExpressionRepresentationTest()
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