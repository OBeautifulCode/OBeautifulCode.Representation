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
        /// Determines whether two objects of type <see cref="AssemblyRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; otherwise false.</returns>
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

            var result = left.Name.Equals(right.Name, StringComparison.Ordinal)
                      && left.Version.Equals(right.Version, StringComparison.Ordinal)
                      && left.FilePath.Equals(right.FilePath, StringComparison.Ordinal)
                      && left.FrameworkVersion.Equals(right.FrameworkVersion, StringComparison.Ordinal);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="AssemblyRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; otherwise false.</returns>
        public static bool operator !=(AssemblyRepresentation left, AssemblyRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(AssemblyRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as AssemblyRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Name)
            .Hash(this.Version)
            .Hash(this.FilePath)
            .Hash(this.FrameworkVersion)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public AssemblyRepresentation DeepClone()
        {
            var result = new AssemblyRepresentation(
                                 this.Name?.Clone().ToString(),
                                 this.Version?.Clone().ToString(),
                                 this.FilePath?.Clone().ToString(),
                                 this.FrameworkVersion?.Clone().ToString());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="name" />.
        /// </summary>
        /// <param name="name">The new <see cref="Name" />.</param>
        /// <returns>New <see cref="AssemblyRepresentation" /> using the specified <paramref name="name" /> for <see cref="Name" /> and a deep clone of every other property.</returns>
        public AssemblyRepresentation DeepCloneWithName(string name)
        {
            var result = new AssemblyRepresentation(
                                 name,
                                 this.Version?.Clone().ToString(),
                                 this.FilePath?.Clone().ToString(),
                                 this.FrameworkVersion?.Clone().ToString());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="version" />.
        /// </summary>
        /// <param name="version">The new <see cref="Version" />.</param>
        /// <returns>New <see cref="AssemblyRepresentation" /> using the specified <paramref name="version" /> for <see cref="Version" /> and a deep clone of every other property.</returns>
        public AssemblyRepresentation DeepCloneWithVersion(string version)
        {
            var result = new AssemblyRepresentation(
                                 this.Name?.Clone().ToString(),
                                 version,
                                 this.FilePath?.Clone().ToString(),
                                 this.FrameworkVersion?.Clone().ToString());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="filePath" />.
        /// </summary>
        /// <param name="filePath">The new <see cref="FilePath" />.</param>
        /// <returns>New <see cref="AssemblyRepresentation" /> using the specified <paramref name="filePath" /> for <see cref="FilePath" /> and a deep clone of every other property.</returns>
        public AssemblyRepresentation DeepCloneWithFilePath(string filePath)
        {
            var result = new AssemblyRepresentation(
                                 this.Name?.Clone().ToString(),
                                 this.Version?.Clone().ToString(),
                                 filePath,
                                 this.FrameworkVersion?.Clone().ToString());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="frameworkVersion" />.
        /// </summary>
        /// <param name="frameworkVersion">The new <see cref="FrameworkVersion" />.</param>
        /// <returns>New <see cref="AssemblyRepresentation" /> using the specified <paramref name="frameworkVersion" /> for <see cref="FrameworkVersion" /> and a deep clone of every other property.</returns>
        public AssemblyRepresentation DeepCloneWithFrameworkVersion(string frameworkVersion)
        {
            var result = new AssemblyRepresentation(
                                 this.Name?.Clone().ToString(),
                                 this.Version?.Clone().ToString(),
                                 this.FilePath?.Clone().ToString(),
                                 frameworkVersion);
            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"{nameof(OBeautifulCode.Representation)}.{nameof(AssemblyRepresentation)}: Name = {this.Name?.ToString() ?? "<null>"}), Version = {this.Version?.ToString() ?? "<null>"}), FilePath = {this.FilePath?.ToString() ?? "<null>"}), FrameworkVersion = {this.FrameworkVersion?.ToString() ?? "<null>"}).");

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
