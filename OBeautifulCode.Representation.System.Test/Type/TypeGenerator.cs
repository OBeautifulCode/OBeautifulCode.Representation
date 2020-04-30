// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeGenerator.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using global::OBeautifulCode.Representation.System.Test.Internal;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Reflection.Recipes;

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

            var customGenericClassTypes1 = coreTypes.Select(_ => typeof(GenericClass<>).MakeGenericType(_)).ToList();

            var customGenericClassTypes2 = coreTypes.Select(_ => typeof(TestClassInNamespace<>).MakeGenericType(_)).ToList();

            var customGenericClassTypes3 = coreTypes.Select(_ => typeof(TestClassInNamespace<>.NestedClassInTestClassInNamespace<>).MakeGenericType(_, _)).ToList();

            var customGenericClassTypes4 = coreTypes.Select(_ => typeof(TestClassInNamespace<>.NestedClassInTestClassInNamespace<>.NestedClassInNestedClassInTestClassInNamespace<>).MakeGenericType(_, _, _)).ToList();

            var vectorArrayTypes = coreTypes.Select(_ => _.MakeArrayType()).ToList();

            var rank1ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(1)).ToList();

            var rank2ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(2)).ToList();

            var rank3ArrayTypes = coreTypes.Select(_ => _.MakeArrayType(3)).ToList();

            var testTypes = new Type[0]
                .Concat(coreTypes)
                .Concat(genericInterfaceTypes)
                .Concat(systemGenericClassTypes)
                .Concat(customGenericClassTypes1)
                .Concat(customGenericClassTypes2)
                .Concat(customGenericClassTypes3)
                .Concat(customGenericClassTypes4)
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

            var closedTypes = new Type[0]
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

            var genericTypeDefinitions = closedTypes.Where(_ => _.IsGenericType).Select(_ => _.GetGenericTypeDefinition()).Distinct().ToList();

            var result = new Type[0]
                .Concat(closedTypes)
                .Concat(genericTypeDefinitions)
                .Distinct()
                .ToList();

            return result;
        }

        public static Assembly LoadOlderVersionOfConditions()
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
            var result = AppDomain.CurrentDomain.Load(assemblyBytes);

            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public sealed class TestClassInStaticClass
        {
            private TestClassInStaticClass()
            {
            }

            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
            public sealed class NestedClassInTestClassInStaticClass
            {
                private NestedClassInTestClassInStaticClass()
                {
                }

                [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
                public class NestedClassInNestedClassInTestClassInStaticClass
                {
                    private NestedClassInNestedClassInTestClassInStaticClass()
                    {
                    }
                }
            }
        }
    }

    public sealed class TestClassInNamespace
    {
        private TestClassInNamespace()
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public sealed class NestedClassInTestClassInNamespace
        {
            private NestedClassInTestClassInNamespace()
            {
            }

            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
            public sealed class NestedClassInNestedClassInTestClassInNamespace
            {
                private NestedClassInNestedClassInTestClassInNamespace()
                {
                }
            }
        }
    }

    public sealed class TestClassInNamespace<T>
    {
        private TestClassInNamespace()
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public sealed class NestedClassInTestClassInNamespace<T2>
        {
            private NestedClassInTestClassInNamespace()
            {
            }

            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
            public sealed class NestedClassInNestedClassInTestClassInNamespace<T3>
            {
                private NestedClassInNestedClassInTestClassInNamespace()
                {
                }
            }
        }
    }

    // ReSharper disable once UnusedTypeParameter
    public class GenericClass<T>
    {
    }
}
