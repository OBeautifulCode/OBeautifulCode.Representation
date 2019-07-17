// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructingGeneration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Collection.Recipes;
    using static System.FormattableString;

    public static class ConstructingGeneration
    {
        private const string TypeNameToken = "<<<TypeNameHere>>>";

        private const string ConstructorParameterToken = "<<<ConstructorParameterUnderTest>>>";

        private const string ConstructorTestInflationToken = "<<<ConstructorTestMethodsInflatedGoesHere>>>";

        private const string NewObjectForArgumentTestToken = "<<<NewObjectWithOneArgumentNullHere>>>";

        private const string ConstructingTestMethodsCodeTemplate = @"
        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Constructing
        {
            [Fact]
            public static void " + TypeNameToken + @"___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(" + TypeNameToken + @");
                var expectedModelMethods = typeof(IModel<" + TypeNameToken + @">)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<" + TypeNameToken + @">));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }" + ConstructorTestInflationToken + @"
        }";

        private const string ConstructorTestMethodForArgumentCodeTemplate = @"
            [Fact]
            public static void Constructor___Should_throw_ArgumentNullException___When_parameter_" + ConstructorParameterToken + @"_is_null()
            {
                // Arrange,
                var referenceObject = A.Dummy<" + TypeNameToken + @">();

                // Act
                var actual = Record.Exception(() => " + NewObjectForArgumentTestToken + @");

                // Assert
                actual.Should().BeOfType<ArgumentNullException>();
                actual.Message.Should().Contain(""" + ConstructorParameterToken + @""");
            }";

        public static string GenerateNewLogicCodeForTypeWithSources(
            this Type                  type,
            Dictionary<string, string> propertyNameToSourceCodeMap)
        {
            if (type.GetConstructors()
                    .Any(
                         _ =>
                         {
                             var parameters = _.GetParameters();
                             var nonMatchingParameters = parameters.Select(p => p.Name)
                                                                   .SymmetricDifference(propertyNameToSourceCodeMap.Keys.Select(k => k.ToLowerFirstLetter()))
                                                                   .ToList();

                             return nonMatchingParameters.Count == 0;
                         }))
            {
                var parameterPadding = "                                 ";
                return "new " + type.Name + "(" + Environment.NewLine + parameterPadding + string.Join("," + Environment.NewLine + parameterPadding, propertyNameToSourceCodeMap.Values) + ")";
            }
            else if (type.GetPropertiesOfConcernFromType().All(_ => _.CanWrite))
            {
                return "new "
                     + type.Name
                     + "{"
                     + string.Join(", ", propertyNameToSourceCodeMap.Select(_ => Invariant($"{_.Key} = {_.Value}")))
                     + "}";
            }
            else
            {
                var propertiesAddIn = string.Join(",", type.GetPropertiesOfConcernFromType().Select(_ => _.Name));
                throw new NotSupportedException("Could not find a constructor to take properties of concern and they are not all settable: " + propertiesAddIn);
            }
        }

        public static string GenerateConstructorTestMethods(
            this Type type)
        {
            var constructorWithParameters = type.GetConstructors().SingleOrDefault(_ => _.GetParameters().Length > 0);
            var testMethods = new List<string>();
            if (constructorWithParameters != null)
            {
                // since we have parameters we'll go ahead and pad down.
                testMethods.Add(string.Empty);

                var parameters = constructorWithParameters.GetParameters();
                foreach (var parameter in parameters)
                {
                    var propertyNameToSourceCodeMap = parameters.ToDictionary(
                        k => k.Name,
                        v =>
                        {
                            var referenceObject = "referenceObject." + v.Name.ToUpperFirstLetter();
                            return v.Name == parameter.Name ? "null" : referenceObject;
                        });

                    var newObjectCode = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMap);

                    var testMethod = ConstructorTestMethodForArgumentCodeTemplate
                                    .Replace(TypeNameToken,             type.Name)
                                    .Replace(ConstructorParameterToken, parameter.Name)
                                    .Replace(NewObjectForArgumentTestToken, newObjectCode);
                    testMethods.Add(testMethod);
                }
            }

            var constructorTestInflationToken = string.Join(Environment.NewLine, testMethods);

            var result = ConstructingTestMethodsCodeTemplate
                        .Replace(TypeNameToken,                 type.Name)
                        .Replace(ConstructorTestInflationToken, constructorTestInflationToken);
            return result;
        }
    }
}