// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerationShared.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using static System.FormattableString;

    public static class CodeGenerationShared
    {
        private const string TypeNameToken = "<<<<TypeNameHere>>>";

        private const string WireUpOutputCodeTemplate = @"
        private readonly ITestOutputHelper testOutputHelper;

        public " + TypeNameToken + @"Test(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }";

        private const string GenerationTestMethodCodeTemplate = @"
        [Fact]
        public void Generate()
        {
            var results = ModelObjectCodeGenerator.GenerateCodeForModelObject<" + TypeNameToken + @">();
            this.testOutputHelper.WriteLine(results);
        }";

        private static readonly Type[] DictionaryTypes = new[]
                                                         {
                                                             typeof(Dictionary<,>),
                                                             typeof(IDictionary<,>),
                                                             typeof(ReadOnlyDictionary<,>),
                                                             typeof(IReadOnlyDictionary<,>),
                                                             typeof(ConcurrentDictionary<,>),
                                                         };

        private static readonly Type[] CollectionTypes = new[]
                                                         {
                                                             typeof(Collection<>),
                                                             typeof(ICollection<>),
                                                             typeof(ReadOnlyCollection<>),
                                                             typeof(IReadOnlyCollection<>),
                                                             typeof(List<>),
                                                             typeof(IList<>),
                                                             typeof(IReadOnlyList<>),
                                                         };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want Type to be the type.")]
        public static string GenerateWireUpOutputLogic(
            this Type type)
        {
            var result = WireUpOutputCodeTemplate.Replace(TypeNameToken, type.TreatedTypeName());
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want Type to be the type.")]
        public static string GenerateGenerationTestMethod(
            this Type type)
        {
            var result = GenerationTestMethodCodeTemplate.Replace(TypeNameToken, type.TreatedTypeName());
            return result;
        }

        public static string GenerateFluentEqualityStatement(
            this Type type,
            string actual,
            string expected)
        {
            var result = Invariant($"{actual}.Should().{(type.IsAssignableToAnyDictionary() || type.IsAssignableToAnyCollection() ? "Equal" : "Be")}({expected});");
            return result;
        }

        public static PropertyInfo[] GetPropertiesOfConcernFromType(
            this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Lowercase is correct here.")]
        public static string TreatedTypeName(
            this Type type)
        {
            if (type == typeof(string))
            {
                return typeof(string).Name.ToLowerInvariant();
            }
            else if (type == typeof(int))
            {
                return "int";
            }
            else if (type == typeof(bool))
            {
                return "bool";
            }
            else if (type.IsGenericType)
            {
                var typeName                  = type.Name.Split('`').First();
                var genericArguments          = type.GetGenericArguments();
                var genericArgumentsTypeNames = genericArguments.Select(_ => _.TreatedTypeName());
                return typeName + "<" + string.Join(", ", genericArgumentsTypeNames) + ">";
            }
            else
            {
                return type.Name;
            }
        }

        public static bool IsAssignableToAnyDictionary(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericType = type.GetGenericTypeDefinition();

            var result = DictionaryTypes.Any(_ => genericType == _);
            return result;
        }

        public static bool IsAssignableToAnyCollection(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericType = type.GetGenericTypeDefinition();

            var result = CollectionTypes.Any(_ => genericType == _);
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Lowercase is correct here.")]
        public static string ToLowerFirstLetter(
            this string input)
        {
            if (input == null)
            {
                return null;
            }

            if (input.Length == 1)
            {
                return input.ToLowerInvariant();
            }

            return input[0].ToString().ToLowerInvariant() + input.Substring(1);
        }

        public static string ToUpperFirstLetter(
            this string input)
        {
            if (input == null)
            {
                return null;
            }

            if (input.Length == 1)
            {
                return input.ToUpperInvariant();
            }

            return input[0].ToString().ToUpperInvariant() + input.Substring(1);
        }
    }
}