// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Model object containing a representation of a type that can be serialized without knowledge of the type.
    /// </summary>
    public class TypeRepresentation : IModel<TypeRepresentation>
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
        /// Determines whether two objects of type <see cref="TypeRepresentation"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items are equal; otherwise false.</returns>
        public static bool operator ==(TypeRepresentation left, TypeRepresentation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = (left.Namespace ?? string.Empty).Equals(right.Namespace                         ?? string.Empty, StringComparison.Ordinal)
                      && (left.Name ?? string.Empty).Equals(right.Name                                   ?? string.Empty, StringComparison.Ordinal)
                      && (left.AssemblyQualifiedName ?? string.Empty).Equals(right.AssemblyQualifiedName ?? string.Empty, StringComparison.Ordinal)
                      && (left.GenericArguments ?? new TypeRepresentation[0]).SequenceEqualHandlingNulls(
                             right.GenericArguments ?? new TypeRepresentation[0]);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentation"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the operator.</param>
        /// <param name="right">The object to the right of the operator.</param>
        /// <returns>True if the two items not equal; otherwise false.</returns>
        public static bool operator !=(TypeRepresentation left, TypeRepresentation right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeRepresentation other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeRepresentation);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Namespace)
            .Hash(this.Name)
            .Hash(this.AssemblyQualifiedName)
            .HashElements(this.GenericArguments)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public TypeRepresentation DeepClone()
        {
            var result = new TypeRepresentation(
                                 this.Namespace?.Clone().ToString(),
                                 this.Name?.Clone().ToString(),
                                 this.AssemblyQualifiedName?.Clone().ToString(),
                                 this.GenericArguments?.Select(_ => _?.DeepClone()).ToList());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="namespace" />.
        /// </summary>
        /// <param name="namespace">The new <see cref="Namespace" />.</param>
        /// <returns>New <see cref="TypeRepresentation" /> using the specified <paramref name="namespace" /> for <see cref="Namespace" /> and a deep clone of every other property.</returns>
        public TypeRepresentation DeepCloneWithNamespace(string @namespace)
        {
            var result = new TypeRepresentation(
                                 @namespace,
                                 this.Name?.Clone().ToString(),
                                 this.AssemblyQualifiedName?.Clone().ToString(),
                                 this.GenericArguments?.Select(_ => _?.DeepClone()).ToList());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="name" />.
        /// </summary>
        /// <param name="name">The new <see cref="Name" />.</param>
        /// <returns>New <see cref="TypeRepresentation" /> using the specified <paramref name="name" /> for <see cref="Name" /> and a deep clone of every other property.</returns>
        public TypeRepresentation DeepCloneWithName(string name)
        {
            var result = new TypeRepresentation(
                                 this.Namespace?.Clone().ToString(),
                                 name,
                                 this.AssemblyQualifiedName?.Clone().ToString(),
                                 this.GenericArguments?.Select(_ => _?.DeepClone()).ToList());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="assemblyQualifiedName" />.
        /// </summary>
        /// <param name="assemblyQualifiedName">The new <see cref="AssemblyQualifiedName" />.</param>
        /// <returns>New <see cref="TypeRepresentation" /> using the specified <paramref name="assemblyQualifiedName" /> for <see cref="AssemblyQualifiedName" /> and a deep clone of every other property.</returns>
        public TypeRepresentation DeepCloneWithAssemblyQualifiedName(string assemblyQualifiedName)
        {
            var result = new TypeRepresentation(
                                 this.Namespace?.Clone().ToString(),
                                 this.Name?.Clone().ToString(),
                                 assemblyQualifiedName,
                                 this.GenericArguments?.Select(_ => _?.DeepClone()).ToList());
            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <paramref name="genericArguments" />.
        /// </summary>
        /// <param name="genericArguments">The new <see cref="GenericArguments" />.</param>
        /// <returns>New <see cref="TypeRepresentation" /> using the specified <paramref name="genericArguments" /> for <see cref="GenericArguments" /> and a deep clone of every other property.</returns>
        public TypeRepresentation DeepCloneWithGenericArguments(IReadOnlyList<TypeRepresentation> genericArguments)
        {
            var result = new TypeRepresentation(
                                 this.Namespace?.Clone().ToString(),
                                 this.Name?.Clone().ToString(),
                                 this.AssemblyQualifiedName?.Clone().ToString(),
                                 genericArguments);
            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"{nameof(OBeautifulCode.Representation)}.{nameof(TypeRepresentation)}: Namespace = {this.Namespace?.ToString() ?? "<null>"}, Name = {this.Name?.ToString() ?? "<null>"}, AssemblyQualifiedName = {this.AssemblyQualifiedName?.ToString() ?? "<null>"}, GenericArguments = {this.GenericArguments?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}