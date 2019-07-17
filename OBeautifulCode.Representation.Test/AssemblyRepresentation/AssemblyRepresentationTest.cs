// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test.AssemblyRepresentationTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy.Internal;
    using FakeItEasy;
    using FluentAssertions;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    public class AssemblyRepresentationTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public AssemblyRepresentationTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Generate()
        {
            var results = ModelObjectCodeGenerator.GenerateCodeForModelObject<AssemblyRepresentation>();
            this.testOutputHelper.WriteLine(results);
        }
    }
}