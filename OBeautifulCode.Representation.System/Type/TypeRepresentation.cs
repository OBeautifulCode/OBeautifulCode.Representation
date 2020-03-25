// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Model object containing a representation of a type that can be serialized without knowledge of the type.
    /// </summary>
    public partial class TypeRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// The unknown type representation to use.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is in fact immutable.")]
        public static readonly TypeRepresentation UnknownTypeRepresentation = typeof(UnknownTypePlaceholder).ToRepresentation();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepresentation" /> class.
        /// </summary>
        public TypeRepresentation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepresentation" /> class.
        /// </summary>
        /// <param name="namespace">Namespace of type.</param>
        /// <param name="name">Name of type.</param>
        /// <param name="assemblyQualifiedName">Assembly qualified name of type.</param>
        /// <param name="genericArguments">Generic arguments if any.</param>
        public TypeRepresentation(
            string @namespace,
            string name,
            string assemblyQualifiedName,
            IReadOnlyList<TypeRepresentation> genericArguments)
        {
            this.Namespace = @namespace;
            this.Name = name;
            this.AssemblyQualifiedName = assemblyQualifiedName;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        /// Gets or sets the namespace of the type.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the qualified name of the assembly of the type.
        /// </summary>
        /// <value>The name of the assembly qualified.</value>
        public string AssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets or sets the generic arguments.
        /// </summary>
        /// <value>The generic arguments.</value>
        public IReadOnlyList<TypeRepresentation> GenericArguments { get; set; }
    }
}