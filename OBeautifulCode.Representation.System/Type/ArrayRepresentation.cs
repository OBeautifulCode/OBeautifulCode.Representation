// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayRepresentation.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Representation.System
{
    using global::System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Representation.System.Internal;

    /// <summary>
    /// Represents a vector array and is used to create a representation of a vector array type.
    /// </summary>
    /// <typeparam name="TElement">The array's element type.</typeparam>
    #pragma warning disable SA1649 // File name should match first type name
    public class VectorArray<TElement>
    {
    }
    #pragma warning restore SA1649 // File name should match first type name

    /// <summary>
    /// Represents a multidimensional array and is used to create a representation of a multidimensional array type.
    /// </summary>
    /// <typeparam name="TElement">The array's element type.</typeparam>
    /// <typeparam name="TArrayRank">The rank (dimensions) of the array.</typeparam>
    public class MultidimensionalArray<TElement, TArrayRank>
        where TArrayRank : IArrayRank
    {
    }

    /// <summary>
    /// The rank (dimensions) of an array.
    /// </summary>
#pragma warning disable SA1201 // Elements should appear in the correct order
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = ObcSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IArrayRank
    {
    }
    #pragma warning restore SA1201 // Elements should appear in the correct order

    /// <summary>
    /// A rank 1 array (1 dimension).
    /// </summary>
    public class ArrayRank1 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 2 array (2 dimensions).
    /// </summary>
    public class ArrayRank2 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 3 array (3 dimensions).
    /// </summary>
    public class ArrayRank3 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 4 array (4 dimensions).
    /// </summary>
    public class ArrayRank4 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 5 array (5 dimensions).
    /// </summary>
    public class ArrayRank5 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 6 array (6 dimensions).
    /// </summary>
    public class ArrayRank6 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 7 array (7 dimensions).
    /// </summary>
    public class ArrayRank7 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 8 array (8 dimensions).
    /// </summary>
    public class ArrayRank8 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 9 array (9 dimensions).
    /// </summary>
    public class ArrayRank9 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 10 array (10 dimensions).
    /// </summary>
    public class ArrayRank10 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 11 array (11 dimensions).
    /// </summary>
    public class ArrayRank11 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 12 array (12 dimensions).
    /// </summary>
    public class ArrayRank12 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 13 array (13 dimensions).
    /// </summary>
    public class ArrayRank13 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 14 array (14 dimensions).
    /// </summary>
    public class ArrayRank14 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 15 array (15 dimensions).
    /// </summary>
    public class ArrayRank15 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 16 array (16 dimensions).
    /// </summary>
    public class ArrayRank16 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 17 array (17 dimensions).
    /// </summary>
    public class ArrayRank17 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 18 array (18 dimensions).
    /// </summary>
    public class ArrayRank18 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 19 array (19 dimensions).
    /// </summary>
    public class ArrayRank19 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 20 array (20 dimensions).
    /// </summary>
    public class ArrayRank20 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 21 array (21 dimensions).
    /// </summary>
    public class ArrayRank21 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 22 array (22 dimensions).
    /// </summary>
    public class ArrayRank22 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 23 array (23 dimensions).
    /// </summary>
    public class ArrayRank23 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 24 array (24 dimensions).
    /// </summary>
    public class ArrayRank24 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 25 array (25 dimensions).
    /// </summary>
    public class ArrayRank25 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 26 array (26 dimensions).
    /// </summary>
    public class ArrayRank26 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 27 array (27 dimensions).
    /// </summary>
    public class ArrayRank27 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 28 array (28 dimensions).
    /// </summary>
    public class ArrayRank28 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 29 array (29 dimensions).
    /// </summary>
    public class ArrayRank29 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 30 array (30 dimensions).
    /// </summary>
    public class ArrayRank30 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 31 array (31 dimensions).
    /// </summary>
    public class ArrayRank31 : IArrayRank
    {
    }

    /// <summary>
    /// A rank 32 array (32 dimensions).
    /// </summary>
    public class ArrayRank32 : IArrayRank
    {
    }
}
