// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test.AssemblyRepresentationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FakeItEasy;
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

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