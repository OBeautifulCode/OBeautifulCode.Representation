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
        private const string ClassNameToken = "<<<CLASNAMEHERE>>>";
        private const string NewObjectForEquatableToken = "<<<NEWOBJECTFOREQUATABLELOGICHERE>>>";
        private const string AssertDeepCloneToken = "<<<ASSERTDEEPCLONELOGICHERE>>>";
        private const string UnequalObjectsToken = "<<<UNEQUALOBJECTSLOGICHERE>>>";
        private const string NewDummyToken = "<<<NEWDUMMYLOGICHERE>>>";
        private const string HashToken = "<<<HASHCODELOGICHERE>>>";
        private const string EqualityToken = "<<<EQUALITYLOGICHERE>>>";
        private const string ToStringToken = "<<<TOSTRINGLOGICHERE>>>";
        private const string ToStringTestToken = "<<<TOSTRINGLOGICFORTESTHERE>>>";
        private const string DeepCloneToken = "<<<CLONELOGICHERE>>>";

        private const string ModelObjectCode = @"

        /// <summary>
        /// Determines whether two objects of type <see cref=""" + ClassNameToken + @"""/> are equal.
        /// </summary>
        /// <param name=""left"">The object to the left of the operator.</param>
        /// <param name=""right"">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; otherwise false.</returns>
        public static bool operator ==(" + ClassNameToken + @" left, " + ClassNameToken + @" right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = " + EqualityToken + @";

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref=""" + ClassNameToken + @"""/> are not equal.
        /// </summary>
        /// <param name=""left"">The object to the left of the operator.</param>
        /// <param name=""right"">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; otherwise false.</returns>
        public static bool operator !=(" + ClassNameToken + @" left, " + ClassNameToken + @" right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(" + ClassNameToken + @" other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as " + ClassNameToken + @");

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize().
            " + HashToken + @"
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public " + ClassNameToken + @" DeepClone()
        {
            var result = " + DeepCloneToken + @";

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = " + ToStringToken + @";

            return result;
        }
";

        private const string TestClassCode = @"
        private static readonly ISerializeAndDeserialize BsonSerializer = new NaosBsonSerializer<BsonConfigurationTypeHere>();
        
        private static readonly ISerializeAndDeserialize JsonSerializer = new NaosJsonSerializer<JsonConfigurationTypeHere>();

        private static readonly " + ClassNameToken + @" ObjectForEquatableTests = A.Dummy<" + ClassNameToken + @">();

        private static readonly " + ClassNameToken + @" ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests =
            " + NewObjectForEquatableToken + @";

        private static readonly " + ClassNameToken + @"[] ObjectsThatAreNotEqualToObjectForEquatableTests =
        {
            " + UnequalObjectsToken + @"
        };

        private static readonly string ObjectThatIsNotTheSameTypeAsObjectForEquatableTests = A.Dummy<string>();

        private readonly ITestOutputHelper testOutputHelper;

        public " + ClassNameToken + @"Test(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ToString___Should_generate_friendly_string_representation_of_object___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<" + ClassNameToken + @">();

            var expected = " + ToStringTestToken + @";

            // Act
            var actual = systemUnderTest.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Generate()
        {
            var results = ModelObjectCodeGenerator.GenerateCodeForModelObject<" + ClassNameToken + @">();
            this.testOutputHelper.WriteLine(results);
        }

        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Constructing
        {
            [Fact]
            public static void " + ClassNameToken + @"___Should_implement_IModel___When_reflecting()
            {
                // Arrange
                var type = typeof(" + ClassNameToken + @");
                var expectedModelMethods = typeof(IModel<" + ClassNameToken + @">)
                                          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                          .ToList();
                var expectedModelMethodHashes = expectedModelMethods.Select(_ => _.GetSignatureHash());

                // Act
                var actualInterfaces = type.GetAllInterfaces();
                var actualModelMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.DeclaringType == type).ToList();
                var actualModeMethodHashes = actualModelMethods.Select(_ => _.GetSignatureHash());

                // Assert
                actualInterfaces.Should().Contain(typeof(IModel<" + ClassNameToken + @">));
                actualModeMethodHashes.Should().Contain(expectedModelMethodHashes);
            }

            [Fact]
            public static void Constructor___Should_throw_ArgumentNullException___When_parameter_myConstructorParameterName_is_null()
            {
                throw new NotImplementedException();
            }
        }

        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Cloning
        {
            [Fact]
            public static void DeepClone___Should_deep_clone_object___When_called()
            {
                // Arrange
                var systemUnderTest = A.Dummy<" + ClassNameToken + @">();

                // Act
                var actual = systemUnderTest.DeepClone();

                // Assert
               actual.Should().Be(systemUnderTest);
               actual.Should().NotBeSameAs(systemUnderTest);
               " + AssertDeepCloneToken + @"
            }
        }

        [SuppressMessage(""Microsoft.Naming"", ""CA1724:TypeNamesShouldNotMatchNamespaces"", Justification = ""Name is correct."")]
        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Serialization
        {
            [Fact]
            public static void Deserialize___Should_roundtrip_object___When_serializing_and_deserializing_using_NaosJsonSerializer()
            {
                // Arrange
                var expected = A.Dummy<" + ClassNameToken + @">();

                var serializer = JsonSerializer;

                var serializedJson = serializer.SerializeToString(expected);

                // Act
                var actual = serializer.Deserialize<" + ClassNameToken + @">(serializedJson);

                // Assert
                actual.Should().Be(expected);
            }

            [Fact]
            public static void Deserialize___Should_roundtrip_object___When_serializing_and_deserializing_using_NaosBsonSerializer()
            {
                // Arrange
                var expected = A.Dummy<" + ClassNameToken + @">();

                var serializer = BsonSerializer;

                var serializedBson = serializer.SerializeToString(expected);

                // Act
                var actual = serializer.Deserialize<" + ClassNameToken + @">(serializedBson);

                // Assert
                actual.Should().Be(expected);
            }
        }

        [SuppressMessage(""Microsoft.Design"", ""CA1034:NestedTypesShouldNotBeVisible"", Justification = ""Grouping construct for unit test runner."")]
        public static class Equality
        {
            [Fact]
            public static void EqualsOperator___Should_return_true___When_both_sides_of_operator_are_null()
            {
                // Arrange
                " + ClassNameToken + @" systemUnderTest1 = null;
                " + ClassNameToken + @" systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 == systemUnderTest2;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                " + ClassNameToken + @" systemUnderTest = null;

                // Act
                var result1 = systemUnderTest == ObjectForEquatableTests;
                var result2 = ObjectForEquatableTests == systemUnderTest;

                // Assert
                result1.Should().BeFalse();
                result2.Should().BeFalse();
            }

            [Fact]
            public static void EqualsOperator___Should_return_true___When_same_object_is_on_both_sides_of_operator()
            {
                // Arrange, Act
    #pragma warning disable CS1718 // Comparison made to same variable
                var result = ObjectForEquatableTests == ObjectForEquatableTests;
    #pragma warning restore CS1718 // Comparison made to same variable

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void EqualsOperator___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests == _).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=>_ .First() == _.Last()).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void EqualsOperator___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests == ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests;

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_both_sides_of_operator_are_null()
            {
                // Arrange
                " + ClassNameToken + @" systemUnderTest1 = null;
                " + ClassNameToken + @" systemUnderTest2 = null;

                // Act
                var result = systemUnderTest1 != systemUnderTest2;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_one_side_of_operator_is_null_and_the_other_side_is_not_null()
            {
                // Arrange
                " + ClassNameToken + @" systemUnderTest = null;

                // Act
                var result1 = systemUnderTest != ObjectForEquatableTests;
                var result2 = ObjectForEquatableTests != systemUnderTest;

                // Assert
                result1.Should().BeTrue();
                result2.Should().BeTrue();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_same_object_is_on_both_sides_of_operator()
            {
                // Arrange, Act
    #pragma warning disable CS1718 // Comparison made to same variable
                var result = ObjectForEquatableTests != ObjectForEquatableTests;
    #pragma warning restore CS1718 // Comparison made to same variable

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_true___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests != _).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=>_ .First() != _.Last()).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeTrue());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeTrue());
            }

            [Fact]
            public static void NotEqualsOperator___Should_return_false___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests != ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests;

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_" + ClassNameToken + @"___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange
                " + ClassNameToken + @" systemUnderTest = null;

                // Act
                var result = ObjectForEquatableTests.Equals(systemUnderTest);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_" + ClassNameToken + @"___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_" + ClassNameToken + @"___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals(_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=> _.First().Equals(_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());

            }

            [Fact]
            public static void Equals_with_" + ClassNameToken + @"___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_parameter_other_is_null()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals(null);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_parameter_other_is_not_of_the_same_type()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectThatIsNotTheSameTypeAsObjectForEquatableTests);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_true___When_parameter_other_is_same_object()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void Equals_with_Object___Should_return_false___When_objects_being_compared_have_different_property_values()
            {
                // Arrange, Act
                var actualCheckReferenceAgainstUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.Select(_ => ObjectForEquatableTests.Equals((object)_)).ToList();
                var actualCheckAgainstOthersInUnequalSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=>_ .First().Equals((object)_.Last())).ToList();

                // Assert
                actualCheckReferenceAgainstUnequalSet.ForEach(_ => _.Should().BeFalse());
                actualCheckAgainstOthersInUnequalSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void Equals_with_Object___Should_return_true___When_objects_being_compared_have_same_property_values()
            {
                // Arrange, Act
                var result = ObjectForEquatableTests.Equals((object)ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public static void GetHashCode___Should_not_be_equal_for_two_objects___When_objects_have_different_property_values()
            {
                // Arrange, Act
                var actualHashCodeOfReference = ObjectForEquatableTests.GetHashCode();
                var actualHashCodesInNotEqualSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=>_ .First().GetHashCode() == _.Last().GetHashCode()).ToList();
                var actualEqualityCheckOfHashCodesAgainstOthersInNotEqualSet = ObjectsThatAreNotEqualToObjectForEquatableTests.GetCombinations(2, 2).Select( _=>_ .First().GetHashCode() == _.Last().GetHashCode()).ToList();

                // Assert
                actualHashCodesInNotEqualSet.ForEach(_ => _.Should().NotBe(actualHashCodeOfReference));
                actualEqualityCheckOfHashCodesAgainstOthersInNotEqualSet.ForEach(_ => _.Should().BeFalse());
            }

            [Fact]
            public static void GetHashCode___Should_be_equal_for_two_objects___When_objects_have_the_same_property_values()
            {
                // Arrange, Act
                var hash1 = ObjectForEquatableTests.GetHashCode();
                var hash2 = ObjectThatIsEqualButNotTheSameAsObjectForEquatableTests.GetHashCode();

                // Assert
                hash1.Should().Be(hash2);
            }
        }";

        private const string DummyFactoryCode = @"
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => " + NewDummyToken + @");";

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Want generic for future ability.")]
        public static string GenerateCodeForModelObject<T>()
        {
            var type = typeof(T);

            var dummyFactorySnippet = type.GenerateCodeForDummyFactory();
            var tests = GenerateCodeForTests(type);
            var equalityAndCloning = GenerateCodeForEqualityAndCloningSupport(type);

            var result = string.Join(
                Environment.NewLine + "--------------------------------------------------------------" + Environment.NewLine,
                new[]
                {
                    dummyFactorySnippet,
                    tests,
                    equalityAndCloning,
                });

            return result;
        }

        private static string GenerateCodeForDummyFactory(
            this Type type)
        {
            var properties = GetPropertiesOfConcernFromType(type);
            var propertyNameToSourceCodeMap = properties.ToDictionary(k => k.Name, v => v.PropertyType.GenerateDummyConstructionCodeForType());
            var newDummyToken = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMap);
            var result = DummyFactoryCode.Replace(NewDummyToken, newDummyToken);
            return result;
        }

        private static string GenerateCodeForTests(
            Type type)
        {
            var toStringTestToken = type.GenerateToStringTestConstructionCode();

            var properties = type.GetPropertiesOfConcernFromType();
            var unequalSet = new List<string>();
            foreach (var property in properties)
            {
                var propertyNameToSourceCodeMap = properties.ToDictionary(
                    k => k.Name,
                    v =>
                    {
                        var referenceObject = "ObjectForEquatableTests." + v.Name;
                        return v.Name == property.Name ? referenceObject : v.PropertyType.GenerateDummyConstructionCodeForType(referenceObject);
                    });

                var code = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMap);
                unequalSet.Add(code);
            }

            var unequalObjectsToken = string.Join("," + Environment.NewLine + "            ", unequalSet);

            var propertyNameToSourceCodeMapForNewForEquatable = properties.ToDictionary(k => k.Name, v =>
                                                                                                     {
                                                                                                         var referenceObject = "ObjectForEquatableTests." + v.Name;
                                                                                                         return v.PropertyType.GenerateDummyConstructionCodeForType(referenceObject);
                                                                                                     });

            var assertDeepCloneSet = properties.Select(_ => Invariant($"actual.{_.Name}.Should().NotBeSameAs(systemUnderTest.{_.Name})")).ToList();
            var assertDeepCloneToken = string.Join(Environment.NewLine + "               ", assertDeepCloneSet);

            var newObjectFromEquatableToken = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMapForNewForEquatable);

            var result = TestClassCode.Replace(ClassNameToken, type.Name)
                                      .Replace(ToStringTestToken, toStringTestToken)
                                      .Replace(UnequalObjectsToken, unequalObjectsToken)
                                      .Replace(AssertDeepCloneToken, assertDeepCloneToken)
                                      .Replace(NewObjectForEquatableToken, newObjectFromEquatableToken);

            return result;
        }

        private static string GenerateCodeForEqualityAndCloningSupport(Type type)
        {
            var properties = type.GetPropertiesOfConcernFromType();

            var equalityLines = properties.Select(_ => _.GenerateEqualityLogicCodeForProperty()).ToList();
            var hashLines = properties.Select(_ => _.GenerateHashCodeMethodCodeForProperty()).ToList();
            var propertyNameToCloneMethodMap = properties.ToDictionary(
                k => k.Name,
                v => v.PropertyType.GenerateCloningLogicCodeForType("this." + v.Name));

            var equalityToken = string.Join(Environment.NewLine + "                      && ", equalityLines);
            var hashToken = string.Join(".", hashLines);
            var deepCloneToken = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToCloneMethodMap);
            var toStringToken = type.GenerateToStringConstructionCode();

            var result = ModelObjectCode.Replace(ClassNameToken, type.Name)
                                        .Replace(EqualityToken, equalityToken)
                                        .Replace(HashToken, hashToken)
                                        .Replace(DeepCloneToken, deepCloneToken)
                                        .Replace(ToStringToken, toStringToken);

            return result;
        }

        private static PropertyInfo[] GetPropertiesOfConcernFromType(
            this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy);
        }

        private static string GenerateToStringConstructionCode(
            this Type type)
        {
            var propertyNames = type.GetPropertiesOfConcernFromType().Select(_ => _.Name).ToList();
            return "Invariant($\"{nameof("
                 + type.Namespace
                 + ")}.{nameof("
                 + type.Name
                 + ")}\")}: "
                 + string.Join(
                       ", ",
                       propertyNames.Select(_ => _ + " = {this." + _ + "?.ToString() ?? \"<null>\"})"))
                 + ".\"";
        }

        private static string GenerateToStringTestConstructionCode(
            this Type type)
        {
            var propertyNames = type.GetPropertiesOfConcernFromType().Select(_ => _.Name).ToList();
            return "Invariant($\""
                 + type.Namespace
                 + "."
                 + type.Name
                 + ": "
                 + string.Join(
                       ", ",
                       propertyNames.Select(_ => _ + " = {systemUnderTest." + _ + "})"))
                 + ".\"";
        }

        private static string GenerateNewLogicCodeForTypeWithSources(
            this Type    type,
            Dictionary<string, string> propertyNameToSourceCodeMap)
        {
            if (type.GetConstructors()
                    .Any(
                         _ =>
                         {
                             var parameters = _.GetParameters();
                             var nonMatchingParameters = parameters.Select(p => p.Name)
                                                                   .SymmetricDifference(propertyNameToSourceCodeMap.Keys.Select(k => k.ToLowerFirstLetter()))
                                                                   .ToList();

                             return nonMatchingParameters.Count == 0;
                         }))
            {
                var parameterPadding = "                                 ";
                return "new " + type.Name + "(" + Environment.NewLine + parameterPadding + string.Join("," + Environment.NewLine + parameterPadding, propertyNameToSourceCodeMap.Values) + ")";
            }
            else if (GetPropertiesOfConcernFromType(type).All(_ => _.CanWrite))
            {
                return "new "
                     + type.Name
                     + "{"
                     + string.Join(", ", propertyNameToSourceCodeMap.Select(_ => Invariant($"{_.Key} = {_.Value}")))
                     + "}";
            }
            else
            {
                var propertiesAddIn = string.Join(",", GetPropertiesOfConcernFromType(type).Select(_ => _.Name));
                throw new NotSupportedException("Could not find a constructor to take properties of concern and they are not all settable: " + propertiesAddIn);
            }
        }

        private static string GenerateDummyConstructionCodeForType(
            this Type type,
            string thatIsNot = null)
        {
            type.Named(nameof(type)).Must().NotBeNull();

            var result =
                string.IsNullOrWhiteSpace(thatIsNot)
                    ? "A.Dummy<" + type.TreatedTypeName() + ">()"
                    : "A.Dummy<" + type.TreatedTypeName() + ">().ThatIsNot(" + thatIsNot + ")";

            return result;
        }

        private static string GenerateCloningLogicCodeForType(
            this Type type,
            string cloneSourceCode)
        {
            type.Named(nameof(type)).Must().NotBeNull();

            string result;
            if (type.IsAssignableToAnyDictionary())
            {
                var genericArguments = type.GetGenericArguments();
                genericArguments.Length.Named(Invariant($"Number{nameof(genericArguments)}Of{nameof(type)}.{nameof(type)}For{type.Name}"))
                                .Must()
                                .BeEqualTo(2);

                var keyType = genericArguments.First();
                var valueType = genericArguments.Last();
                var keyClone = keyType.GenerateCloningLogicCodeForType("k.Key");
                var valueClone = valueType.GenerateCloningLogicCodeForType("v.Value");
                result = Invariant($"{cloneSourceCode}?.ToDictionary(k => {keyClone}, v => {valueClone})");
            }
            else if (type.IsAssignableToAnyCollection())
            {
                var genericArguments = type.GetGenericArguments();
                var valueType = genericArguments.Single();
                var valueClone = valueType.GenerateCloningLogicCodeForType("_");
                result = Invariant($"{cloneSourceCode}?.Select(_ => {valueClone}).ToList()");
            }
            else if (type == typeof(string))
            {
                result = Invariant($"{cloneSourceCode}?.Clone().ToString()");
            }
            else
            {
                result = Invariant($"{cloneSourceCode}?.DeepClone()");
            }

            return result;
        }

        private static string GenerateHashCodeMethodCodeForProperty(
            this PropertyInfo propertyInfo)
        {
            propertyInfo.Named(nameof(propertyInfo)).Must().NotBeNull();

            if (propertyInfo.PropertyType.IsAssignableToAnyDictionary())
            {
                return Invariant($"HashDictionary(this.{propertyInfo.Name})");
            }
            else if (propertyInfo.PropertyType.IsAssignableToAnyCollection())
            {
                return Invariant($"HashElements(this.{propertyInfo.Name})");
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                return Invariant($"Hash(this.{propertyInfo.Name})");
            }
            else
            {
                return Invariant($"Hash(this.{propertyInfo.Name})");
            }
        }

        private static string GenerateEqualityLogicCodeForProperty(
            this PropertyInfo propertyInfo)
        {
            propertyInfo.Named(nameof(propertyInfo)).Must().NotBeNull();

            if (propertyInfo.PropertyType.IsAssignableToAnyDictionary())
            {
                return Invariant($"left.{propertyInfo.Name}.DictionaryEqual(right.{propertyInfo.Name})");
            }
            else if (propertyInfo.PropertyType.IsAssignableToAnyCollection())
            {
                return Invariant($"left.{propertyInfo.Name}.SequenceEqualHandlingNulls(right.{propertyInfo.Name})");
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                return Invariant($"left.{propertyInfo.Name}.Equals(right.{propertyInfo.Name}, StringComparison.Ordinal)");
            }
            else
            {
                return Invariant($"left.{propertyInfo.Name} == right.{propertyInfo.Name}");
            }
        }

        private static bool IsAssignableToAnyDictionary(
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

        private static bool IsAssignableToAnyCollection(
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
        private static string TreatedTypeName(
            this Type type)
        {
            if (type == typeof(string))
            {
                return typeof(string).Name.ToLowerInvariant();
            }
            else
            {
                return type.Name;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Lowercase is correct here.")]
        private static string ToLowerFirstLetter(
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
    }
}