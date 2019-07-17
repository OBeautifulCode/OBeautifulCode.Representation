// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelObjectCodeGenerator.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Validation.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Helper methods for creating object equality and hash code text via reflection.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class ModelObjectCodeGenerator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Want generic for future ability.")]
        public static string GenerateCodeForModelObject<T>()
        {
            var type = typeof(T);

            var dummyFactorySnippet = type.GenerateCodeForDummyFactory();
            var tests = type.GenerateCodeForTests();
            var modelMethods = type.GenerateCodeForModel();

            var result = string.Join(
                Environment.NewLine + "--------------------------------------------------------------------------" + Environment.NewLine,
                new[]
                {
                    dummyFactorySnippet,
                    tests,
                    modelMethods,
                });

            return result;
        }

        private static string GenerateCodeForModel(
            this Type type)
        {
            var result = string.Join(
                Environment.NewLine,
                new[]
                {
                    type.GenerateEqualityMethods(),
                    type.GenerateGetHashCodeMethod(),
                    type.GenerateCloningMethods(),
                });

            return result;
        }

        private static string GenerateCodeForTests(
            this Type type)
        {
            var result = string.Join(
                Environment.NewLine,
                new[]
                {
                    type.GenerateSerializationTestFields(),
                    type.GenerateEqualityTestFields(),
                    type.GenerateWireUpOutputLogic(),
                    type.GenerateToStringTestMethod(),
                    type.GenerateGenerationTestMethod(),
                    type.GenerateConstructorTestMethods(),
                    type.GenerateCloningTestMethods(),
                    type.GenerateSerializationTestMethods(),
                    type.GenerateEqualityTestMethods(),
                });

            return result;
        }
    }
}