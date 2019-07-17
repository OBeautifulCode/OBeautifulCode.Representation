// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Validation.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Representation of <see cref="Assembly" />.
    /// </summary>
    public class AssemblyRepresentation : IModel<AssemblyRepresentation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyRepresentation"/> class.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="version">Version of the assembly.</param>
        /// <param name="filePath">File path of the assembly observed.</param>
        /// <param name="frameworkVersion">Framework of assembly.</param>
        public AssemblyRepresentation(string name, string version, string filePath, string frameworkVersion)
        {
            new { name }.Must().NotBeNullNorWhiteSpace();

            this.Name = name;
            this.Version = version;
            this.FilePath = filePath;
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

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left">Left parameter.</param>
        /// <param name="right">Right parameter.</param>
        /// <returns>A value indicating whether or not the two items are equal.</returns>
        public static bool operator ==(AssemblyRepresentation left, AssemblyRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Name.Equals(right.Name, StringComparison.Ordinal)
                && left.Version.Equals(right.Version, StringComparison.Ordinal)
                && left.FilePath.Equals(right.FilePath, StringComparison.Ordinal)
                && left.FrameworkVersion.Equals(right.FrameworkVersion, StringComparison.Ordinal);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left">Left parameter.</param>
        /// <param name="right">Right parameter.</param>
        /// <returns>A value indicating whether or not the two items are unequal.</returns>
        public static bool operator !=(AssemblyRepresentation left, AssemblyRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(AssemblyRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as AssemblyRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize().
            Hash(this.Name).Hash(this.Version).Hash(this.FilePath).Hash(this.FrameworkVersion)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public AssemblyRepresentation DeepClone()
        {
            var result = new AssemblyRepresentation(this.Name?.Clone().ToString(), this.Version?.Clone().ToString(), this.FilePath?.Clone().ToString(), this.FrameworkVersion?.Clone().ToString());

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"{nameof(AssemblyRepresentation)}: Name = {this.Name?.ToString() ?? "<null>"} ,Version = {this.Version?.ToString() ?? "<null>"} ,FilePath = {this.FilePath?.ToString() ?? "<null>"} ,FrameworkVersion = {this.FrameworkVersion?.ToString() ?? "<null>"}.");

            return result;
        }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="AssemblyRepresentation" />.
    /// </summary>
    public static class AssemblyRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>
        /// Reads the assembly to create a new <see cref="AssemblyRepresentation"/>.
        /// </summary>
        /// <param name="assembly">The assembly object to interrogate.</param>
        /// <returns>Details about an assembly.</returns>
        public static AssemblyRepresentation ToRepresentation(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var codeBasesToIgnore = new List<string>(new[] { "Microsoft.GeneratedCode", "Anonymously Hosted DynamicMethods Assembly" });

            var asmName = assembly.GetName();

            var frameworkVersionNumber = assembly.ImageRuntimeVersion.Substring(1, 3);

            var version = asmName.Version.ToString();
            var name = asmName.Name;

            var codeBase = codeBasesToIgnore.Contains(name) ? name : assembly.GetCodeBaseAsPathInsteadOfUri();

            return new AssemblyRepresentation(name, version, codeBase, frameworkVersionNumber);
        }

        /// <summary>
        /// Converts from the Representation back to the original.
        /// </summary>
        /// <param name="assemblyRepresentation">The assembly representation.</param>
        /// <returns>System.Reflection.Assembly.</returns>
        public static Assembly FromRepresentation(
                    this AssemblyRepresentation assemblyRepresentation)
        {
            if (assemblyRepresentation == null)
            {
                throw new ArgumentNullException(nameof(assemblyRepresentation));
            }

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

            return results.Single();
        }
    }
}
