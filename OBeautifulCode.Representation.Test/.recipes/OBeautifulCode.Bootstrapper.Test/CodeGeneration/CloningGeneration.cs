// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloningGeneration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Linq;
    using OBeautifulCode.Validation.Recipes;
    using static System.FormattableString;

    public static class CloningGeneration
    {
        private const string TypeNameToken = "<<<TypeNameHere>>>";
        private const string DeepCloneToken = "<<<DeepCloneLogicHere>>>";
        private const string AssertDeepCloneToken = "<<<AssertDeepCloneHere>>>";
        private const string DeepCloneWithTestInflationToken = "<<<DeepCloneTestInflationHere>>>";

        private const string CloningMethodsCodeTemplate = @"
        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public " + TypeNameToken + @" DeepClone()
        {
            var result = " + DeepCloneToken + @";

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

        public static string GenerateCloningMethods(
            this Type type)
        {
            var properties = type.GetPropertiesOfConcernFromType();

            var propertyNameToCloneMethodMap = properties.ToDictionary(
                k => k.Name,
                v => v.PropertyType.GenerateCloningLogicCodeForType("this." + v.Name));

            var deepCloneToken = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToCloneMethodMap);

            var result = CloningMethodsCodeTemplate.Replace(TypeNameToken, type.Name)
                                                   .Replace(DeepCloneToken, deepCloneToken);

            return result;
        }

        public static string GenerateCloningTestMethods(
            this Type type)
        {
            var properties = type.GetPropertiesOfConcernFromType();
            var assertDeepCloneSet = properties.Select(_ => Invariant($"actual.{_.Name}.Should().NotBeSameAs(systemUnderTest.{_.Name})")).ToList();
            var assertDeepCloneToken = string.Join(Environment.NewLine + "               ", assertDeepCloneSet);

            var result = CloningTestMethodsCodeTemplate.Replace(TypeNameToken, type.Name)
                                                       .Replace(AssertDeepCloneToken, assertDeepCloneToken);

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