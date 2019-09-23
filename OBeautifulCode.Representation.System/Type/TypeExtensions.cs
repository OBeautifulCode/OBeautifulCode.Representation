﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Representation source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

#if !OBeautifulCodeRepresentationRecipesProject
namespace OBeautifulCode.Representation.Recipes
#else
namespace OBeautifulCode.Representation
#endif
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Extensions methods on type <see cref="Type"/>.
    /// </summary>
#if !OBeautifulCodeRepresentationRecipesProject
    internal
#else
    public
#endif
        static class TypeExtensions
    {
        private static readonly Regex GenericBracketsRegex = new Regex("<.*>", RegexOptions.Compiled);

        private static readonly CodeDomProvider CodeDomProvider = CodeDomProvider.CreateProvider("CSharp");

        private static readonly Dictionary<Type, string> Aliases = new Dictionary<Type, string>
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(void), "void" },
        };

        /// <summary>
        /// Gets a compilable, readability-optimized string representation of the specified type.
        /// </summary>
        /// <remarks>
        /// Adapted from: <a href="https://stackoverflow.com/a/6402967/356790" />.
        /// Adapted from: <a href="https://stackoverflow.com/questions/1362884/is-there-a-way-to-get-a-types-alias-through-reflection" />.
        /// Helpful breakdown of generics: <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenerictype" />.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <param name="throwIfNoCompilableStringExists">Optional value indicating whether to throw a <see cref="NotSupportedException"/> if there's no compilable representation of the specified type.</param>
        /// <returns>
        /// A compilable, readability-optimized string representation of the specified type
        /// OR
        /// null if there is no compilable representation and <paramref name="throwIfNoCompilableStringExists"/> is true.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="NotSupportedException"><paramref name="throwIfNoCompilableStringExists"/> is true and <paramref name="type"/> is a generic open constructed type, which is not supported.</exception>
        /// <exception cref="NotSupportedException"><paramref name="throwIfNoCompilableStringExists"/> is true and <paramref name="type"/> is a generic parameter.</exception>
        public static string ToStringCompilable(
            this Type type,
            bool throwIfNoCompilableStringExists = false)
        {
            new { type }.Must().NotBeNull();

            string result;

            if (type.IsAnonymous())
            {
                if (throwIfNoCompilableStringExists)
                {
                    throw new NotSupportedException("Anonymous types are not supported.");
                }
                else
                {
                    result = null;
                }
            }
            else if (type.IsGenericParameter)
            {
                if (throwIfNoCompilableStringExists)
                {
                    // note that IsGenericParameter and ContainsGenericParameters will return true for generic parameters,
                    // hence we order the IsGenericParameter check first.
                    throw new NotSupportedException("Generic parameters not supported.");
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                if (Aliases.ContainsKey(type))
                {
                    result = Aliases[type];
                }
                else if (type.IsNullableType())
                {
                    result = Nullable.GetUnderlyingType(type).ToStringCompilable() + "?";
                }
                else if (type.IsArray)
                {
                    result = type.GetElementType().ToStringCompilable() + "[]";
                }
                else
                {
                    result = CodeDomProvider.GetTypeOutput(new CodeTypeReference(type.FullName?.Replace(type.Namespace + ".", string.Empty)));

                    if (type.IsGenericType)
                    {
                        if (type.IsGenericTypeDefinition)
                        {
                            result = result.Replace(" ", string.Empty);
                        }
                        else if (type.ContainsGenericParameters)
                        {
                            if (throwIfNoCompilableStringExists)
                            {
                                throw new NotSupportedException("Generic open constructed types are not supported.");
                            }
                            else
                            {
                                result = null;
                            }
                        }
                        else
                        {
                            var genericParameters = type.GetGenericArguments().Select(_ => _.ToStringCompilable()).ToArray();

                            result = GenericBracketsRegex.Replace(result, "<" + string.Join(", ", genericParameters) + ">");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a readability-optimized string representation of the specified type.
        /// </summary>
        /// <remarks>
        /// Adapted from: <a href="https://stackoverflow.com/a/6402967/356790" />.
        /// Adapted from: <a href="https://stackoverflow.com/questions/1362884/is-there-a-way-to-get-a-types-alias-through-reflection" />.
        /// Helpful breakdown of generics: <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenerictype" />.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A readability-optimized string representation of the specified type
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public static string ToStringReadable(
            this Type type)
        {
            // A copy of this method exists in OBC.Validation.
            // Any bug fixes made here should also be applied to OBC.Validation.
            // OBC.Validation cannot take a reference to OBC.Representation because it creates a circular reference
            // since OBC.Representation itself depends on OBC.Validation.
            new { type }.Must().NotBeNull();

            string result;

            if (type.IsGenericParameter)
            {
                result = type.Name;
            }
            else
            {
                if (Aliases.ContainsKey(type))
                {
                    result = Aliases[type];
                }
                else if (type.IsNullableType())
                {
                    result = Nullable.GetUnderlyingType(type).ToStringReadable() + "?";
                }
                else if (type.IsArray)
                {
                    result = type.GetElementType().ToStringReadable() + "[]";
                }
                else
                {
                    result = CodeDomProvider.GetTypeOutput(new CodeTypeReference(type.FullName?.Replace(type.Namespace + ".", string.Empty) ?? type.Name));

                    if (type.IsAnonymous())
                    {
                        result = result.Replace("<>f__", string.Empty);
                    }

                    if (type.IsGenericType)
                    {
                        var genericParameters = type.GetGenericArguments().Select(_ => _.ToStringReadable()).ToArray();

                        result = GenericBracketsRegex.Replace(result, "<" + string.Join(", ", genericParameters) + ">");
                    }
                }
            }

            return result;
        }
    }
}
