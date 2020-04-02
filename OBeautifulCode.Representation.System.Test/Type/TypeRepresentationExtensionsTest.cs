// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public static class TypeRepresentationExtensionsTest
    {
        [Fact]
        public static void ToTypeRepresentation___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => TypeRepresentationExtensions.ToRepresentation(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("type");
        }

        [Fact]
        public static void ToTypeRepresentation___Should_throw_ArgumentException___When_parameter_type_is_an_open_generic_type()
        {
            // Arrange
            var types = new[]
            {
                typeof(List<>),
                typeof(List<>).MakeArrayType(),
                typeof(List<>).MakeGenericType(typeof(List<>)),
                typeof(IReadOnlyCollection<>).MakeGenericType(typeof(IReadOnlyCollection<>)),
                typeof(Dictionary<,>).GetGenericArguments()[0],
            };

            // Act
            var actuals = types.Select(_ => Record.Exception(_.ToRepresentation)).ToList();

            // Assert
            actuals.AsTest().Must().Each().BeOfType<ArgumentException>();
            actuals.Select(_ => _.Message).AsTest().Must().Each().ContainString("ContainsGenericParameters");
        }

        [Fact]
        public static void ToTypeRepresentation___Should_return_type_description___When_called()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.AssemblyQualifiedName.AsTest().Must().BeEqualTo(type.AssemblyQualifiedName);
            description.Namespace.AsTest().Must().BeEqualTo(type.Namespace);
            description.Name.AsTest().Must().BeEqualTo(type.Name);
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_throw_ArgumentNullException___When_parameter_typeRepresentation_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => TypeRepresentationExtensions.ResolveFromLoadedTypes(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("typeRepresentation");
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_roundtrip_a_type_from_its_representation___When_called()
        {
            // Arrange
            var expected = new[]
            {
                // objects, structs, nested class
                typeof(int),
                typeof(string),
                typeof(Guid),
                typeof(DateTime),
                typeof(object),
                typeof(ConstructorInfoRepresentation),
                typeof(TestClass.NestedTestClass),

                // array of objects, structs, nested class
                typeof(int[]),
                typeof(string[]),
                typeof(Guid[]),
                typeof(DateTime[]),
                typeof(object[]),
                typeof(ConstructorInfoRepresentation[]),
                typeof(TestClass.NestedTestClass[]),

                // nullable
                typeof(int?),
                typeof(Guid?),
                typeof(DateTime?),

                // array of nullable
                typeof(int?[]),
                typeof(Guid?[]),
                typeof(DateTime?[]),

                // non-generic interface
                typeof(global::System.Collections.IEnumerable),

                // generic interface
                typeof(IList<int>),
                typeof(IList<string>),
                typeof(IList<Guid>),
                typeof(IList<DateTime>),
                typeof(IList<object>),
                typeof(IList<ConstructorInfoRepresentation>),
                typeof(IList<TestClass.NestedTestClass>),

                // generic interface of array
                typeof(IList<int[]>),
                typeof(IList<string[]>),
                typeof(IList<Guid[]>),
                typeof(IList<DateTime[]>),
                typeof(IList<object[]>),
                typeof(IList<ConstructorInfoRepresentation[]>),
                typeof(IList<TestClass.NestedTestClass[]>),

                // generic interface of array of nullable
                typeof(IList<int?[]>),
                typeof(IList<Guid?[]>),
                typeof(IList<DateTime?[]>),

                // array of generic interface
                typeof(IList<int>[]),
                typeof(IList<string>[]),
                typeof(IList<Guid>[]),
                typeof(IList<DateTime>[]),
                typeof(IList<object>[]),
                typeof(IList<ConstructorInfoRepresentation>[]),
                typeof(IList<TestClass.NestedTestClass>[]),

                // jagged arrays
                typeof(int[][]),
                typeof(string[][]),
                typeof(Guid[][]),
                typeof(DateTime[][]),
                typeof(object[][]),
                typeof(ConstructorInfoRepresentation[][]),
                typeof(TestClass.NestedTestClass[][]),

                // jagged arrays of nullable
                typeof(int?[][]),
                typeof(Guid?[][]),
                typeof(DateTime?[][]),

                // multi-level generics
                typeof(IReadOnlyDictionary<Guid?, IReadOnlyDictionary<ConstructorInfoRepresentation, DateTime>>),
                typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>),
                typeof(IReadOnlyDictionary<IReadOnlyDictionary<ConstructorInfoRepresentation[], int?>, IList<IList<TestClass.NestedTestClass>>[]>[]),

                // 1-dimension multi-dimensional arrays
                typeof(int).MakeArrayType(1),
                typeof(string).MakeArrayType(1),
                typeof(Guid).MakeArrayType(1),
                typeof(DateTime).MakeArrayType(1),
                typeof(object).MakeArrayType(1),
                typeof(ConstructorInfoRepresentation).MakeArrayType(1),
                typeof(TestClass.NestedTestClass).MakeArrayType(1),
                typeof(int?).MakeArrayType(1),
                typeof(Guid?).MakeArrayType(1),
                typeof(DateTime?).MakeArrayType(1),
                typeof(IList<int>).MakeArrayType(1),
                typeof(IList<string>).MakeArrayType(1),
                typeof(IList<Guid>).MakeArrayType(1),
                typeof(IList<DateTime>).MakeArrayType(1),
                typeof(IList<object>).MakeArrayType(1),
                typeof(IList<ConstructorInfoRepresentation>).MakeArrayType(1),
                typeof(IList<TestClass.NestedTestClass>).MakeArrayType(1),

                // 2 or more dimension multi-dimesional arrays
                typeof(int[,]),
                typeof(string[,]),
                typeof(Guid[,]),
                typeof(DateTime[,]),
                typeof(object[,]),
                typeof(ConstructorInfoRepresentation[,]),
                typeof(TestClass.NestedTestClass[,]),
                typeof(int?[,]),
                typeof(Guid?[,]),
                typeof(DateTime?[,]),
                typeof(IList<int>[,]),
                typeof(IList<string>[,]),
                typeof(IList<Guid>[,]),
                typeof(IList<DateTime>[,]),
                typeof(IList<object>[,]),
                typeof(IList<ConstructorInfoRepresentation>[,]),
                typeof(IList<TestClass.NestedTestClass>[,]),

                // array crazyness
                typeof(object[][,]),
                typeof(string[][,]),
                typeof(Guid[][,]),
                typeof(DateTime[][,]),
                typeof(int[][,]),
                typeof(Guid?[][,]),
                typeof(DateTime?[][,]),
                typeof(int?[][,]),
                typeof(IReadOnlyCollection<object>[][,]),
                typeof(IReadOnlyCollection<string>[][,]),
                typeof(IReadOnlyCollection<Guid>[][,]),
                typeof(IReadOnlyCollection<DateTime>[][,]),
                typeof(IReadOnlyCollection<int>[][,]),
                typeof(List<object>[][,]),
                typeof(List<string>[][,]),
                typeof(List<Guid>[][,]),
                typeof(List<DateTime>[][,]),
                typeof(List<int>[][,]),
                typeof(object[,][]),
                typeof(string[,][]),
                typeof(Guid[,][]),
                typeof(DateTime[,][]),
                typeof(int[,][]),
                typeof(Guid?[,][]),
                typeof(DateTime?[,][]),
                typeof(int?[,][]),
                typeof(IReadOnlyCollection<object>[,][]),
                typeof(IReadOnlyCollection<string>[,][]),
                typeof(IReadOnlyCollection<Guid>[,][]),
                typeof(IReadOnlyCollection<DateTime>[,][]),
                typeof(IReadOnlyCollection<int>[,][]),
                typeof(List<object>[,][]),
                typeof(List<string>[,][]),
                typeof(List<Guid>[,][]),
                typeof(List<DateTime>[,][]),
                typeof(List<int>[,][]),
                typeof(object[]).MakeArrayType(1),
                typeof(string[]).MakeArrayType(1),
                typeof(Guid[]).MakeArrayType(1),
                typeof(DateTime[]).MakeArrayType(1),
                typeof(int[]).MakeArrayType(1),
                typeof(Guid?[]).MakeArrayType(1),
                typeof(DateTime?[]).MakeArrayType(1),
                typeof(int?[]).MakeArrayType(1),
                typeof(IReadOnlyCollection<object>[]).MakeArrayType(1),
                typeof(IReadOnlyCollection<string>[]).MakeArrayType(1),
                typeof(IReadOnlyCollection<Guid>[]).MakeArrayType(1),
                typeof(IReadOnlyCollection<DateTime>[]).MakeArrayType(1),
                typeof(IReadOnlyCollection<int>[]).MakeArrayType(1),
                typeof(List<object>[]).MakeArrayType(1),
                typeof(List<string>[]).MakeArrayType(1),
                typeof(List<Guid>[]).MakeArrayType(1),
                typeof(List<DateTime>[]).MakeArrayType(1),
                typeof(List<int>[]).MakeArrayType(1),
            };

            var representations = expected.Select(_ => _.ToRepresentation()).ToArray();

            // Act
            var actual = representations.Select(_ => _.ResolveFromLoadedTypes()).ToArray();

            // Assert
            actual.Must().BeEqualTo(expected);
        }

        private class TestClass
        {
            public class NestedTestClass
            {
            }
        }
    }
}
