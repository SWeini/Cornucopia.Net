using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cornucopia.DataStructures
{
    /// <summary>
    ///     A min-heap
    /// </summary>
    /// <typeparam name="T">The type of elements to store in the heap.</typeparam>
    public class BinaryHeap<T>
    {
        private readonly IComparer<T> _comparer;
        private DynamicArray<T> _elements;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class that is empty and uses the default comparer.
        /// </summary>
        public BinaryHeap()
        {
            this._comparer = Comparer<T>.Default;
            this._elements = new DynamicArray<T>(true);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class that is empty.
        /// </summary>
        /// <param name="comparer">The comparer used to compare elements.</param>
        public BinaryHeap(IComparer<T> comparer)
        {
            this._comparer = comparer;
            this._elements = new DynamicArray<T>(true);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class with a specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the heap.</param>
        /// <param name="comparer">The comparer used to compare elements.</param>
        public BinaryHeap(int capacity, IComparer<T> comparer)
        {
            this._comparer = comparer;
            this._elements = new DynamicArray<T>(capacity);
        }

        /// <summary>
        ///     Gets the number of elements contained in the heap.
        /// </summary>
        /// <value>The number of elements in the heap.</value>
        public int Count => this._elements.Length;

        /// <summary>
        ///     Inserts an element into the heap.
        /// </summary>
        /// <param name="item">The element to insert into the heap.</param>
        public void Insert(T item)
        {
            var idx = this._elements.Length;
            this._elements.AddLast(item);
            BinaryHeapUtilities.Up(this._elements.AsSpan(), idx, this._comparer);
        }

        /// <summary>
        ///     A minimum element contained in the heap.
        /// </summary>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T Minimum
        {
            get
            {
                if (this._elements.Length == 0)
                {
                    throw new InvalidOperationException("The heap is empty.");
                }

                return this._elements[0];
            }
        }

        /// <summary>
        ///     Extracts a minimum element from the heap.
        /// </summary>
        /// <returns>A minimum element that was contained in the heap.</returns>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T ExtractMinimum()
        {
            if (!this.TryExtractMinimum(out var result))
            {
                throw new InvalidOperationException("The heap is empty.");
            }

            return result;
        }

        /// <summary>
        ///     Extracts a minimum element from the heap.
        /// </summary>
        /// <param name="result">A minimum element that was contained in the heap.</param>
        /// <returns><c>true</c> if an element was removed from the heap; otherwise, <c>false</c>.</returns>
        public bool TryExtractMinimum([MaybeNullWhen(false)] out T result)
        {
            if (this._elements.Length == 0)
            {
                result = default;
                return false;
            }

            result = this.RemoveAt(0);
            return true;
        }

        private T RemoveAt(int idx)
        {
            var item = this._elements[idx];
            var lastItem = this._elements.RemoveLast();
            if (idx < this._elements.Length)
            {
                this._elements[idx] = lastItem;
                BinaryHeapUtilities.Down(this._elements.AsSpan(), idx, this._comparer);
            }

            return item;
        }
    }
}