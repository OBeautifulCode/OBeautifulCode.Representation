// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToStringGeneration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Linq;

    public static class ToStringGeneration
    {
        private const string TypeNameToken = "<<<TypeNameHere>>>";
        private const string ToStringToken = "<<<ToStringConstructionHere>>>";
        private const string ToStringTestToken = "<<<ToStringConstructionForTestHere>>>";

        private const string ToStringMethodCodeTemplate = @"
        /// <inheritdoc />
        public override string ToString()
        {
            var result = " + ToStringToken + @";

            return result;
        }";

        private const string ToStringTestMethodCodeTemplate = @"
        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<" + TypeNameToken + @">();

            var expected = " + ToStringTestToken + @";

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }";

        public static string GenerateToStringMethod(
            this Type type)
        {
            var toStringConstructionCode = type.GenerateToStringConstructionCode();
            var result = ToStringMethodCodeTemplate.Replace(ToStringToken, toStringConstructionCode);
            return result;
        }

        public static string GenerateToStringTestMethod(
            this Type type)
        {
            var toStringConstructionCode = type.GenerateToStringTestConstructionCode();
            var result = ToStringTestMethodCodeTemplate
                        .Replace(TypeNameToken, type.Name)
                        .Replace(ToStringTestToken, toStringConstructionCode);
            return result;
        }

        private static string GenerateToStringConstructionCode(
            this Type type)
        {
            var propertyNames = type.GetPropertiesOfConcernFromType().Select(_ => _.Name).ToList();
            return "Invariant($\"{nameof("
                 + type.Namespace
                 + ")}.{nameof("
                 + type.Name
                 + ")}: "
                 + string.Join(
                       ", ",
                       propertyNames.Select(_ => _ + " = {this." + _ + "?.ToString() ?? \"<null>\"})"))
                 + ".\")";
        }

        private static string GenerateToStringTestConstructionCode(
            this Type type)
        {
            var propertyNames = type.GetPropertiesOfConcernFromType().Select(_ => _.Name).ToList();
            return "Invariant($\""
                 + type.Namespace?.Split('.').Last()
                 + "."
                 + type.Name
                 + ": "
                 + string.Join(
                       ", ",
                       propertyNames.Select(_ => _ + " = {systemUnderTest." + _ + "})"))
                 + ".\")";
        }
    }
}