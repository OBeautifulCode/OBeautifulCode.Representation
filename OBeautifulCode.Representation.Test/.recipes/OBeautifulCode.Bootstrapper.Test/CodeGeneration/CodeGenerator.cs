// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="OBeautifulCode">
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
    public static partial class CodeGenerator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames", Justification = "Name/spelling is correct.")]
        [Flags]
        public enum GenerateFor
        {
            None = 0,
            ModelDummyFactorySnippet = 1,
            ModelImplementationPartialClass = 2,
            ModelImplementationTestsPartialClassWithSerialization = 4,
            ModelImplementationTestsPartialClassWithoutSerialization = 8,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Want generic for future ability.")]
        public static string GenerateForModel<T>(GenerateFor kind)
        {
            var type = typeof(T);

            var generatedKindSet = new List<string>();
            if (kind.HasFlag(GenerateFor.ModelDummyFactorySnippet))
            {
                var dummyFactorySnippet = type.GenerateCodeForDummyFactory();
                generatedKindSet.Add(dummyFactorySnippet);
            }

            if (kind.HasFlag(GenerateFor.ModelImplementationPartialClass))
            {
                var modelMethods = type.GenerateCodeForModel();
                generatedKindSet.Add(modelMethods);
            }

            if (kind.HasFlag(GenerateFor.ModelImplementationTestsPartialClassWithSerialization) || kind.HasFlag(GenerateFor.ModelImplementationTestsPartialClassWithoutSerialization))
            {
                var tests = type.GenerateCodeForTests(kind);
                generatedKindSet.Add(tests);
            }

            var result = string.Join(
                Environment.NewLine + "--------------------------------------------------------------------------" + Environment.NewLine,
                generatedKindSet);

            return result;
        }

        private static string GenerateCodeForModel(
            this Type type)
        {
            var items = new List<string>();
            items.Add(Invariant($"public partial class {type.TreatedTypeName()} : IModel<{type.TreatedTypeName()}>"));
            items.Add("{");
            items.Add("    " + type.GenerateEqualityMethods());
            items.Add("    " + type.GenerateGetHashCodeMethod());
            items.Add("    " + type.GenerateCloningMethods());
            items.Add("    " + type.GenerateToStringMethod());
            items.Add("}");

            var result = string.Join(
                Environment.NewLine,
                items);

            return result;
        }

        private static string GenerateCodeForTests(
            this Type   type,
            GenerateFor kind)
        {
            var items = new List<string>();
            items.Add(Invariant($"public partial class {type.TreatedTypeName()}Test"));
            items.Add("{");

            if (kind.HasFlag(GenerateFor.ModelImplementationTestsPartialClassWithSerialization))
            {
                items.Add("    " + type.GenerateSerializationTestFields());
            }

            items.Add("    " + type.GenerateEqualityTestFields());
            items.Add("    " + type.GenerateToStringTestMethod());
            items.Add("    " + type.GenerateConstructorTestMethods());
            items.Add("    " + type.GenerateCloningTestMethods());

            if (kind.HasFlag(GenerateFor.ModelImplementationTestsPartialClassWithSerialization))
            {
                items.Add("    " + type.GenerateSerializationTestMethods());
            }

            items.Add("    " + type.GenerateEqualityTestMethods());
            items.Add("}");

            var result = string.Join(
                Environment.NewLine,
                items);

            return result;
        }
    }
}