// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorInfoExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq;
    using global::System.Reflection;
    using static global::System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="ConstructorInfo" />.
    /// </summary>
    public static class ConstructorInfoExtensions
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
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var methodName = constructorInfo.Name;

            var generics = constructorInfo.IsGenericMethod ? string.Join(",", constructorInfo.GetGenericArguments().Select(_ => _.FullName)) : null;

            var genericsAddIn = generics == null ? string.Empty : Invariant($"<{generics}>");

            var parameters = string.Join(",", constructorInfo.GetParameters().Select(_ => Invariant($"{_.ParameterType}-{_.Name}")));

            var result = Invariant($"{methodName}{genericsAddIn}({parameters})");

            return result;
        }
    }
}