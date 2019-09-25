// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationDummyFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Linq.Expressions;
    using global::System.Reflection;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Reflection.Recipes;

    using static global::System.FormattableString;

    /// <summary>
    /// Type of how to control how dummy objects get created.
    /// </summary>
    public class RepresentationDummyFactory : IDummyFactory
    {
        private static readonly List<Type> AppDomainTypes = new List<Type>();
        private static readonly Random Random = new Random();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Prefer this structure.")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "accumulatedReflectionTypeLoadExceptions", Justification = "Prefer this structure.")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Prefer this structure.")]
        static RepresentationDummyFactory()
        {
            var loadedAssemblies             = AssemblyLoader.GetLoadedAssemblies().Distinct().ToList();
            var reflectionTypeLoadExceptions = new List<ReflectionTypeLoadException>();
            foreach (var assembly in loadedAssemblies)
            {
                try
                {
                    AppDomainTypes.AddRange(new[] { assembly }.GetTypesFromAssemblies());
                }
                catch (TypeLoadException ex) when (ex.InnerException?.GetType() == typeof(ReflectionTypeLoadException))
                {
                    var reflectionTypeLoadException = (ReflectionTypeLoadException)ex.InnerException;
                    AppDomainTypes.AddRange(reflectionTypeLoadException.Types);
                    reflectionTypeLoadExceptions.Add(reflectionTypeLoadException);
                }
            }

            AggregateException accumulatedReflectionTypeLoadExceptions = reflectionTypeLoadExceptions.Any()
                ? new AggregateException(Invariant($"Getting types from assemblies threw one or more {nameof(ReflectionTypeLoadException)}.  See inner exceptions."), reflectionTypeLoadExceptions)
                : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationDummyFactory"/> class.
        /// </summary>
        public RepresentationDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var index = Random.Next(0, AppDomainTypes.Count - 1);

                    var result = AppDomainTypes[index];
                    return result.ToRepresentation();
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new AssemblyRepresentation(
                    A.Dummy<string>(),
                    A.Dummy<string>(),
                    A.Dummy<string>(),
                    A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ElementInitRepresentation(
                    A.Dummy<TypeRepresentation>(),
                    A.Dummy<MethodInfoRepresentation>(),
                    A.Dummy<IReadOnlyList<ExpressionRepresentationBase>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => (ExpressionRepresentationBase)new ConstantExpressionRepresentation<string>(
                    A.Dummy<string>(),
                    A.Dummy<ExpressionType>(),
                    A.Dummy<TypeRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ConstantExpressionRepresentation<string>(
                    A.Dummy<string>(),
                    A.Dummy<ExpressionType>(),
                    A.Dummy<TypeRepresentation>()));
        }

        /// <inheritdoc />
        public Priority Priority => new Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}