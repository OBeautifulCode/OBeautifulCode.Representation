// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewExpressionDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Description of <see cref="NewExpression" />.
    /// </summary>
    public class NewExpressionDescription : ExpressionDescriptionBase
    {
        /// <summary>Initializes a new instance of the <see cref="NewExpressionDescription"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorInfo">The constructor info.</param>
        /// <param name="arguments">The arguments.</param>
        public NewExpressionDescription(
            TypeDescription type,
            ConstructorInfoDescription constructorInfo,
            IReadOnlyList<ExpressionDescriptionBase> arguments)
            : base(type, ExpressionType.New)
        {
            this.ConstructorInfo = constructorInfo;
            this.Arguments = arguments;
        }

        /// <summary>Gets the constructor hash.</summary>
        /// <value>The constructor hash.</value>
        public ConstructorInfoDescription ConstructorInfo { get; private set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IReadOnlyList<ExpressionDescriptionBase> Arguments { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    /// <summary>
    /// Extensions to <see cref="NewExpressionDescription" />.
    /// </summary>
    public static class NewExpressionDescriptionExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="newExpression">The new expression.</param>
        /// <returns>Serializable expression.</returns>
        public static NewExpressionDescription ToDescription(this NewExpression newExpression)
        {
            if (newExpression == null)
            {
                throw new ArgumentNullException(nameof(newExpression));
            }

            var type = newExpression.Type.ToDescription();
            var constructorInfoDescription = newExpression.Constructor.ToDescription();
            var arguments = newExpression.Arguments.ToDescription();
            var result = new NewExpressionDescription(type, constructorInfoDescription, arguments);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="newExpressionDescription">The new expression.</param>
        /// <returns>Converted expression.</returns>
        public static NewExpression FromDescription(this NewExpressionDescription newExpressionDescription)
        {
            if (newExpressionDescription == null)
            {
                throw new ArgumentNullException(nameof(newExpressionDescription));
            }

            var type = newExpressionDescription.Type.ResolveFromLoadedTypes();

            NewExpression result;
            if (newExpressionDescription.ConstructorInfo != null)
            {
                var constructor = type.GetConstructors().Single(_ => _.ToDescription().Equals(newExpressionDescription.ConstructorInfo));
                var arguments = newExpressionDescription.Arguments.FromDescription();
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
