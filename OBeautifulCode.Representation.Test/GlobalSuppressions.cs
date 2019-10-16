// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using global::System.Diagnostics.CodeAnalysis;

using OBeautifulCode.Representation.Test.Internal;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OBeautifulCode.Representation.Test", Justification = ObcSuppressBecause.CA1020_AvoidNamespacesWithFewTypes_OptimizeForLogicalGroupingOfTypes)]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OBeautifulCode.Representation.Test.AssemblyRepresentationTests", Justification = "Grouping construct for unit test runner.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OBeautifulCode.Representation.Test.ElementInitRepresentationTests", Justification = "Grouping construct for unit test runner.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OBeautifulCode.Representation.Test.TypeRepresentationTests", Justification = "Grouping construct for unit test runner.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "OBeautifulCode.Representation.Test.ConstantExpressionRepresentationTests", Justification = "Grouping construct for unit test runner.")]