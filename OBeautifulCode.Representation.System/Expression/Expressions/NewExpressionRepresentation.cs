﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="NewExpression" />.
    /// </summary>
    public class NewExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="NewExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorInfo">The constructor info.</param>
        /// <param name="arguments">The arguments.</param>
        public NewExpressionRepresentation(
            TypeRepresentation type,
            ConstructorInfoRepresentation constructorInfo,
            IReadOnlyList<ExpressionRepresentationBase> arguments)
            : base(type, ExpressionType.New)
        {
            this.ConstructorInfo = constructorInfo;
            this.Arguments = arguments;
        }

        /// <summary>Gets the constructor hash.</summary>
        /// <value>The constructor hash.</value>
        public ConstructorInfoRepresentation ConstructorInfo { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionRepresentationBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="NewExpressionRepresentation" />.
    /// </summary>
    public static class NewExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="newExpression">The new expression.</param>
        /// <returns>Serializable expression.</returns>
        public static NewExpressionRepresentation ToRepresentation(this NewExpression newExpression)
        {
            if (newExpression == null)
            {
                throw new ArgumentNullException(nameof(newExpression));
            }

            var type = newExpression.Type.ToRepresentation();
            var constructorInfoRepresentation = newExpression.Constructor.ToRepresentation();
            var arguments = newExpression.Arguments.ToRepresentation();
            var result = new NewExpressionRepresentation(type, constructorInfoRepresentation, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="newExpressionRepresentation">The new expression.</param>
        /// <returns>Converted expression.</returns>
        public static NewExpression FromRepresentation(this NewExpressionRepresentation newExpressionRepresentation)
        {
            if (newExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(newExpressionRepresentation));
            }

            var type = newExpressionRepresentation.Type.ResolveFromLoadedTypes();

            NewExpression result;
            if (newExpressionRepresentation.ConstructorInfo != null)
            {
                var constructor = type.GetConstructors().Single(_ => _.ToRepresentation().Equals(newExpressionRepresentation.ConstructorInfo));
                var arguments = newExpressionRepresentation.Arguments.FromRepresentation();
                result = Expression.New(constructor, arguments);
            }
            else
            {
                result = Expression.New(type);
            }

            return result;
        }
    }
}
