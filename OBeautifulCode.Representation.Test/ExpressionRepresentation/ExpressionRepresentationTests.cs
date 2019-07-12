// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionRepresentationTests.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using FakeItEasy;
    using FluentAssertions;
    using Xunit;

    public delegate OutputForExample DelegateForExample(
        InputForExample input);

    public static class ExpressionRepresentationTests
    {
        [Fact]
        public static void Basic_lambda()
        {
            Expression<DelegateForExample> lambda = _ => new OutputForExample(_, "open");
            var lambdaDescription = lambda.ToRepresentation();
            var lambdaAgain = lambdaDescription.FromRepresentation();
            var input = A.Dummy<InputForExample>();
            var output = (OutputForExample)lambdaAgain.Compile().DynamicInvoke(input);
            output.Input.Input.Should().Be(input.Input);
            output.Extra.Should().Be("open");
        }
    }

    public class InputForExample
    {
        public InputForExample(
            string input)
        {
            this.Input = input;
        }

        public string Input { get; private set; }
    }

    public class OutputForExample
    {
        public OutputForExample(
            InputForExample input,
            string extra)
        {
            this.Input = input;
            this.Extra = extra;
        }

        public InputForExample Input { get; private set; }

        public string Extra { get; private set; }
    }
}