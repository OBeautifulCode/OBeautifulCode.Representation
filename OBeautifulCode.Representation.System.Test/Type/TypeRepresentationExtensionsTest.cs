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
            TypeGenerator.LoadOlderVersionOfConditions();

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
            TypeGenerator.LoadOlderVersionOfConditions();

            var type = typeof(Conditions.Condition);

            var representation = type.ToRepresentation();

            // Act
            var actual = Record.Exception(() => representation.ResolveFromLoadedTypes(AssemblyMatchStrategy.AnySingleVersion, throwIfCannotResolve: true));

            // Assert
            actual.AsTest().Must().BeOfType<InvalidOperationException>();
            actual.Message.AsTest().Must().ContainString("Unable to resolve the specified TypeRepresentation (Conditions.Condition, Conditions, Version=2.1.0.24) with AssemblyMatchStrategy.AnySingleVersion.  There were multiple versions of the following assemblies loaded: [Conditions,");
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
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_roundtrip_a_TypeRepresentation___When_assembly_qualified_name_generated_using_versioned_TypeRepresentation_BuildAssemblyQualifiedName()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var expected = types.Select(_ => _.ToRepresentation()).ToList();

            var assemblyQualifiedNames = expected.Select(_ => _.BuildAssemblyQualifiedName()).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_roundtrip_a_TypeRepresentation___When_assembly_qualified_name_generated_using_unversioned_TypeRepresentation_BuildAssemblyQualifiedName()
        {
            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var expected = types.Select(_ => _.ToRepresentation().RemoveAssemblyVersions()).ToList();

            var assemblyQualifiedNames = expected.Select(_ => _.BuildAssemblyQualifiedName()).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_return_same_result_as_ToRepresentation___When_assembly_qualified_name_generated_using_Type_AssemblyQualifiedName()
        {
            // versioned

            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var expected = types.Select(_ => _.ToRepresentation()).ToList();

            var assemblyQualifiedNames = types.Select(_ => _.AssemblyQualifiedName).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToTypeRepresentationFromAssemblyQualifiedName___Should_return_same_result_as_ToRepresentation___When_assembly_qualified_name_generated_using_old_OBC_serialization_inherited_type_concrete_type_logic()
        {
            // versioned

            // Arrange
            var types = TypeGenerator.GenerateTypesForTesting().ToList();

            var expected = types.Select(_ => _.ToRepresentation().DeepCloneWithAssemblyVersion(null)).ToList();

            var assemblyQualifiedNames = types.Select(_ => _.FullName + ", " + _.Assembly.GetName().Name).ToList();

            // Act
            var actual = assemblyQualifiedNames.Select(_ => _.ToTypeRepresentationFromAssemblyQualifiedName()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void RemoveAssemblyVersions___Should_throw_ArgumentNullException___When_parameter_representation_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => TypeRepresentationExtensions.RemoveAssemblyVersions(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("representation");
        }

        [Fact]
        public static void RemoveAssemblyVersions___Should_remove_assembly_version_for_all_contained_types___When_called()
        {
            // Arrange
            var stringType = typeof(string);

            var intType = typeof(int);

            var collectionType = typeof(IReadOnlyCollection<string>[,]);

            var dictionaryType = typeof(Dictionary<string, int>);

            var collectionOfCollectionType = typeof(IReadOnlyCollection<Dictionary<string, int>>[,]);

            var representation1 = stringType.ToRepresentation();

            var representation2 = collectionType.ToRepresentation();

            var representation3 = collectionOfCollectionType.ToRepresentation();

            var expected1 = new TypeRepresentation(stringType.Namespace, stringType.Name, stringType.Assembly.GetName().Name, null, new TypeRepresentation[0]);

            var expected2 = new TypeRepresentation(collectionType.Namespace, collectionType.Name, collectionType.Assembly.GetName().Name, null, new[] { expected1 });

            var expected3 = new TypeRepresentation(collectionType.Namespace, collectionType.Name, collectionType.Assembly.GetName().Name, null, new[]
            {
                new TypeRepresentation(dictionaryType.Namespace, dictionaryType.Name, dictionaryType.Assembly.GetName().Name, null, new[]
                {
                    new TypeRepresentation(stringType.Namespace, stringType.Name, stringType.Assembly.GetName().Name, null, new TypeRepresentation[0]),
                    new TypeRepresentation(intType.Namespace, intType.Name, intType.Assembly.GetName().Name, null, new TypeRepresentation[0]),
                }),
            });

            // Act
            var actual1 = representation1.RemoveAssemblyVersions();
            var actual2a = representation2.RemoveAssemblyVersions();
            var actual2b = actual2a.RemoveAssemblyVersions();
            var actual3 = representation3.RemoveAssemblyVersions();

            // Assert
            actual1.AsTest().Must().BeEqualTo(expected1);
            actual2a.AsTest().Must().BeEqualTo(expected2);
            actual2b.AsTest().Must().BeEqualTo(expected2);
            actual3.AsTest().Must().BeEqualTo(expected3);
        }
    }
}
