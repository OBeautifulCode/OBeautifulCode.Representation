// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeComparerTests.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using global::System;

    using Xunit;

    public static class TypeComparerTests
    {
        [Fact]
        public static void Equals_Nulls_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);

            Assert.False(comparer.Equals((Type)null, (Type)null));
            Assert.False(comparer.Equals((TypeRepresentation)null, (TypeRepresentation)null));

            Assert.False(comparer.Equals(typeof(string), (Type)null));
            Assert.False(comparer.Equals((Type)null, typeof(string)));

            Assert.False(comparer.Equals(typeof(string).ToRepresentation(), (TypeRepresentation)null));
            Assert.False(comparer.Equals((TypeRepresentation)null, typeof(string).ToRepresentation()));
        }

        [Fact]
        public static void EqualsNamespaceAndName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x, y);
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsAssemblyQualifiedName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x, y);
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsNamespaceAndName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x, y);
            Assert.False(actual);
        }

        [Fact]
        public static void EqualsAssemblyQualifiedName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x, y);
            Assert.False(actual);
        }

        [Fact]
        public static void EqualsStringsNamespaceAndName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x.Namespace, x.Name, x.AssemblyQualifiedName, y.Namespace, y.Name, y.AssemblyQualifiedName);
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsStringsAssemblyQualifiedName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x.Namespace, x.Name, x.AssemblyQualifiedName, y.Namespace, y.Name, y.AssemblyQualifiedName);
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsStringsNamespaceAndName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x.Namespace, x.Name, x.AssemblyQualifiedName, y.Namespace, y.Name, y.AssemblyQualifiedName);
            Assert.False(actual);
        }

        [Fact]
        public static void EqualsStringsAssemblyQualifiedName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x.Namespace, x.Name, x.AssemblyQualifiedName, y.Namespace, y.Name, y.AssemblyQualifiedName);
            Assert.False(actual);
        }

        [Fact]
        public static void EqualsDescriptionNamespaceAndName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x.ToRepresentation(), y.ToRepresentation());
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsDescriptionAssemblyQualifiedName_Match_True()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(string);
            var actual = comparer.Equals(x.ToRepresentation(), y.ToRepresentation());
            Assert.True(actual);
        }

        [Fact]
        public static void EqualsDescriptionNamespaceAndName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.NamespaceAndName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x.ToRepresentation(), y.ToRepresentation());
            Assert.False(actual);
        }

        [Fact]
        public static void EqualsDescriptionAssemblyQualifiedName_NoMatch_False()
        {
            var comparer = new TypeComparer(TypeMatchStrategy.AssemblyQualifiedName);
            var x = typeof(string);
            var y = typeof(Type);
            var actual = comparer.Equals(x.ToRepresentation(), y.ToRepresentation());
            Assert.False(actual);
        }
    }
}