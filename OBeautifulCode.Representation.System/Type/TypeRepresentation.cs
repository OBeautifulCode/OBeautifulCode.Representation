// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;

    /// <summary>
    /// Model object containing a representation of a type that can be serialized without knowledge of the type.
    /// </summary>
    public class TypeRepresentation : IEquatable<TypeRepresentation>
    {
        /// <summary>
        /// The unknown type representation to use.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is in fact immutable.")]
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

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentation" /> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are equal; false otherwise.</returns>
        public static bool operator ==(
            TypeRepresentation left,
            TypeRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result =
                (left.AssemblyQualifiedName == right.AssemblyQualifiedName) &&
                (left.Namespace == right.Namespace) &&
                (left.Name == right.Name) &&
                left.GenericArguments.SequenceEqualHandlingNulls(right.GenericArguments);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentation" /> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two object are not equal; false otherwise.</returns>
        public static bool operator !=(
            TypeRepresentation left,
            TypeRepresentation right)
            => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.AssemblyQualifiedName)
                .Hash(this.Namespace)
                .Hash(this.Name)
                .HashElements(this.GenericArguments)
                .Value;
    }
}