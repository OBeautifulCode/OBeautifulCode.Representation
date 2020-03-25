// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="Assembly" />.
    /// </summary>
    public partial class AssemblyRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyRepresentation"/> class.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="version">Version of the assembly.</param>
        /// <param name="filePath">File path of the assembly observed.</param>
        /// <param name="frameworkVersion">Framework of assembly.</param>
        public AssemblyRepresentation(
            string name,
            string version,
            string filePath,
            string frameworkVersion)
        {
            name.AsArg(nameof(name)).Must().NotBeNullNorWhiteSpace();

            this.Name             = name;
            this.Version          = version;
            this.FilePath         = filePath;
            this.FrameworkVersion = frameworkVersion;
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets the file path the assembly was observed at.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the .NET framework the assembly was build for.
        /// </summary>
        public string FrameworkVersion { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="AssemblyRepresentation" />.
    /// </summary>
    public static class AssemblyRepresentationExtensions
    {
        /// <summary>
        /// Reads the assembly to create a new <see cref="AssemblyRepresentation"/>.
        /// </summary>
        /// <param name="assembly">The assembly object to interrogate.</param>
        /// <returns>
        /// Details about an assembly.
        /// </returns>
        public static AssemblyRepresentation ToRepresentation(
            this Assembly assembly)
        {
            new { assembly }.AsArg().Must().NotBeNull();

            var codeBasesToIgnore = new List<string>(new[] { "Microsoft.GeneratedCode", "Anonymously Hosted DynamicMethods Assembly" });

            var asmName = assembly.GetName();

            var frameworkVersionNumber = assembly.ImageRuntimeVersion.Substring(1, 3);

            var version = asmName.Version.ToString();

            var name = asmName.Name;

            var codeBase = codeBasesToIgnore.Contains(name) ? name : assembly.GetCodeBaseAsPathInsteadOfUri();

            var result = new AssemblyRepresentation(name, version, codeBase, frameworkVersionNumber);

            return result;
        }

        /// <summary>
        /// Converts from the Representation back to the original.
        /// </summary>
        /// <param name="assemblyRepresentation">The assembly representation.</param>
        /// <returns>
        /// The assembly.
        /// </returns>
        public static Assembly FromRepresentation(
            this AssemblyRepresentation assemblyRepresentation)
        {
            new { assemblyRepresentation }.AsArg().Must().NotBeNull();

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var results = allAssemblies
                              .Where(_ => _.ToRepresentation().Equals(assemblyRepresentation))
                              .ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find an assembly that matched representation '{assemblyRepresentation}' in '{nameof(AppDomain)}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.ToString()));

                throw new ArgumentException(Invariant($"Found too many assemblies that matched representation '{assemblyRepresentation}' in '{nameof(AppDomain)}'; {foundAddIn}."));
            }

            var result = results.Single();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
