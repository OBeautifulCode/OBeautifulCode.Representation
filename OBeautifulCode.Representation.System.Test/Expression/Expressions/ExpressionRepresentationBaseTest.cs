// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionRepresentationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using FakeItEasy;

    using global::System.Linq.Expressions;

    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public delegate OutputForExample DelegateForExample(InputForExample input);

    public static partial class ExpressionRepresentationBaseTest
    {
        [Fact]
        public static void Basic_lambda()
        {
            Expression<DelegateForExample> lambda = _ => new OutputForExample(_, "open");

            var lambdaDescription = lambda.ToRepresentation();

            var lambdaAgain = lambdaDescription.FromRepresentation();

            var input = A.Dummy<InputForExample>();

            var output = (OutputForExample)lambdaAgain.Compile().DynamicInvoke(input);

            output.Input.Input.AsTest().Must().BeEqualTo(input.Input);

            output.Extra.AsTest().Must().BeEqualTo("open");
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