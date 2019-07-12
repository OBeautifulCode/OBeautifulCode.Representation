// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodCallExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="MethodCallExpression" />.
    /// </summary>
    public class MethodCallExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="MethodCallExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="parentObject">The object.</param>
        /// <param name="method">The method.</param>
        /// <param name="arguments">The arguments.</param>
        public MethodCallExpressionDescription(
            TypeDescription type,
            ExpressionType nodeType,
            ExpressionDescriptionBase parentObject,
            MethodInfoDescription method,
            IReadOnlyList<ExpressionDescriptionBase> arguments)
        : base(type, nodeType)
        {
            this.ParentObject = parentObject;
            this.Method = method;
            this.Arguments = arguments;
        }

        /// <summary>Gets the object.</summary>
        /// <value>The object.</value>
        public ExpressionDescriptionBase ParentObject { get; private set; }

        /// <summary>Gets the method hash.</summary>
        /// <value>The method hash.</value>
        public MethodInfoDescription Method { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionDescriptionBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MethodCallExpressionDescription" />.
                              /// </summary>
    public static class MethodCallExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="methodCallExpression">The methodCall expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MethodCallExpressionDescription ToDescription(this MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null)
            {
                throw new ArgumentNullException(nameof(methodCallExpression));
            }

            var type = methodCallExpression.Type.ToDescription();
            var nodeType = methodCallExpression.NodeType;
            var parentObject = methodCallExpression.Object.ToDescription();
            var method = methodCallExpression.Method.ToDescription();
            var parameters = methodCallExpression.Arguments.ToDescription();

            var result = new MethodCallExpressionDescription(type, nodeType, parentObject, method, parameters);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="methodCallExpressionDescription">The methodCall expression.</param>
        /// <returns>Converted expression.</returns>
        public static MethodCallExpression FromDescription(this MethodCallExpressionDescription methodCallExpressionDescription)
        {
            if (methodCallExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(methodCallExpressionDescription));
            }

            var instance = methodCallExpressionDescription.ParentObject.FromDescription();
            var method = methodCallExpressionDescription.Method.FromDescription();
            var arguments = methodCallExpressionDescription.Arguments.FromDescription();
            var result = Expression.Call(
                instance,
                method,
                arguments);

            return result;
        }
    }
}
