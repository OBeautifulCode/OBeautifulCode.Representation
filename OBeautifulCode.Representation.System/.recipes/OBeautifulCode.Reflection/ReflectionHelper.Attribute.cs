﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.Attribute.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Reflection.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Reflection.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Provides useful methods related to reflection.
    /// </summary>
#if !OBeautifulCodeReflectionRecipesProject
    internal
#else
    public
#endif
    static partial class ReflectionHelper
    {
        /// <summary>
        /// Gets the specified type of attribute, applied to a specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to return.</typeparam>
        /// <param name="type">The type to scope the attribute search to.</param>
        /// <returns>
        /// The attribute of type <typeparamref name="TAttribute"/> that has been applied
        /// to <paramref name="type"/> or null if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="type"/> has multiple attributes of type <typeparamref name="TAttribute"/>.  Consider calling <see cref="GetAttributes{T}(Type)"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GetAttributes", Justification = "This is spelled correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)", Justification = "This is a developer-facing string, not a user-facing string.")]
        public static TAttribute GetAttribute<TAttribute>(
            this Type type)
            where TAttribute : Attribute
        {
            new { type }.AsArg().Must().NotBeNull();

            var attributes = type.GetAttributes<TAttribute>();
            if (attributes.Count > 1)
            {
                throw new InvalidOperationException($"Type '{type}' has multiple attributes of type '{typeof(TAttribute)}'.  Consider calling {nameof(GetAttributes)}.");
            }

            var result = attributes.SingleOrDefault();
            return result;
        }

        /// <summary>
        /// Gets the specified type of attribute, applied to a specific enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to return.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <returns>
        /// An attribute object of the specified type that has been applied to the specified
        /// enum value or null if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumValue"/> is not an Enum.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="enumValue"/> has multiple attributes of type <typeparamref name="TAttribute"/>.  Consider calling <see cref="GetAttributesOnEnumValue{T}(Enum)"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GetAttributes", Justification = "This is spelled correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)", Justification = "This is a developer-facing string, not a user-facing string.")]
        public static TAttribute GetAttributeOnEnumValue<TAttribute>(
            this object enumValue)
            where TAttribute : Attribute
        {
            new { enumValue }.AsArg().Must().NotBeNull();
            var enumValueAsEnum = enumValue as Enum;
            (enumValueAsEnum != null).AsArg($"{nameof(enumValue)} is Enum").Must().BeTrue();

            var result = enumValueAsEnum.GetAttributeOnEnumValue<TAttribute>();
            return result;
        }

        /// <summary>
        /// Gets the specified type of attribute, applied to a specific enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to return.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <returns>
        /// An attribute object of the specified type that has been applied to the specified
        /// enum value or null if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="enumValue"/> has multiple attributes of type <typeparamref name="TAttribute"/>.  Consider calling <see cref="GetAttributesOnEnumValue{T}(Enum)"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GetAttributesOnEnumValue", Justification = "This is spelled correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)", Justification = "This is a developer-facing string, not a user-facing string.")]
        public static TAttribute GetAttributeOnEnumValue<TAttribute>(
            this Enum enumValue)
            where TAttribute : Attribute
        {
            new { enumValue }.AsArg().Must().NotBeNull();

            var attributes = enumValue.GetAttributesOnEnumValue<TAttribute>();
            if (attributes.Count > 1)
            {
                throw new InvalidOperationException($"Enum value '{enumValue}' has multiple attributes of type '{typeof(TAttribute)}'.  Consider calling {nameof(GetAttributesOnEnumValue)}.");
            }

            var result = attributes.SingleOrDefault();
            return result;
        }

        /// <summary>
        /// Gets all attributes of a specified type that have been applied to some type.
        /// Only useful when the attribute is configured such that more one instance can be applied.
        /// </summary>
        /// <remarks>
        /// adapted from <a href="http://stackoverflow.com/a/2656211/356790"/>.
        /// </remarks>
        /// <typeparam name="TAttribute">The type of the attributes to return.</typeparam>
        /// <param name="type">The type to scope the attribute search to.</param>
        /// <returns>
        /// A collection all attributes of the specified type that have been applied to the specified
        /// enum value or an empty collection if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public static IReadOnlyCollection<TAttribute> GetAttributes<TAttribute>(
            this Type type)
            where TAttribute : Attribute
        {
            new { type }.AsArg().Must().NotBeNull();

            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
            var result = attributes.Cast<TAttribute>().ToList().AsReadOnly();
            return result;
        }

        /// <summary>
        /// Gets all attributes of the specified type that have been applied to a specific enum value.
        /// Only useful when the attribute is configured such that more one instance can be applied to an enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attributes to return.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <returns>
        /// A collection all attributes of the specified type that have been applied to the specified
        /// enum value or an empty collection if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumValue"/> is not an Enum.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)", Justification = "This is a developer-facing string, not a user-facing string.")]
        public static IReadOnlyCollection<TAttribute> GetAttributesOnEnumValue<TAttribute>(
            this object enumValue)
            where TAttribute : Attribute
        {
            new { enumValue }.AsArg().Must().NotBeNull();
            var enumValueAsEnum = enumValue as Enum;
            (enumValueAsEnum != null).AsArg($"{nameof(enumValue)} is Enum").Must().BeTrue();

            var result = enumValueAsEnum.GetAttributesOnEnumValue<TAttribute>();
            return result;
        }

        /// <summary>
        /// Gets all attributes of the specified type that have been applied to a specific enum value.
        /// Only useful when the attribute is configured such that more one instance can be applied to an enum value.
        /// </summary>
        /// <remarks>
        /// adapted from <a href="http://stackoverflow.com/a/9276348/356790"/>.
        /// </remarks>
        /// <typeparam name="TAttribute">The type of the attributes to return.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <returns>
        /// A collection all attributes of the specified type that have been applied to the specified
        /// enum value or an empty collection if no such attribute has been applied.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        public static IReadOnlyCollection<TAttribute> GetAttributesOnEnumValue<TAttribute>(
            this Enum enumValue)
            where TAttribute : Attribute
        {
            new { enumValue }.AsArg().Must().NotBeNull();

            var type = enumValue.GetType();
            var member = type.GetMember(enumValue.ToString());
            var attributes = member[0].GetCustomAttributes(typeof(TAttribute), false);
            var result = attributes.Cast<TAttribute>().ToList().AsReadOnly();
            return result;
        }

        /// <summary>
        /// Determines if an attribute of a specified type that have been applied to some type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attributes to search for.</typeparam>
        /// <param name="type">The type to scope the attribute search to.</param>
        /// <param name="throwOnMultiple">
        /// Optional.  Determines if method should throw when multiple instances of the specified
        /// attribute have been applied to the specified type.  Default is true
        /// (it's typically unlikely that multiple attributes of the same type are applied to a type).
        /// </param>
        /// <returns>
        /// True if the attribute has been applied to the specified type, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="throwOnMultiple"/> is true and <paramref name="type"/> has multiple attributes of type <typeparamref name="TAttribute"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This mirrors the 'Get' methods")]
        public static bool HasAttribute<TAttribute>(
            this Type type,
            bool throwOnMultiple = true)
            where TAttribute : Attribute
        {
            bool result;
            if (throwOnMultiple)
            {
                result = GetAttribute<TAttribute>(type) != null;
            }
            else
            {
                result = GetAttributes<TAttribute>(type).Any();
            }

            return result;
        }

        /// <summary>
        /// Determines if an attribute of the specified type has been applied to a specific enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attributes to search for.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <param name="throwOnMultiple">
        /// Optional.  Determines if method should throw when multiple instances of the specified
        /// attribute have been applied to the specified enum value.  Default is true
        /// (it's typically unlikely that multiple attributes of the same type are applied to an enum value).
        /// </param>
        /// <returns>
        /// True if the attribute has been applied to the specified enum value, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumValue"/> is not an Enum.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="throwOnMultiple"/> is true and <paramref name="enumValue"/> has multiple attributes of type <typeparamref name="TAttribute"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This mirrors the 'Get' methods")]
        public static bool HasAttributeOnEnumValue<TAttribute>(
            this object enumValue,
            bool throwOnMultiple = true)
            where TAttribute : Attribute
        {
            bool result;
            if (throwOnMultiple)
            {
                result = GetAttributeOnEnumValue<TAttribute>(enumValue) != null;
            }
            else
            {
                result = GetAttributesOnEnumValue<TAttribute>(enumValue).Any();
            }

            return result;
        }

        /// <summary>
        /// Determines if an attribute of the specified type that has been applied to a specific enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attributes to serach for.</typeparam>
        /// <param name="enumValue">The enum value to scope the attribute search to.</param>
        /// <param name="throwOnMultiple">
        /// Optional.  Determines if method should throw when multiple instances of the specified
        /// attribute have been applied to the specified enum value.  Default is true
        /// (it's typically unlikely that multiple attributes of the same type are applied to an enum value).
        /// </param>
        /// <returns>
        /// True if the attribute has been applied to the specified enum value, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumValue"/> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This mirrors the 'Get' methods")]
        public static bool HasAttributeOnEnumValue<TAttribute>(
            this Enum enumValue,
            bool throwOnMultiple = true)
            where TAttribute : Attribute
        {
            bool result;
            if (throwOnMultiple)
            {
                result = GetAttributeOnEnumValue<TAttribute>(enumValue) != null;
            }
            else
            {
                result = GetAttributesOnEnumValue<TAttribute>(enumValue).Any();
            }

            return result;
        }
    }
}
