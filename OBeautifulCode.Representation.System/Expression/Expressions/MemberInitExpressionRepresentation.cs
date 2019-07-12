﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInitExpressionRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Representation of <see cref="MemberInitExpression" />.
    /// </summary>
    public class MemberInitExpressionRepresentation : ExpressionRepresentationBase
    {
        /// <summary>Initializes a new instance of the <see cref="MemberInitExpressionRepresentation"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="newExpressionRepresentation">The new expression.</param>
        /// <param name="bindings">The bindings.</param>
        public MemberInitExpressionRepresentation(TypeRepresentation type, NewExpressionRepresentation newExpressionRepresentation, IReadOnlyCollection<MemberBindingRepresentationBase> bindings)
            : base(type, ExpressionType.MemberInit)
        {
            this.NewExpressionRepresentation = newExpressionRepresentation;
            this.Bindings = bindings;
        }

        /// <summary>Gets the new expression.</summary>
        /// <value>The new expression.</value>
        public NewExpressionRepresentation NewExpressionRepresentation { get; private set; }

        /// <summary>Gets the bindings.</summary>
        /// <value>The bindings.</value>
        public IReadOnlyCollection<MemberBindingRepresentationBase> Bindings { get; private set; }
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
                              /// <summary>
                              /// Extensions to <see cref="MemberInitExpressionRepresentation" />.
                              /// </summary>
    public static class MemberInitExpressionRepresentationExtensions
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        /// <summary>Converts to serializable.</summary>
        /// <param name="memberInitExpression">The memberInit expression.</param>
        /// <returns>Serializable expression.</returns>
        public static MemberInitExpressionRepresentation ToRepresentation(this MemberInitExpression memberInitExpression)
        {
            if (memberInitExpression == null)
            {
                throw new ArgumentNullException(nameof(memberInitExpression));
            }

            var type = memberInitExpression.Type.ToRepresentation();
            var newExpression = memberInitExpression.NewExpression.ToRepresentation();
            var bindings = memberInitExpression.Bindings.ToRepresentation();
            var result = new MemberInitExpressionRepresentation(type, newExpression, bindings);
            return result;
        }

        /// <summary>From the serializable.</summary>
        /// <param name="memberInitExpressionRepresentation">The memberInit expression.</param>
        /// <returns>Converted expression.</returns>
        public static MemberInitExpression FromRepresentation(this MemberInitExpressionRepresentation memberInitExpressionRepresentation)
        {
            if (memberInitExpressionRepresentation == null)
            {
                throw new ArgumentNullException(nameof(memberInitExpressionRepresentation));
            }

            var newExpression = memberInitExpressionRepresentation.NewExpressionRepresentation.FromRepresentation();
            var bindings = memberInitExpressionRepresentation.Bindings.FromRepresentation();
            var result = Expression.MemberInit(newExpression, bindings);

            return result;
        }
    }
}