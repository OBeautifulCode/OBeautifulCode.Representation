// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyGeneration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.Test
{
    using System;
    using System.Linq;
    using OBeautifulCode.Validation.Recipes;

    public static class DummyGeneration
    {
        private const string NewDummyToken = "<<<NEWDUMMYLOGICHERE>>>";

        private const string DummyFactoryCode = @"
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => " + NewDummyToken + @");";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Want a default value for optional ThatIsNot source.")]
        public static string GenerateDummyConstructionCodeForType(
            this Type type,
            string    thatIsNot = null)
        {
            type.Named(nameof(type)).Must().NotBeNull();

            var result =
                string.IsNullOrWhiteSpace(thatIsNot)
                    ? "A.Dummy<" + type.TreatedTypeName()                                + ">()"
                    : "A.Dummy<" + type.TreatedTypeName() + ">().ThatIsNot(" + thatIsNot + ")";

            return result;
        }

        public static string GenerateCodeForDummyFactory(
            this Type type)
        {
            var properties                  = type.GetPropertiesOfConcernFromType();
            var propertyNameToSourceCodeMap = properties.ToDictionary(k => k.Name, v => v.PropertyType.GenerateDummyConstructionCodeForType());
            var newDummyToken               = type.GenerateNewLogicCodeForTypeWithSources(propertyNameToSourceCodeMap);
            var result                      = DummyFactoryCode.Replace(NewDummyToken, newDummyToken);
            return result;
        }
    }
}