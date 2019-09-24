// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    using static System.FormattableString;

    public static class TypeExtensionsTest
    {
        private static readonly string MsCorLibNameAndVersion = "mscorlib (4.0.0.0)";
        private static readonly string ThisAssemblyNameAndVersion = "OBeautifulCode.Representation.Test" + " (" + Assembly.GetExecutingAssembly().GetName().Version + ")";

        [Fact]
        public static void ToStringCompilable___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => Representation.TypeExtensions.ToStringCompilable(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("type");
        }

        [Fact]
        public static void ToStringCompilable___Should_throw_NotSupportedException___When_parameter_throwIfNoCompilableStringExists_is_true_and_parameter_type_is_an_anonymous_type()
        {
            // Arrange
            var types = new[]
            {
                new { Anonymous = true }.GetType(),
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: true))).ToList();

            // Assert
            actuals.Should().AllBeOfType<NotSupportedException>();
            actuals.Select(_ => _.Message.Should().Be("Anonymous types are not supported.")).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_return_null___When_parameter_throwIfNoCompilableStringExists_is_false_and_parameter_type_is_an_anonymous_type()
        {
            // Arrange
            var types = new[]
            {
                new { Anonymous = true }.GetType(),
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: false))).ToList();

            // Assert
            actuals.Select(_ => _.Should().BeNull()).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_throw_NotSupportedException___When_parameter_throwIfNoCompilableStringExists_is_true_and_parameter_type_is_a_generic_open_constructed_type()
        {
            // Arrange
            var types = new[]
            {
                // IsGenericType: True
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: False
                typeof(Derived<>).BaseType,

                // IsGenericType: True
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: False
                typeof(Derived<>).GetField("F").FieldType,
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: true))).ToList();

            // Assert
            actuals.Should().AllBeOfType<NotSupportedException>();
            actuals.Select(_ => _.Message.Should().Be("Generic open constructed types are not supported.")).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_return_null___When_parameter_throwIfNoCompilableStringExists_is_false_and_parameter_type_is_a_generic_open_constructed_type()
        {
            // Arrange
            var types = new[]
            {
                // IsGenericType: True
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: False
                typeof(Derived<>).BaseType,

                // IsGenericType: True
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: False
                typeof(Derived<>).GetField("F").FieldType,
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: false))).ToList();

            // Assert
            actuals.Select(_ => _.Should().BeNull()).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_throw_NotSupportedException___When_parameter_throwIfNoCompilableStringExists_is_true_and_type_is_a_generic_parameter()
        {
            // Arrange
            var types = new[]
            {
                // IsGenericType: False
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: True
                typeof(Base<,>).GetGenericArguments()[0],
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: true))).ToList();

            // Assert
            actuals.Should().AllBeOfType<NotSupportedException>();
            actuals.Select(_ => _.Message.Should().Be("Generic parameters not supported.")).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_return_null___When_parameter_throwIfNoCompilableStringExists_is_false_and_type_is_a_generic_parameter()
        {
            // Arrange
            var types = new[]
            {
                // IsGenericType: False
                // IsGenericTypeDefinition: False
                // ContainsGenericParameters: True
                // IsGenericParameter: True
                typeof(Base<,>).GetGenericArguments()[0],
            };

            // Act
            // ReSharper disable once ConvertClosureToMethodGroup
            var actuals = types.Select(_ => Record.Exception(() => _.ToStringCompilable(throwIfNoCompilableStringExists: false))).ToList();

            // Assert
            actuals.Select(_ => _.Should().BeNull()).ToList();
        }

        [Fact]
        public static void ToStringCompilable___Should_return_compilable_string_representation_of_the_specified_type___When_called()
        {
            // Arrange
            var typesAndExpected = new[]
            {
                new { Type = typeof(Derived<>), Expected = "Derived<>" },
                new { Type = new Derived<int>[0].GetType(), Expected = "Derived<int>[]" },
                new { Type = typeof(Derived<>.Nested), Expected = "Derived<>.Nested" },
                new { Type = typeof(string), Expected = "string" },
                new { Type = typeof(int), Expected = "int" },
                new { Type = typeof(int?), Expected = "int?" },
                new { Type = typeof(Guid), Expected = "Guid" },
                new { Type = typeof(Guid?), Expected = "Guid?" },
                new { Type = typeof(MyNonNestedClass), Expected = "MyNonNestedClass" },
                new { Type = typeof(MyNestedClass), Expected = "TypeExtensionsTest.MyNestedClass" },
                new { Type = typeof(IReadOnlyDictionary<string, int?>), Expected = "IReadOnlyDictionary<string, int?>" },
                new { Type = typeof(IReadOnlyDictionary<string, Guid?>), Expected = "IReadOnlyDictionary<string, Guid?>" },
                new { Type = typeof(string[]), Expected = "string[]" },
                new { Type = typeof(int?[]), Expected = "int?[]" },
                new { Type = typeof(MyNonNestedClass[]), Expected = "MyNonNestedClass[]" },
                new { Type = typeof(Guid?[]), Expected = "Guid?[]" },
                new { Type = typeof(IList<int?[]>), Expected = "IList<int?[]>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool?>[]), Expected = "IReadOnlyDictionary<MyNonNestedClass, bool?>[]" },
                new { Type = typeof(IReadOnlyDictionary<bool[], MyNonNestedClass>), Expected = "IReadOnlyDictionary<bool[], MyNonNestedClass>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool[]>), Expected = "IReadOnlyDictionary<MyNonNestedClass, bool[]>" },
                new { Type = typeof(IList<>), Expected = "IList<>" },
                new { Type = typeof(List<>), Expected = "List<>" },
                new { Type = typeof(IReadOnlyDictionary<,>), Expected = "IReadOnlyDictionary<,>" },
                new { Type = typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>), Expected = "IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>" },
                new { Type = (first: "one", second: 10).GetType(), Expected = "ValueTuple<string, int>" },
            };

            // Act
            var actuals = typesAndExpected.Select(_ => _.Type.ToStringCompilable()).ToList();

            // Assert
            typesAndExpected.Select(_ => _.Expected).Should().Equal(actuals);
        }

        [Fact]
        public static void ToStringReadable___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => Representation.TypeExtensions.ToStringReadable(null, A.Dummy<ToStringReadableOptions>()));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("type");
        }

        [Fact]
        public static void ToStringReadable___Should_return_readable_string_representation_of_the_specified_type___When_parameter_options_is_None()
        {
            // Arrange
            var innerAnonymousObject = new { InnerAnonymous = 6 };
            var innerAnonymousTypeName = new Regex("AnonymousType\\d*").Match(innerAnonymousObject.GetType().Name);

            var anonymousObject = new { Anonymous = true, Inner = innerAnonymousObject };
            var anonymousTypeName = new Regex("AnonymousType\\d*").Match(anonymousObject.GetType().Name);

            var typesAndExpected = new[]
            {
                // value tuple:
                new { Type = (first: "one", second: 7).GetType(), Expected = "ValueTuple<string, int>" },

                // anonymous type:
                new { Type = anonymousObject.GetType(), Expected = Invariant($"{anonymousTypeName}<bool, {innerAnonymousTypeName}<int>>") },

                // anonymous type generic type definition:
                new { Type = anonymousObject.GetType().GetGenericTypeDefinition(), Expected = Invariant($"{anonymousTypeName}<T1, T2>") },

                // generic open constructed types:
                new { Type = typeof(Derived<>).BaseType, Expected = "Base<string, V>" },
                new { Type = typeof(Derived<>).GetField("F").FieldType, Expected = "G<Derived<V>>" },

                // generic parameter:
                new { Type = typeof(Base<,>).GetGenericArguments()[0], Expected = "T" },

                // generic type definitions:
                new { Type = typeof(IList<>), Expected = "IList<T>" },
                new { Type = typeof(List<>), Expected = "List<T>" },
                new { Type = typeof(IReadOnlyDictionary<,>), Expected = "IReadOnlyDictionary<TKey, TValue>" },
                new { Type = typeof(Derived<>), Expected = "Derived<V>" },

                // other types
                new { Type = new Derived<int>[0].GetType(), Expected = "Derived<int>[]" },
                new { Type = typeof(Derived<>.Nested), Expected = "Derived<V>.Nested" },
                new { Type = typeof(string), Expected = "string" },
                new { Type = typeof(int), Expected = "int" },
                new { Type = typeof(int?), Expected = "int?" },
                new { Type = typeof(Guid), Expected = "Guid" },
                new { Type = typeof(Guid?), Expected = "Guid?" },
                new { Type = typeof(MyNonNestedClass), Expected = "MyNonNestedClass" },
                new { Type = typeof(MyNestedClass), Expected = "TypeExtensionsTest.MyNestedClass" },
                new { Type = typeof(IReadOnlyDictionary<string, int?>), Expected = "IReadOnlyDictionary<string, int?>" },
                new { Type = typeof(IReadOnlyDictionary<string, Guid?>), Expected = "IReadOnlyDictionary<string, Guid?>" },
                new { Type = typeof(string[]), Expected = "string[]" },
                new { Type = typeof(int?[]), Expected = "int?[]" },
                new { Type = typeof(MyNonNestedClass[]), Expected = "MyNonNestedClass[]" },
                new { Type = typeof(Guid?[]), Expected = "Guid?[]" },
                new { Type = typeof(IList<int?[]>), Expected = "IList<int?[]>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool?>[]), Expected = "IReadOnlyDictionary<MyNonNestedClass, bool?>[]" },
                new { Type = typeof(IReadOnlyDictionary<bool[], MyNonNestedClass>), Expected = "IReadOnlyDictionary<bool[], MyNonNestedClass>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool[]>), Expected = "IReadOnlyDictionary<MyNonNestedClass, bool[]>" },
                new { Type = typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>), Expected = "IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>" },
            };

            // Act
            var actuals = typesAndExpected.Select(_ => _.Type.ToStringReadable(ToStringReadableOptions.None)).ToList();

            // Assert
            typesAndExpected.Select(_ => _.Expected).Should().Equal(actuals);
        }

        [Fact]
        public static void ToStringReadable___Should_return_readable_string_representation_of_the_specified_type_with_namespaces_included___When_parameter_options_is_IncludeNamespace()
        {
            // Arrange
            var innerAnonymousObject = new { InnerAnonymous = 6 };
            var innerAnonymousTypeName = new Regex("AnonymousType\\d*").Match(innerAnonymousObject.GetType().Name);

            var anonymousObject = new { Anonymous = true, Inner = innerAnonymousObject };
            var anonymousTypeName = new Regex("AnonymousType\\d*").Match(anonymousObject.GetType().Name);

            var typesAndExpected = new[]
            {
                // value tuple:
                new { Type = (first: "one", second: 7).GetType(), Expected = "System.ValueTuple<string, int>" },

                // anonymous type:
                new { Type = anonymousObject.GetType(), Expected = anonymousTypeName + "<bool, " + innerAnonymousTypeName + "<int>>" },

                // anonymous type generic type definition:
                new { Type = anonymousObject.GetType().GetGenericTypeDefinition(), Expected = Invariant($"{anonymousTypeName}<T1, T2>") },

                // generic open constructed types:
                new { Type = typeof(Derived<>).BaseType, Expected = "OBeautifulCode.Representation.Test.Base<string, V>" },
                new { Type = typeof(Derived<>).GetField("F").FieldType, Expected = "OBeautifulCode.Representation.Test.G<OBeautifulCode.Representation.Test.Derived<V>>" },

                // generic parameter:
                new { Type = typeof(Base<,>).GetGenericArguments()[0], Expected = "T" },

                // generic type definitions:
                new { Type = typeof(IList<>), Expected = "System.Collections.Generic.IList<T>" },
                new { Type = typeof(List<>), Expected = "System.Collections.Generic.List<T>" },
                new { Type = typeof(IReadOnlyDictionary<,>), Expected = "System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>" },
                new { Type = typeof(Derived<>), Expected = "OBeautifulCode.Representation.Test.Derived<V>" },

                // other types
                new { Type = new Derived<int>[0].GetType(), Expected = "OBeautifulCode.Representation.Test.Derived<int>[]" },
                new { Type = typeof(Derived<>.Nested), Expected = "OBeautifulCode.Representation.Test.Derived<V>.Nested" },
                new { Type = typeof(string), Expected = "string" },
                new { Type = typeof(int), Expected = "int" },
                new { Type = typeof(int?), Expected = "int?" },
                new { Type = typeof(Guid), Expected = "System.Guid" },
                new { Type = typeof(Guid?), Expected = "System.Guid?" },
                new { Type = typeof(MyNonNestedClass), Expected = "OBeautifulCode.Representation.Test.MyNonNestedClass" },
                new { Type = typeof(MyNestedClass), Expected = "OBeautifulCode.Representation.Test.TypeExtensionsTest.MyNestedClass" },
                new { Type = typeof(IReadOnlyDictionary<string, int?>), Expected = "System.Collections.Generic.IReadOnlyDictionary<string, int?>" },
                new { Type = typeof(IReadOnlyDictionary<string, Guid?>), Expected = "System.Collections.Generic.IReadOnlyDictionary<string, System.Guid?>" },
                new { Type = typeof(string[]), Expected = "string[]" },
                new { Type = typeof(int?[]), Expected = "int?[]" },
                new { Type = typeof(MyNonNestedClass[]), Expected = "OBeautifulCode.Representation.Test.MyNonNestedClass[]" },
                new { Type = typeof(Guid?[]), Expected = "System.Guid?[]" },
                new { Type = typeof(IList<int?[]>), Expected = "System.Collections.Generic.IList<int?[]>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool?>[]), Expected = "System.Collections.Generic.IReadOnlyDictionary<OBeautifulCode.Representation.Test.MyNonNestedClass, bool?>[]" },
                new { Type = typeof(IReadOnlyDictionary<bool[], MyNonNestedClass>), Expected = "System.Collections.Generic.IReadOnlyDictionary<bool[], OBeautifulCode.Representation.Test.MyNonNestedClass>" },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool[]>), Expected = "System.Collections.Generic.IReadOnlyDictionary<OBeautifulCode.Representation.Test.MyNonNestedClass, bool[]>" },
                new { Type = typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>), Expected = "System.Collections.Generic.IReadOnlyDictionary<System.Collections.Generic.IReadOnlyDictionary<System.Guid[], int?>, System.Collections.Generic.IList<System.Collections.Generic.IList<short>>[]>" },
            };

            // Act
            var actuals = typesAndExpected.Select(_ => _.Type.ToStringReadable(ToStringReadableOptions.IncludeNamespace)).ToList();

            // Assert
            typesAndExpected.Select(_ => _.Expected).Should().Equal(actuals);
        }

        [Fact]
        public static void ToStringReadable___Should_return_readable_string_representation_of_the_specified_type_with_assembly_details_included___When_parameter_options_is_IncludeAssemblyDetails()
        {
            // Arrange
            var innerAnonymousObject = new { InnerAnonymous = 6 };
            var innerAnonymousTypeName = new Regex("AnonymousType\\d*").Match(innerAnonymousObject.GetType().Name);

            var anonymousObject = new { Anonymous = true, Inner = innerAnonymousObject };
            var anonymousTypeName = new Regex("AnonymousType\\d*").Match(anonymousObject.GetType().Name);

            var typesAndExpected = new[]
            {
                // value tuple:
                new { Type = (first: "one", second: 7).GetType(), Expected = Invariant($"ValueTuple<string, int> || System.ValueTuple<T1, T2> => {MsCorLibNameAndVersion} | string => {MsCorLibNameAndVersion} | int => {MsCorLibNameAndVersion}") },

                // anonymous type:
                new { Type = anonymousObject.GetType(), Expected = Invariant($"{anonymousTypeName}<bool, {innerAnonymousTypeName}<int>> || {anonymousTypeName}<T1, T2> => {ThisAssemblyNameAndVersion} | bool => {MsCorLibNameAndVersion} | {innerAnonymousTypeName}<T1> => {ThisAssemblyNameAndVersion} | int => {MsCorLibNameAndVersion}") },

                // anonymous type generic type definition:
                new { Type = anonymousObject.GetType().GetGenericTypeDefinition(), Expected = Invariant($"{anonymousTypeName}<T1, T2> || {anonymousTypeName}<T1, T2> => {ThisAssemblyNameAndVersion}") },

                // generic open constructed types:
                new { Type = typeof(Derived<>).BaseType, Expected = Invariant($"Base<string, V> || OBeautifulCode.Representation.Test.Base<T, U> => {ThisAssemblyNameAndVersion} | string => {MsCorLibNameAndVersion}") },
                new { Type = typeof(Derived<>).GetField("F").FieldType, Expected = Invariant($"G<Derived<V>> || OBeautifulCode.Representation.Test.G<T> => {ThisAssemblyNameAndVersion} | OBeautifulCode.Representation.Test.Derived<V> => {ThisAssemblyNameAndVersion}") },

                // generic parameter:
                new { Type = typeof(Base<,>).GetGenericArguments()[0], Expected = "T" },

                // generic type definitions:
                new { Type = typeof(IList<>), Expected = Invariant($"IList<T> || System.Collections.Generic.IList<T> => {MsCorLibNameAndVersion}") },
                new { Type = typeof(List<>), Expected = Invariant($"List<T> || System.Collections.Generic.List<T> => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<,>), Expected = Invariant($"IReadOnlyDictionary<TKey, TValue> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion}") },
                new { Type = typeof(Derived<>), Expected = Invariant($"Derived<V> || OBeautifulCode.Representation.Test.Derived<V> => {ThisAssemblyNameAndVersion}") },

                // other types
                new { Type = new Derived<int>[0].GetType(), Expected = Invariant($"Derived<int>[] || OBeautifulCode.Representation.Test.Derived<V> => {ThisAssemblyNameAndVersion} | int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(Derived<>.Nested), Expected = Invariant($"Derived<V>.Nested || OBeautifulCode.Representation.Test.Derived<V>.Nested => {ThisAssemblyNameAndVersion}") },
                new { Type = typeof(string), Expected = Invariant($"string || string => {MsCorLibNameAndVersion}") },
                new { Type = typeof(int), Expected = Invariant($"int || int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(int?), Expected = Invariant($"int? || int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(Guid), Expected = Invariant($"Guid || System.Guid => {MsCorLibNameAndVersion}") },
                new { Type = typeof(Guid?), Expected = Invariant($"Guid? || System.Guid => {MsCorLibNameAndVersion}") },
                new { Type = typeof(MyNonNestedClass), Expected = Invariant($"MyNonNestedClass || OBeautifulCode.Representation.Test.MyNonNestedClass => {ThisAssemblyNameAndVersion}") },
                new { Type = typeof(MyNestedClass), Expected = Invariant($"TypeExtensionsTest.MyNestedClass || OBeautifulCode.Representation.Test.TypeExtensionsTest.MyNestedClass => {ThisAssemblyNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<string, int?>), Expected = Invariant($"IReadOnlyDictionary<string, int?> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | string => {MsCorLibNameAndVersion} | int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<string, Guid?>), Expected = Invariant($"IReadOnlyDictionary<string, Guid?> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | string => {MsCorLibNameAndVersion} | System.Guid => {MsCorLibNameAndVersion}") },
                new { Type = typeof(string[]), Expected = Invariant($"string[] || string => {MsCorLibNameAndVersion}") },
                new { Type = typeof(int?[]), Expected = Invariant($"int?[] || int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(MyNonNestedClass[]), Expected = Invariant($"MyNonNestedClass[] || OBeautifulCode.Representation.Test.MyNonNestedClass => {ThisAssemblyNameAndVersion}") },
                new { Type = typeof(Guid?[]), Expected = Invariant($"Guid?[] || System.Guid => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IList<int?[]>), Expected = Invariant($"IList<int?[]> || System.Collections.Generic.IList<T> => {MsCorLibNameAndVersion} | int => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool?>[]), Expected = Invariant($"IReadOnlyDictionary<MyNonNestedClass, bool?>[] || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | OBeautifulCode.Representation.Test.MyNonNestedClass => {ThisAssemblyNameAndVersion} | bool => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<bool[], MyNonNestedClass>), Expected = Invariant($"IReadOnlyDictionary<bool[], MyNonNestedClass> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | bool => {MsCorLibNameAndVersion} | OBeautifulCode.Representation.Test.MyNonNestedClass => {ThisAssemblyNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<MyNonNestedClass, bool[]>), Expected = Invariant($"IReadOnlyDictionary<MyNonNestedClass, bool[]> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | OBeautifulCode.Representation.Test.MyNonNestedClass => {ThisAssemblyNameAndVersion} | bool => {MsCorLibNameAndVersion}") },
                new { Type = typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>), Expected = Invariant($"IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]> || System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> => {MsCorLibNameAndVersion} | System.Guid => {MsCorLibNameAndVersion} | int => {MsCorLibNameAndVersion} | System.Collections.Generic.IList<T> => {MsCorLibNameAndVersion} | System.Collections.Generic.IList<T> => {MsCorLibNameAndVersion} | short => {MsCorLibNameAndVersion}") },
            };

            // Act
            var actuals = typesAndExpected.Select(_ => _.Type.ToStringReadable(ToStringReadableOptions.IncludeAssemblyDetails)).ToList();

            // Assert
            typesAndExpected.Select(_ => _.Expected).Should().Equal(actuals);
        }

        private class MyNestedClass
        {
        }
    }

    public class MyNonNestedClass
    {
    }

#pragma warning disable SA1314 // Type parameter names should begin with T
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "U", Justification = "For testing purposes.")]
    [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "T", Justification = "For testing purposes.")]
    public class Base<T, U>
#pragma warning restore SA1314 // Type parameter names should begin with T
    {
    }

#pragma warning disable SA1314 // Type parameter names should begin with T
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "V", Justification = "For testing purposes.")]
    [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "T", Justification = "For testing purposes.")]
    public class Derived<V> : Base<string, V>
#pragma warning restore SA1314 // Type parameter names should begin with T
    {
#pragma warning disable SA1401 // Fields should be private
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "F", Justification = "For testing purposes.")]
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "For testing purposes.")]
        public G<Derived<V>> F;
#pragma warning restore SA1401 // Fields should be private

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "For testing purposes.")]
        public class Nested
        {
        }
    }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "G", Justification = "For testing purposes.")]
    public class G<T>
    {
    }
}
