// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorInfoRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static global::System.FormattableString;

    /// <summary>
    /// Representation of <see cref="ConstructorInfo" />.
    /// </summary>
    public partial class ConstructorInfoRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInfoRepresentation"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorHash">The method hash.</param>
        public ConstructorInfoRepresentation(
            TypeRepresentation type,
            string constructorHash)
        {
            this.Type = type;
            this.ConstructorHash = constructorHash;
        }

        /// <summary>
        /// Gets the constructor hash.
        /// </summary>
        public string ConstructorHash { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Name/spelling is correct.")]
        public TypeRepresentation Type { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements

    /// <summary>
    /// Extensions to <see cref="ConstructorInfoRepresentation" />.
    /// </summary>
    public static class ConstructorInfoRepresentationExtensions
    {
        /// <summary>
        /// Gets the constructor hash.
        /// </summary>
        /// <param name="constructorInfo">The constructor information.</param>
        /// <returns>
        /// Hash of the constructor.
        /// </returns>
        public static string GetSignatureHash(
            this ConstructorInfo constructorInfo)
        {
            new { constructorInfo }.AsArg().Must().NotBeNull();

            var methodName = constructorInfo.Name;

            var generics = constructorInfo.IsGenericMethod ? string.Join(",", constructorInfo.GetGenericArguments().Select(_ => _.FullName)) : null;

            var genericsAddIn = generics == null ? string.Empty : Invariant($"<{generics}>");

            var parameters = string.Join(",", constructorInfo.GetParameters().Select(_ => Invariant($"{_.ParameterType}-{_.Name}")));

            var result = Invariant($"{methodName}{genericsAddIn}({parameters})");

            return result;
        }

        /// <summary>
        /// Converts to representation.
        /// </summary>
        /// <param name="constructorInfo">The constructor information.</param>
        /// <returns>
        /// Converted <see cref="ConstructorInfoRepresentation" />.
        /// </returns>
        public static ConstructorInfoRepresentation ToRepresentation(
            this ConstructorInfo constructorInfo)
        {
            new { constructorInfo }.AsArg().Must().NotBeNull();

            var type = constructorInfo.DeclaringType.ToRepresentation();

            var constructorHash = constructorInfo.GetSignatureHash();

            var result = new ConstructorInfoRepresentation(type, constructorHash);

            return result;
        }

        /// <summary>
        /// Converts from representation.
        /// </summary>
        /// <param name="constructorInfoRepresentation">The representation.</param>
        /// <returns>
        /// Converted <see cref="ConstructorInfo" />.
        /// </returns>
        public static ConstructorInfo FromRepresentation(
            this ConstructorInfoRepresentation constructorInfoRepresentation)
        {
            new { constructorInfoRepresentation }.AsArg().Must().NotBeNull();

            var type = constructorInfoRepresentation.Type.ResolveFromLoadedTypes();

            var results = type.GetConstructors()
                              .Where(_ => _.GetSignatureHash().Equals(constructorInfoRepresentation.ConstructorHash, StringComparison.OrdinalIgnoreCase))
                              .ToList();

            if (!results.Any())
            {
                throw new ArgumentException(Invariant($"Could not find a constructor that matched hash '{constructorInfoRepresentation.ConstructorHash}' on type '{type}'."));
            }

            if (results.Count > 1)
            {
                var foundAddIn = string.Join(",", results.Select(_ => _.ToString()));

                throw new ArgumentException(Invariant($"Found too many constructors that matched hash '{constructorInfoRepresentation.ConstructorHash}' on type '{type}'; {foundAddIn}."));
            }

            var result = results.Single();

            return result;
        }
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}