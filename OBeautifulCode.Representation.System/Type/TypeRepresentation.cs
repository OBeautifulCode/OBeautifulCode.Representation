// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Reflection;

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
        /// <param name="genericArguments">Generic arguments if any.  Use null if the type is not generic.  Specify an empty set for a generic type definition.  Other open types are not supported.</param>
        public TypeRepresentation(
            string @namespace,
            string name,
            string assemblyName,
            string assemblyVersion,
            IReadOnlyList<TypeRepresentation> genericArguments)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentException(Invariant($"'{nameof(@namespace)}' is white space"));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Invariant($"'{nameof(name)}' is white space"));
            }

            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentException(Invariant($"'{nameof(assemblyName)}' is white space"));
            }

            if (assemblyVersion != null)
            {
                if (string.IsNullOrWhiteSpace(assemblyVersion))
                {
                    throw new ArgumentException(Invariant($"'{nameof(assemblyVersion)}' is not null, but white space"));
                }
            }

            if (genericArguments != null)
            {
                if (genericArguments.Any(_ => _ == null))
                {
                    throw new ArgumentException(Invariant($"'{nameof(genericArguments)}' contains at least one null element"));
                }
            }

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
        /// Gets the assembly qualified name.
        /// </summary>
        /// <returns>
        /// The assembly qualified name of the type.
        /// </returns>
        public string BuildAssemblyQualifiedName()
        {
            var assemblyName = this.BuildAssemblyName();

            var genericArgumentsQualifiedNames = this.GenericArguments?.Select(_ => "[" + _.BuildAssemblyQualifiedName() + "]").ToArray() ?? new string[0];

            var genericToken = genericArgumentsQualifiedNames.Any()
                ? Invariant($"[{string.Join(",", genericArgumentsQualifiedNames)}]")
                : string.Empty;

            string result;

            if (this.IsArray() && this.IsClosedGenericType())
            {
                var arraySpecifierStartIndex = this.Name.IndexOf('[');

                var nameWithGenericArguments = this.Name.Insert(arraySpecifierStartIndex, genericToken);

                result = Invariant($"{this.Namespace}.{nameWithGenericArguments}, {assemblyName.FullName}");
            }
            else
            {
                result = Invariant($"{this.Namespace}.{this.Name}{genericToken}, {assemblyName.FullName}");
            }

            return result;
        }

        /// <summary>
        /// Builds the <see cref="AssemblyName"/> of the assembly that contains the type being represented by this object.
        /// </summary>
        /// <returns>
        /// The <see cref="AssemblyName"/> of the assembly that contains the type being represented by this object.
        /// </returns>
        public AssemblyName BuildAssemblyName()
        {
            var versionToken = (this.AssemblyVersion != null)
                ? Invariant($", Version={this.AssemblyVersion}")
                : string.Empty;

            var assemblyName = Invariant($"{this.AssemblyName}{versionToken}");

            var result = new AssemblyName(assemblyName);

            return result;
        }

        /// <inheritdoc cref="IDeclareToStringMethod" />
        public override string ToString()
        {
            var result = this.BuildAssemblyQualifiedName();

            return result;
        }
    }
}