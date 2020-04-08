// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeGenerator.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    public static class TypeGenerator
    {
        public static IReadOnlyList<Type> GenerateTypesForTesting()
        {
            var coreTypes = new[]
            {
                typeof(int),
                typeof(string),
                typeof(Guid),
                typeof(DateTime),
                typeof(object),
                typeof(ConstructorInfoRepresentation),
                typeof(TestClassInStaticClass),
                typeof(TestClassInStaticClass.NestedClassInTestClassInStaticClass),
                typeof(TestClassInStaticClass.NestedClassInTestClassInStaticClass.NestedClassInNestedClassInTestClassInStaticClass),
                typeof(TestClassInNamespace),
                typeof(TestClassInNamespace.NestedClassInTestClassInNamespace),
                typeof(TestClassInNamespace.NestedClassInTestClassInNamespace.NestedClassInNestedClassInTestClassInNamespace),
                typeof(global::System.Collections.IEnumerable),
                typeof(int?),
                typeof(Guid?),
                typeof(DateTime?),
            };

            var genericInterfaceTypes = coreTypes.Select(_ => typeof(IList<>).MakeGenericType(_)).ToList();

            var systemGenericClassTypes = coreTypes.Select(_ => typeof(Dictionary<,>).MakeGenericType(_, _)).ToList();

            var customGenericClassTypes = coreTypes.Select(_ => typeof(GenericClass<>).MakeGenericType(_)).ToList();

            var vectorArrayTypes = coreTypes.Select(_ => _.MakeArrayType()).ToList();

            var rank1ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(1)).ToList();

            var rank2ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(2)).ToList();

            var rank3ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(3)).ToList();

            var testTypes = new Type[0]
                .Concat(coreTypes)
                .Concat(genericInterfaceTypes)
                .Concat(systemGenericClassTypes)
                .Concat(customGenericClassTypes)
                .Concat(vectorArrayTypes)
                .Concat(rank1ArrayTypes)
                .Concat(rank2ArrayTypes)
                .Concat(rank3ArrayTypes)
                .ToList();

            var genericInterfaceOfTestTypes = testTypes.Select(_ => typeof(IList<>).MakeGenericType(_)).ToList();

            var genericClassOfTestTypes = testTypes.Select(_ => typeof(Dictionary<,>).MakeGenericType(_, _)).ToList();

            var vectorArrayOfTestTypes = testTypes.Select(_ => _.MakeArrayType()).ToList();

            var rank1ArrayOfTestTypes = testTypes.Select(_ => _.MakeArrayType(1));

            var rank2ArrayOfTestTypes = testTypes.Select(_ => _.MakeArrayType(2));

            var rank3ArrayOfTestTypes = testTypes.Select(_ => _.MakeArrayType(3));

            var additionalTypes = new[]
            {
                typeof(IReadOnlyDictionary<Guid?, IReadOnlyDictionary<ConstructorInfoRepresentation, DateTime>>),
                typeof(IReadOnlyDictionary<IReadOnlyDictionary<Guid[], int?>, IList<IList<short>>[]>),
                typeof(IReadOnlyDictionary<IReadOnlyDictionary<ConstructorInfoRepresentation[], int?>, IList<IList<TestClassInStaticClass.NestedClassInTestClassInStaticClass>>[]>[]),
            };

            var result = new Type[0]
                .Concat(testTypes)
                .Concat(genericInterfaceOfTestTypes)
                .Concat(genericClassOfTestTypes)
                .Concat(vectorArrayOfTestTypes)
                .Concat(rank1ArrayOfTestTypes)
                .Concat(rank2ArrayOfTestTypes)
                .Concat(rank3ArrayOfTestTypes)
                .Concat(additionalTypes)
                .Distinct()
                .ToList();

            return result;
        }

        public class TestClassInStaticClass
        {
            public class NestedClassInTestClassInStaticClass
            {
                public class NestedClassInNestedClassInTestClassInStaticClass
                {
                }
            }
        }
    }

    public class TestClassInNamespace
    {
        public class NestedClassInTestClassInNamespace
        {
            public class NestedClassInNestedClassInTestClassInNamespace
            {
            }
        }
    }

    public class GenericClass<T>
    {
    }
}
