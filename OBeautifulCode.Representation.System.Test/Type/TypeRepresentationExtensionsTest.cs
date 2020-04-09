// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Enum.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    using Xunit;

    public static class TypeRepresentationExtensionsTest
    {
        static TypeRepresentationExtensionsTest()
        {
            byte[] assemblyBytes;

            using (var stream = AssemblyHelper.OpenEmbeddedResourceStream("OBeautifulCode.Representation.System.Test.Type.Conditions.dll", addCallerNamespace: false))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    assemblyBytes = ms.ToArray();
                }
            }

            // Conditions is already loaded because it is included in the project
            // and we use a type in that assembly in unit tests below.
            // Here we load an older version of Conditions so that two versions of
            // the same assembly are loaded
            AppDomain.CurrentDomain.Load(assemblyBytes);
        }

        [Fact]
        public static void ToRepresentation___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((Type)null).ToRepresentation());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("type");
        }

        [Fact]
        public static void ToRepresentation___Should_throw_ArgumentException___When_parameter_type_is_an_open_generic_type()
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
        public static void ToRepresentation___Should_return_expected_representation_of_type___When_type_is_string()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.Namespace.AsTest().Must().BeEqualTo("System");
            description.Name.AsTest().Must().BeEqualTo("String");
            description.AssemblyName.AsTest().Must().BeEqualTo("mscorlib");
            description.AssemblyVersion.AsTest().Must().BeEqualTo("4.0.0.0");
            description.GenericArguments.AsTest().Must().BeEmptyEnumerable();
        }

        [Fact]
        public static void ToRepresentation___Should_return_expected_representation_of_type___When_type_is_generic()
        {
            // Arrange
            var type = typeof(IReadOnlyDictionary<string, int>);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.Namespace.AsTest().Must().BeEqualTo("System.Collections.Generic");
            description.Name.AsTest().Must().BeEqualTo("IReadOnlyDictionary`2");
            description.AssemblyName.AsTest().Must().BeEqualTo("mscorlib");
            description.AssemblyVersion.AsTest().Must().BeEqualTo("4.0.0.0");
            description.GenericArguments.AsTest().Must().HaveCount(2);
            description.GenericArguments.First().Must().BeEqualTo(typeof(string).ToRepresentation());
            description.GenericArguments.Last().Must().BeEqualTo(typeof(int).ToRepresentation());
        }

        [Fact]
        public static void ToRepresentation___Should_return_expected_representation_of_type___When_type_is_an_array()
        {
            // Arrange
            var type = typeof(int?[]);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.Namespace.AsTest().Must().BeEqualTo("System");
            description.Name.AsTest().Must().BeEqualTo("Nullable`1[]");
            description.AssemblyName.AsTest().Must().BeEqualTo("mscorlib");
            description.AssemblyVersion.AsTest().Must().BeEqualTo("4.0.0.0");
            description.GenericArguments.AsTest().Must().HaveCount(1);
            description.GenericArguments.Single().Must().BeEqualTo(typeof(int).ToRepresentation());
        }

        [Fact]
        public static void ToRepresentation___Should_return_expected_representation_of_type___When_type_is_a_jagged_array()
        {
            // Arrange
            var type = typeof(int?[][][]);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.Namespace.AsTest().Must().BeEqualTo("System");
            description.Name.AsTest().Must().BeEqualTo("Nullable`1[][][]");
            description.AssemblyName.AsTest().Must().BeEqualTo("mscorlib");
            description.AssemblyVersion.AsTest().Must().BeEqualTo("4.0.0.0");
            description.GenericArguments.AsTest().Must().HaveCount(1);
            description.GenericArguments.Single().Must().BeEqualTo(typeof(int).ToRepresentation());
        }

        [Fact]
        public static void ToRepresentation___Should_return_expected_representation_of_type___When_type_is_nested()
        {
            // Arrange
            var type = typeof(TypeGenerator.TestClassInStaticClass.NestedClassInTestClassInStaticClass.NestedClassInNestedClassInTestClassInStaticClass[]);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.Namespace.AsTest().Must().BeEqualTo("OBeautifulCode.Representation.System.Test");
            description.Name.AsTest().Must().BeEqualTo("TypeGenerator+TestClassInStaticClass+NestedClassInTestClassInStaticClass+NestedClassInNestedClassInTestClassInStaticClass[]");
            description.AssemblyName.AsTest().Must().BeEqualTo("OBeautifulCode.Representation.System.Test");
            description.AssemblyVersion.AsTest().Must().BeEqualTo(typeof(TypeGenerator).Assembly.GetName().Version.ToString());
            description.GenericArguments.AsTest().Must().BeEmptyEnumerable();
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
        public static void ResolvedFromLoadedTypes___Should_throw_NotSupportedException___When_parameter_assemblyMatchStrategy_is_not_AnySingleVersion()
        {
            // Arrange
            var assemblyMatchStrategy = EnumExtensions.GetDefinedEnumValues<AssemblyMatchStrategy>().Where(_ => _ != AssemblyMatchStrategy.AnySingleVersion).ToList();

            var typeRepresentation = A.Dummy<TypeRepresentation>();

            // Act
            var actuals1 = assemblyMatchStrategy.Select(_ => Record.Exception(() => typeRepresentation.ResolveFromLoadedTypes(_, throwIfCannotResolve: false)));
            var actuals2 = assemblyMatchStrategy.Select(_ => Record.Exception(() => typeRepresentation.ResolveFromLoadedTypes(_, throwIfCannotResolve: true)));

            // Assert
            actuals1.AsTest().Must().Each().BeOfType<NotSupportedException>();
            actuals2.AsTest().Must().Each().BeOfType<NotSupportedException>();
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_roundtrip_a_type_from_its_representation___When_called()
        {
            // Arrange
            var expectedTypes = TypeGenerator.GenerateTypesForTesting().ToList();

            var representations = expectedTypes.Select(_ => _.ToRepresentation()).ToList();

            // Act
            var actualTypes = representations.Select(_ => _.ResolveFromLoadedTypes(throwIfCannotResolve: true)).ToList();

            // Assert
            actualTypes.AsTest().Must().BeEqualTo(expectedTypes);
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_return_null___When_assembly_is_not_loaded_and_throwIfCannotResolve_is_false()
        {
            // Arrange
            var representation1 = A.Dummy<TypeRepresentation>().DeepCloneWithAssemblyName(A.Dummy<string>());

            var representation2 = A.Dummy<TypeRepresentation>().Whose(_ => _.GenericArguments.Any());

            var modifiedGenericArgument = representation2.GenericArguments.First().DeepCloneWithAssemblyName(A.Dummy<string>());

            var modifiedGenericArguments = new TypeRepresentation[0].Concat(new[] { modifiedGenericArgument }).Concat(representation2.GenericArguments.Skip(1)).ToList();

            representation2 = representation2.DeepCloneWithGenericArguments(modifiedGenericArguments);

            // Act
            var actual1 = representation1.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: false);
            var actual2 = representation2.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: false);

            // Assert
            actual1.AsTest().Must().BeNull();
            actual2.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_throw_InvalidOperationException___When_assembly_is_not_loaded_and_throwIfCannotResolve_is_true()
        {
            // Arrange
            var dummyAssembly = A.Dummy<string>();

            var representation1 = A.Dummy<TypeRepresentation>().DeepCloneWithAssemblyName(dummyAssembly);

            var representation2 = A.Dummy<TypeRepresentation>().Whose(_ => _.GenericArguments.Any());

            var modifiedGenericArgument = representation2.GenericArguments.First().DeepCloneWithAssemblyName(dummyAssembly);

            var modifiedGenericArguments = new TypeRepresentation[0].Concat(new[] { modifiedGenericArgument }).Concat(representation2.GenericArguments.Skip(1)).ToList();

            representation2 = representation2.DeepCloneWithGenericArguments(modifiedGenericArguments);

            // Act
            var actual1 = Record.Exception(() => representation1.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: true));
            var actual2 = Record.Exception(() => representation2.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: true));

            // Assert
            actual1.AsTest().Must().BeOfType<InvalidOperationException>();
            actual1.Message.AsTest().Must().ContainString("Unable to resolve the specified TypeRepresentation");
            actual1.Message.AsTest().Must().ContainString("These assemblies are not loaded: " + dummyAssembly);

            actual2.AsTest().Must().BeOfType<InvalidOperationException>();
            actual2.Message.AsTest().Must().ContainString("Unable to resolve the specified TypeRepresentation");
            actual2.Message.AsTest().Must().ContainString("These assemblies are not loaded: " + dummyAssembly);
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_return_null___When_multiple_versions_of_assembly_are_loaded_and_throwIfCannotResolve_is_false()
        {
            // Arrange
            var type = typeof(Conditions.Condition);

            var representation = type.ToRepresentation();

            // Act
            var actual = representation.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_throw_InvalidOperationException___When_multiple_versions_of_assembly_are_loaded_and_throwIfCannotResolve_is_true()
        {
            // Arrange
            var type = typeof(Conditions.Condition);

            var representation = type.ToRepresentation();

            // Act
            var actual = Record.Exception(() => representation.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: true));

            // Assert
            actual.AsTest().Must().BeOfType<InvalidOperationException>();
            actual.Message.AsTest().Must().BeEqualTo("Unable to resolve the specified TypeRepresentation (Conditions.Condition, Conditions, Version=2.1.0.24) with AssemblyMatchStrategy.AnySingleVersion.  There were multiple versions of the following assemblies loaded: [Conditions, Version=2.0.1.19, Culture=neutral, PublicKeyToken=null], [Conditions, Version=2.1.0.24, Culture=neutral, PublicKeyToken=null].");
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_throw_ArgumentNullException___When_parameter_assemblyQualifiedName_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((string)null).ToTypeRepresentationFromAssemblyQualifiedName());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("assemblyQualifiedName");
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_throw_ArgumentException___When_parameter_assemblyQualifiedName_is_white_space()
        {
            // Arrange, Act
            var actual = Record.Exception(() => "  \r\n ".ToTypeRepresentationFromAssemblyQualifiedName());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentException>();
            actual.Message.AsTest().Must().ContainString("assemblyQualifiedName");
            actual.Message.AsTest().Must().ContainString("white space");
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_roundtrip_a_TypeRepresentation___When_assembly_qualified_name_generated_using_TypeRepresentation_BuildAssemblyQualifiedName_with_includeVersion_true()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var expected = types.Select(_ => _.ToRepresentation()).ToList();

            var assemblyQualifiedNames = expected.Select(_ => _.BuildAssemblyQualifiedName(includeVersion: true)).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_roundtrip_a_TypeRepresentation___When_assembly_qualified_name_generated_using_TypeRepresentation_BuildAssemblyQualifiedName_with_includeVersion_false()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var versionedRepresentation = types.Select(_ => _.ToRepresentation()).ToList();

            var assemblyQualifiedNames = versionedRepresentation.Select(_ => _.BuildAssemblyQualifiedName(includeVersion: false)).ToList();

            var expected = versionedRepresentation.Select(RemoveVersion).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        private static TypeRepresentation RemoveVersion(
            TypeRepresentation representation)
        {
            var result = representation.DeepCloneWithAssemblyVersion(null);

            result = result.DeepCloneWithGenericArguments(result.GenericArguments.Select(RemoveVersion).ToList());

            return result;
        }
    }
}
