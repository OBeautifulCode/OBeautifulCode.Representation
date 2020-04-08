// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Model object containing a representation of a type that can be serialized without knowledge of the type.
    /// </summary>
    public partial class TypeRepresentation : IModelViaCodeGen, IDeclareToStringMethod
    {
        /// <summary>
        /// The unknown type representation to use.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is in fact immutable.")]
        public static readonly TypeRepresentation UnknownTypeRepresentation = typeof(UnknownTypePlaceholder).ToRepresentation();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepresentation" /> class.
        /// </summary>
        /// <param name="namespace">Namespace of type.</param>
        /// <param name="name">Name of type.</param>
        /// <param name="assemblyName">The simple name of the assembly. This is usually, but not necessarily, the file name of the manifest file of the assembly, minus its extension.</param>
        /// <param name="assemblyVersion">The major, minor, build, and revision numbers of the assembly.</param>
        /// <param name="genericArguments">Generic arguments if any.</param>
        public TypeRepresentation(
            string @namespace,
            string name,
            string assemblyName,
            string assemblyVersion,
            IReadOnlyList<TypeRepresentation> genericArguments)
        {
            new { @namespace }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { name }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { assemblyName }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { assemblyVersion }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { genericArguments }.AsArg().Must().NotBeNull().And().NotContainAnyNullElements();

            this.Namespace = @namespace;
            this.Name = name;
            this.AssemblyName = assemblyName;
            this.AssemblyVersion = assemblyVersion;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        /// Gets the namespace of the type.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the simple name of the assembly. This is usually, but not necessarily, the file name of the manifest file of the assembly, minus its extension.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the major, minor, build, and revision numbers of the assembly.
        /// </summary>
        public string AssemblyVersion { get; private set; }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        public IReadOnlyList<TypeRepresentation> GenericArguments { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this type is an array type.
        /// </summary>
        public bool IsArray => this.Name.EndsWith("[]") || this.Name.EndsWith("[*]") || this.Name.EndsWith(",]");

        /// <summary>
        /// Gets the assembly qualified name.
        /// </summary>
        /// <param name="includeVersion">A value indicating whether to include the version in the assembly qualified name.</param>
        /// <returns>
        /// The assembly qualified name of the type.
        /// </returns>
        public string BuildAssemblyQualifiedName(
            bool includeVersion = true)
        {
            var versionToken = includeVersion
                ? Invariant($", Version={this.AssemblyVersion}")
                : string.Empty;

            var genericArgumentsQualifiedNames = this.GenericArguments.Select(_ => "[" + _.BuildAssemblyQualifiedName(includeVersion) + "]").ToArray();

            var genericToken = this.GenericArguments.Any()
                ? Invariant($"[{string.Join(",", genericArgumentsQualifiedNames)}]")
                : string.Empty;

            string result;

            if (this.IsArray && this.GenericArguments.Any())
            {
                var arraySpecifierStartIndex = this.Name.IndexOf('[');

                var nameWithGenericArguments = this.Name.Insert(arraySpecifierStartIndex, genericToken);

                result = Invariant($"{this.Namespace}.{nameWithGenericArguments}, {this.AssemblyName}{versionToken}");
            }
            else
            {
                result = Invariant($"{this.Namespace}.{this.Name}{genericToken}, {this.AssemblyName}{versionToken}");
            }

            return result;
        }

        /// <inheritdoc cref="IDeclareToStringMethod" />
        public override string ToString()
        {
            var result = this.BuildAssemblyQualifiedName(includeVersion: true);

            return result;
        }
    }
}