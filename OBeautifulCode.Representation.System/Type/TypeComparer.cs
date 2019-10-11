// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeComparer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Type comparer using the provided strategy.
    /// </summary>
    public class TypeComparer : IEqualityComparer<Type>, IEqualityComparer<TypeRepresentation>
    {
        /// <summary>
        /// The strategy to use when comparing types.
        /// </summary>
        private readonly TypeMatchStrategy typeMatchStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeComparer"/> class.
        /// </summary>
        /// <param name="typeMatchStrategy">Strategy for use when matching the type.</param>
        public TypeComparer(
            TypeMatchStrategy typeMatchStrategy)
        {
            this.typeMatchStrategy = typeMatchStrategy;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "These parameter names are better.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "These parameter names are better.")]
        public bool Equals(
            Type first,
            Type second)
        {
            if (first == null || second == null)
            {
                return false;
            }

            var result = first.ToRepresentation().Equals(second.ToRepresentation());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "These parameter names are better.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "These parameter names are better.")]
        public bool Equals(
            TypeRepresentation first,
            TypeRepresentation second)
        {
            if (first == null || second == null)
            {
                return false;
            }

            var result = this.Equals(
                first.Namespace,
                first.Name,
                first.AssemblyQualifiedName,
                second.Namespace,
                second.Name,
                second.AssemblyQualifiedName);
            return result;
        }

        /// <summary>
        /// Equals compare that takes raw strings instead of types.
        /// </summary>
        /// <param name="firstNamespace">Namespace of the first type.</param>
        /// <param name="firstName">Name of the first type.</param>
        /// <param name="firstAssemblyQualifiedName">AssemblyQualifiedName of the first type.</param>
        /// <param name="secondNamespace">Namespace of the second type.</param>
        /// <param name="secondName">Name of the second type.</param>
        /// <param name="secondAssemblyQualifiedName">AssemblyQualifiedName of the second type.</param>
        /// <returns>True for equality and false otherwise.</returns>
        public bool Equals(
            string firstNamespace,
            string firstName,
            string firstAssemblyQualifiedName,
            string secondNamespace,
            string secondName,
            string secondAssemblyQualifiedName)
        {
            bool result;
            switch (this.typeMatchStrategy)
            {
                case TypeMatchStrategy.NamespaceAndName:
                    result = firstNamespace == secondNamespace && firstName == secondName;
                    break;
                case TypeMatchStrategy.AssemblyQualifiedName:
                    if (firstAssemblyQualifiedName == null || secondAssemblyQualifiedName == null)
                    {
                        throw new ArgumentException(
                            "Type(s) AssemblyQualifiedName property was null so catch use matching strategy: AssemblyQualifiedName");
                    }

                    result = firstAssemblyQualifiedName == secondAssemblyQualifiedName;
                    break;
                default:
                    throw new ArgumentException("Unsupported matching strategy: " + this.typeMatchStrategy);
            }

            return result;
        }

        /// <inheritdoc />
        public int GetHashCode(
            TypeRepresentation obj)
        {
            new { obj }.AsArg().Must().NotBeNull();

            switch (this.typeMatchStrategy)
            {
                case TypeMatchStrategy.NamespaceAndName:
                    return (obj.Namespace + "." + obj.Name).GetHashCode();
                case TypeMatchStrategy.AssemblyQualifiedName:
                    if (obj.AssemblyQualifiedName == null)
                    {
                        throw new ArgumentException(
                            "Type's AssemblyQualifiedName property was null so catch use matching strategy: AssemblyQualifiedName");
                    }

                    return obj.AssemblyQualifiedName.GetHashCode();
                default:
                    throw new ArgumentException("Unsupported matching strategy: " + this.typeMatchStrategy);
            }
        }

        /// <inheritdoc />
        public int GetHashCode(
            Type obj)
        {
            return this.GetHashCode(obj.ToRepresentation());
        }
    }
}