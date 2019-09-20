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

    using FluentAssertions;

    using OBeautifulCode.Representation.Recipes;

    using Xunit;

    public static class TypeExtensionsTest
    {
        [Fact]
        public static void ToStringCompilable___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => Recipes.TypeExtensions.ToStringCompilable(null));

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
            };

            // Act
            var actuals = typesAndExpected.Select(_ => _.Type.ToStringCompilable()).ToList();

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
