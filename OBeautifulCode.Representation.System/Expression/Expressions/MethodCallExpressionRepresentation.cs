﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodCallExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="MethodCallExpression" />.
    /// </summary>
    public class MethodCallExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MethodCallExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="parentObject">The object.</param>
        /// <param name="method">The method.</param>
        /// <param name="arguments">The arguments.</param>
        public MethodCallExpressionRepresentation(
            TypeRepresentation type,
            ExpressionType nodeType,
            ExpressionRepresentationBase parentObject,
            MethodInfoRepresentation method,
            IReadOnlyList<ExpressionRepresentationBase> arguments)
        : base(type, nodeType)
        {
            this.ParentObject = parentObject;
            this.Method = method;
            this.Arguments = arguments;
        }

        /// <summary>Gets the object.</summary>
        /// <value>The object.</value>
        public ExpressionRepresentationBase ParentObject { get; private set; }

        /// <summary>Gets the method hash.</summary>
        /// <value>The method hash.</value>
        public MethodInfoRepresentation Method { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionRepresentationBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MethodCallExpressionRepresentation" />.
                              /// </summary>
    public static class MethodCallExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="methodCallExpression">The methodCall expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MethodCallExpressionRepresentation ToRepresentation(this MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null)
            {
                throw new ArgumentNullException(nameof(methodCallExpression));
            }

            var type = methodCallExpression.Type.ToRepresentation();
            var nodeType = methodCallExpression.NodeType;
            var parentObject = methodCallExpression.Object.ToRepresentation();
            var method = methodCallExpression.Method.ToRepresentation();
            var parameters = methodCallExpression.Arguments.ToRepresentation();

            var result = new MethodCallExpressionRepresentation(type, nodeType, parentObject, method, parameters);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="methodCallExpressionRepresentation">The methodCall expression.</param>
        /// <returns>Converted expression.</returns>
        public static MethodCallExpression FromRepresentation(this MethodCallExpressionRepresentation methodCallExpressionRepresentation)
        {
            if (methodCallExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(methodCallExpressionRepresentation));
            }

            var instance = methodCallExpressionRepresentation.ParentObject.FromRepresentation();
            var method = methodCallExpressionRepresentation.Method.FromRepresentation();
            var arguments = methodCallExpressionRepresentation.Arguments.FromRepresentation();
            var result = Expression.Call(
                instance,
                method,
                arguments);

            return result;
        }
    }
}
