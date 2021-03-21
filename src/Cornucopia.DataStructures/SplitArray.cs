using System;

namespace Cornucopia.DataStructures
{
    /// <summary>
    ///     A jagged array in compressed form.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the array.</typeparam>
    public readonly struct SplitArray<T>
    {
        private readonly T[] _values;
        private readonly int[] _firsts;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SplitArray{T}"/> structure with values and boundaries.
        /// </summary>
        /// <param name="values">The values of the jagged array.</param>
        /// <param name="firsts">The indices where the sub-arrays start.</param>
        public SplitArray(T[] values, int[] firsts)
        {
            this._values = values;
            this._firsts = firsts;
        }

        /// <summary>
        ///     Gets the number of sub-arrays in the array.
        /// </summary>
        /// <value>The number of sub-arrays in the array.</value>
        public int Length => this._firsts.Length - 1;

        /// <summary>
        ///     Gets a span representing the contents of a sub-array.
        /// </summary>
        /// <param name="index">The index of the sub-array to get.</param>
        /// <returns>The span representing the contents of a sub-array.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        public ReadOnlySpan<T> this[int index]
        {
            get
            {
                var start = this._firsts[index];
                var end = this._firsts[index + 1];
                return new ReadOnlySpan<T>(this._values, start, end - start);
            }
        }
    }
}
