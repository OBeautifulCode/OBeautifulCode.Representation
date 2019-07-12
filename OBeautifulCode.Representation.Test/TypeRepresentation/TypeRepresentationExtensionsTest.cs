// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using FluentAssertions;

    using Xunit;

    public static class TypeRepresentationExtensionsTest
    {
        [Fact]
        public static void ToTypeRepresentation___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange, Act
            var ex = Record.Exception(() => TypeRepresentationExtensions.ToRepresentation(null));

            // Assert
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToTypeRepresentation___Should_return_type_description___When_called()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var description = type.ToRepresentation();

            // Assert
            description.AssemblyQualifiedName.Should().Be(type.AssemblyQualifiedName);
            description.Namespace.Should().Be(type.Namespace);
            description.Name.Should().Be(type.Name);
        }

        [Fact]
        public static void ResolvedFromLoadedTypes___Should_return_a_loaded_type___When_type_is_loaded()
        {
            // Arrange
            var expectedType = typeof(string);
            var typeDescriptionBase = expectedType.ToRepresentation();
            var typeDescription = new TypeRepresentation(typeDescriptionBase.Namespace, typeDescriptionBase.Name, typeDescriptionBase.AssemblyQualifiedName, new List<TypeRepresentation>());
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Reset();
            stopwatch.Start();
            var actualTypeFirst = typeDescription.ResolveFromLoadedTypes();
            stopwatch.Stop();
            var firstElapsed = stopwatch.Elapsed;

            stopwatch.Reset();
            stopwatch.Start();
            var actualTypeSecond = typeDescription.ResolveFromLoadedTypes();
            stopwatch.Stop();
            var secondElapsed = stopwatch.Elapsed;

            // Assert
            actualTypeFirst.Should().Be(expectedType);
            actualTypeSecond.Should().Be(expectedType);
            firstElapsed.Should().BeGreaterThan(secondElapsed); // this is because the lookup is cached...
        }
    }
}
