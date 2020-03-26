﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorPropertyAssignmentTestScenario{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.CodeGen.ModelObject.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.CodeGen.ModelObject.Recipes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using OBeautifulCode.CodeGen.ModelObject.Recipes.Internal;

    /// <summary>
    /// Specifies a scenario for testing when a constructor sets a property values.
    /// </summary>
    /// <typeparam name="T">The type of the object being tested.</typeparam>
#if !OBeautifulCodeCodeGenRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.CodeGen.ModelObject.Recipes", "See package version number")]
    internal
#else
    public
#endif
    class ConstructorPropertyAssignmentTestScenario<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the name of the scenario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a func that returns the object to test and the expected value of the property being tested.
        /// </summary>
        public Func<SystemUnderTestExpectedPropertyValue<T>> SystemUnderTestExpectedPropertyValueFunc { get; set; }
        
        /// <summary>
        /// Gets or sets a func that calls the getter of the property that is assigned a value by the constructor.
        /// </summary>
        public Func<T, object> PropertyGetterFunc { get; set; }

        /// <summary>
        /// Gets a scenario to use when no properties are assigned in the constructor.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = ObcSuppressBecause.CA1000_DoNotDeclareStaticMembersOnGenericTypes_StaticPropertyReturnsInstanceOfContainingGenericClassAndIsConvenientAndMostDiscoverableWhereDeclared)]
        public static ConstructorPropertyAssignmentTestScenario<T> NoPropertiesAssignedInConstructorScenario =>
            new ConstructorPropertyAssignmentTestScenario<T>
            {
                Name = "no properties assigned in constructor scenario",
                SystemUnderTestExpectedPropertyValueFunc = () => new SystemUnderTestExpectedPropertyValue<T>
                {
                    SystemUnderTest = (T)FormatterServices.GetUninitializedObject(typeof(T)),
                    ExpectedPropertyValue = null,
                },
                PropertyGetterFunc = systemUnderTest => null,
            };

        /// <summary>
        /// Gets a scenario to use when you need to force the consuming unit tests to pass and you intend to write your own unit tests.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = ObcSuppressBecause.CA1000_DoNotDeclareStaticMembersOnGenericTypes_StaticPropertyReturnsInstanceOfContainingGenericClassAndIsConvenientAndMostDiscoverableWhereDeclared)]
        public static ConstructorPropertyAssignmentTestScenario<T> ForceGeneratedTestsToPassAndWriteMyOwnScenario =>
            new ConstructorPropertyAssignmentTestScenario<T>
            {
                Name = "force generated unit tests to pass, i'll write my own",
                SystemUnderTestExpectedPropertyValueFunc = () => new SystemUnderTestExpectedPropertyValue<T>
                {
                    SystemUnderTest = (T)FormatterServices.GetUninitializedObject(typeof(T)),
                    ExpectedPropertyValue = null,
                },
                PropertyGetterFunc = systemUnderTest => null,
            };
    }
}