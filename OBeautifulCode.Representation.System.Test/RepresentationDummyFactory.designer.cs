﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationDummyFactory.designer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package OBeautifulCode.Build.Conventions.VisualStudioProjectTemplates.Domain.Test (1.1.92)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System.Test
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    /// <summary>
    /// DO NOT EDIT.  
    /// THIS CLASS EXISTS SO THAT THE DUMMY FACTORY CAN INHERIT FROM IT AND THE PROJECT CAN COMPILE.
    /// THIS WILL BE REPLACED BY A CODE GENERATED DEFAULT DUMMY FACTORY.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.Build.Conventions.VisualStudioProjectTemplates.Domain.Test", "1.1.92")]
    public abstract class DefaultRepresentationDummyFactory : IDummyFactory
    {
        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

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