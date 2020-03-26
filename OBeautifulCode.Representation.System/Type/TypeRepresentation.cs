// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

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
            new { @namespace }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { name }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { assemblyQualifiedName }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { genericArguments }.AsArg().Must().NotBeNull().And().NotContainAnyNullElements();

            this.Namespace = @namespace;
            this.Name = name;
            this.AssemblyQualifiedName = assemblyQualifiedName;
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
        /// Gets the qualified name of the assembly of the type.
        /// </summary>
        public string AssemblyQualifiedName { get; private set; }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        public IReadOnlyList<TypeRepresentation> GenericArguments { get; private set; }
    }
}