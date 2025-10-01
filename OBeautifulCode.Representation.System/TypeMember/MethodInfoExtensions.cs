// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodInfoExtensions.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Linq;
    using global::System.Reflection;
    using static global::System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="MethodInfo" />.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Gets the method hash.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>
        /// Hash of the method.
        /// </returns>
        public static string GetSignatureHash(
            this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodName = methodInfo.Name;

            var generics = methodInfo.IsGenericMethod ? string.Join(",", methodInfo.GetGenericArguments().Select(_ => _.FullName)) : null;

            var genericsAddIn = generics == null ? string.Empty : Invariant($"<{generics}>");

            var parameters = string.Join(",", methodInfo.GetParameters().Select(_ => Invariant($"{_.ParameterType}-{_.Name}")));

            var result = Invariant($"{methodName}{genericsAddIn}({parameters})");

            return result;
        }
    }
}