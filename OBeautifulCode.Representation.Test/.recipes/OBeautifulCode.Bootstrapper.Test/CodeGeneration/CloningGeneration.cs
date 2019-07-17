// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloningGeneration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Validation.Recipes;
    using static System.FormattableString;

    public static class CloningGeneration
    {
        private const string TypeNameToken = "<<<TypeNameHere>>>";
        private const string DeepCloneToken = "<<<DeepCloneLogicHere>>>";
        private const string DeepCloneWithInflationToken = "<<<DeepCloneInflationHere>>>";
        private const string NewObjectForDeepCloneWithToken = "<<<DeepCloneWithNewObjectHere>>>";
        private const string DeepCloneWithAssertLogicToken = "<<<DeepCloneWithTestAssertionsHere>>>";
        private const string AssertDeepCloneToken = "<<<AssertDeepCloneHere>>>";
        private const string DeepCloneWithTestInflationToken = "<<<DeepCloneTestInflationHere>>>";
        private const string PropertyNameToken = "<<<PropertyNameHere>>>";
        private const string ParameterNameToken = "<<<ParameterNameHere>>>";

        private const string CloningMethodsCodeTemplate = @"
        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public " + TypeNameToken + @" DeepClone()
        {
            var result = " + DeepCloneToken + @";

            return result;
        }" + DeepCloneWithInflationToken;

        private const string DeepCloneWithMethodCodeTemplate = @"
        /// <summary>
        /// Deep clones this object with a new <paramref name=""" + ParameterNameToken + @""" />.
        /// </summary>
        /// <param name=""" + ParameterNameToken + @""">The new <see cref=""" + PropertyNameToken + @""" />.</param>
        /// <returns>New <see cref=""" + TypeNameToken + @""" /> using the specified <paramref name=""" + ParameterNameToken + @""" /> for <see cref=""" + PropertyNameToken + @""" /> and a deep clone of every other property.</returns>
        public " + TypeNameToken + @" DeepCloneWith" + PropertyNameToken + @"(string " + ParameterNameToken + @")
        {
            var result = " + NewObjectForDeepCloneWithToken + @";
            return result;
        }";

        private const string CloningTestMethodsCodeTemplate = @"
        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Cloning
        {
            [Fact]
            public static void DeepClone___Should_deep_clone_object___When_called()
            {
                // Arrange
                var systemUnderTest = A.Dummy<" + TypeNameToken + @">();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
               actual.Should().Be(systemUnderTest);
               actual.Should().NotBeSameAs(systemUnderTest);
               " + AssertDeepCloneToken + @"
            }" + DeepCloneWithTestInflationToken + @"
        }";

        private const string DeepCloneWithTestMethodCodeTemplate = @"
            public static void DeepCloneWith" + PropertyNameToken + @"___Should_deep_clone_object_and_replace_" + PropertyNameToken + @"_with_the_provided_" + ParameterNameToken + @"___When_called()
            {
                // Arrange,
                var systemUnderTest = A.Dummy<" + TypeNameToken + @">();
                var referenceObject = A.Dummy<" + TypeNameToken + @">().ThatIsNot(systemUnderTest);
                
                // Act
                var actual = systemUnderTest.DeepCloneWith" + PropertyNameToken + @"(referenceObject." + PropertyNameToken + @");

                // Assert
               " + DeepCloneWithAssertLogicToken + @"
            }
";

        public static string GenerateCloningMethods(
            this Type type)
        {
            var properties = type.GetPropertiesOfConcernFromType();

            var propertyNameToCloneMethodMap = properties.ToDictionary(
                k => k.Name,
                v => v.PropertyType.GenerateCloningLogicCodeForType("this." + v.Name));

            var deepCloneToken = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToCloneMethodMap);

            var parameters = type.GetConstructors().SingleOrDefault(_ => _.GetParameters().Length > 1)?.GetParameters().ToList();
            var deepCloneWithMethods = new List<string>();
            if (parameters != null)
            {
                // since we have parameters we'll go ahead and pad down.
                deepCloneWithMethods.Add(string.Empty);

                foreach (var parameter in parameters)
                {
                    var propertyNameToSourceCodeMap = parameters.ToDictionary(
                        k => k.Name,
                        v =>
                        {
                            var referenceObject = "this." + v.Name.ToUpperFirstLetter();
                            var referenceItemCloned = v.ParameterType.GenerateCloningLogicCodeForType(referenceObject);
                            return v.Name == parameter.Name ? parameter.Name : referenceItemCloned;
                        });

                    var newObjectCode = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMap);

                    var testMethod = DeepCloneWithMethodCodeTemplate
                                    .Replace(TypeNameToken,                  type.Name)
                                    .Replace(PropertyNameToken,                  parameter.Name.ToUpperFirstLetter())
                                    .Replace(ParameterNameToken,                  parameter.Name)
                                    .Replace(NewObjectForDeepCloneWithToken, newObjectCode);
                    deepCloneWithMethods.Add(testMethod);
                }
            }

            var deepCloneWithInflationToken = string.Join(Environment.NewLine, deepCloneWithMethods);

            var result = CloningMethodsCodeTemplate.Replace(TypeNameToken, type.Name)
                                                   .Replace(DeepCloneToken, deepCloneToken)
                                                   .Replace(DeepCloneWithInflationToken, deepCloneWithInflationToken);

            return result;
        }

        public static string GenerateCloningTestMethods(
            this Type type)
        {
            var properties = type.GetPropertiesOfConcernFromType();
            var assertDeepCloneSet = properties.Where(_ => _.PropertyType.IsByRef).Select(_ => Invariant($"actual.{_.Name}.Should().NotBeSameAs(systemUnderTest.{_.Name})")).ToList();
            var assertDeepCloneToken = string.Join(Environment.NewLine + "               ", assertDeepCloneSet);

            var parameters           = type.GetConstructors().SingleOrDefault(_ => _.GetParameters().Length > 1)?.GetParameters().ToList();
            var deepCloneWithTestMethods = new List<string>();
            if (parameters != null)
            {
                // since we have parameters we'll go ahead and pad down.
                deepCloneWithTestMethods.Add(string.Empty);

                foreach (var parameter in parameters)
                {
                    var assertDeepCloneWithSet = parameters.Select(
                                                                _ =>
                                                                {
                                                                    var sourceName = _.Name == parameter.Name ? "reference" : "systemUnderTest";
                                                                    var resultAssert =
                                                                        Invariant($"actual.{_.Name.ToUpperFirstLetter()}.Should().Be({sourceName}.{_.Name.ToUpperFirstLetter()})");
                                                                    if (parameter.ParameterType.IsByRef || parameter.ParameterType == typeof(string))
                                                                    {
                                                                        resultAssert +=
                                                                            Environment.NewLine
                                                                          + Invariant(
                                                                                $"actual.{_.Name.ToUpperFirstLetter()}.Should().NotBeSameAs({sourceName}.{_.Name.ToUpperFirstLetter()})");
                                                                    }

                                                                    return resultAssert;
                                                                })
                                                           .ToList();
                    var assertDeepCloneWithToken = string.Join(Environment.NewLine + "               ", assertDeepCloneWithSet);

                    var testMethod = DeepCloneWithTestMethodCodeTemplate
                                    .Replace(TypeNameToken,                  type.Name)
                                    .Replace(PropertyNameToken,              parameter.Name.ToUpperFirstLetter())
                                    .Replace(ParameterNameToken,             parameter.Name)
                                    .Replace(DeepCloneWithAssertLogicToken, assertDeepCloneWithToken);
                    deepCloneWithTestMethods.Add(testMethod);
                }
            }

            var deepCloneWithTestInflationToken = string.Join(Environment.NewLine, deepCloneWithTestMethods);

            var result = CloningTestMethodsCodeTemplate.Replace(TypeNameToken, type.Name)
                                                       .Replace(AssertDeepCloneToken, assertDeepCloneToken)
                                                       .Replace(DeepCloneWithTestInflationToken, deepCloneWithTestInflationToken);

            return result;
        }

        private static string GenerateCloningLogicCodeForType(
            this Type type,
            string    cloneSourceCode)
        {
            type.Named(nameof(type)).Must().NotBeNull();

            string result;
            if (type.IsAssignableToAnyDictionary())
            {
                var genericArguments = type.GetGenericArguments();
                genericArguments.Length.Named(Invariant($"Number{nameof(genericArguments)}Of{nameof(type)}.{nameof(type)}For{type.Name}"))
                                .Must()
                                .BeEqualTo(2);

                var keyType    = genericArguments.First();
                var valueType  = genericArguments.Last();
                var keyClone   = keyType.GenerateCloningLogicCodeForType("k.Key");
                var valueClone = valueType.GenerateCloningLogicCodeForType("v.Value");
                result = Invariant($"{cloneSourceCode}?.ToDictionary(k => {keyClone}, v => {valueClone})");
            }
            else if (type.IsAssignableToAnyCollection())
            {
                var genericArguments = type.GetGenericArguments();
                var valueType        = genericArguments.Single();
                var valueClone       = valueType.GenerateCloningLogicCodeForType("_");
                result = Invariant($"{cloneSourceCode}?.Select(_ => {valueClone}).ToList()");
            }
            else if (type == typeof(string))
            {
                // string should be cloned using it's existing interface.
                result = Invariant($"{cloneSourceCode}?.Clone().ToString()");
            }
            else if (type.IsByRef)
            {
                // assume that we are driving the DeepClone convention and it exists.
                result = Invariant($"{cloneSourceCode}?.DeepClone()");
            }
            else
            {
                // this is just a copy of the item anyway (like bool, int, Enumerations, structs like DateTime, etc.).
                result = cloneSourceCode;
            }

            return result;
        }
    }
}