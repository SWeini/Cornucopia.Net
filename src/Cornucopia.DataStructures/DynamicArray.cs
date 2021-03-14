﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

using Cornucopia.DataStructures.Utils;

namespace Cornucopia.DataStructures
{
    /// <summary>
    ///     A dynamic list of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the list.</typeparam>
    [DebuggerTypeProxy(typeof(DynamicArray<>.DebuggerView))]
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct DynamicArray<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicArray{T}"/> struct that is empty. 
        /// </summary>
        /// <param name="initialize">Must be <c>true</c>.</param>
        /// <remarks>This constructor is a workaround to non-existing default constructors for value types.</remarks>
        public DynamicArray(bool initialize)
        {
            Debug.Assert(initialize);
            this._array = ArrayTools.Empty<T>();
            this.Length = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicArray{T}"/> struct with a specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public DynamicArray(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this._array = capacity == 0 ? ArrayTools.Empty<T>() : new T[capacity];
            this.Length = 0;
        }

        private T[] _array;

        /// <summary>
        ///     Gets the number of elements contained in the list.
        /// </summary>
        /// <value>The number of elements in the list.</value>
        public int Length { get; private set; }

        /// <summary>
        ///     Gets or sets the capacity of this list.
        /// </summary>
        /// <value>The capacity of this list.</value>
        public int Capacity
        {
            readonly get => this._array.Length;
            set
            {
                if (value < this.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (value != this._array.Length)
                {
                    if (value > 0)
                    {
                        var newItems = new T[value];
                        if (this.Length > 0)
                        {
                            Array.Copy(this._array, newItems, this.Length);
                        }
                        this._array = newItems;
                    }
                    else
                    {
                        this._array = ArrayTools.Empty<T>();
                    }
                }
            }
        }

        /// <summary>
        ///     Gets a reference to an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element reference to get.</param>
        /// <returns>The element reference at the specified index.</returns>
        /// <remarks>The reference is guaranteed to be valid as long as the list does not change it's <see cref="Length"/>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        public readonly ref T this[int index]
        {
            get
            {
                if ((uint) index >= (uint) this.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return ref this._array[index];
            }
        }

        /// <summary>
        ///     Clears the contents of the list.
        /// </summary>
        public void Clear()
        {
            if (Helpers.MayBeReferenceOrContainReferences<T>())
            {
                var size = this.Length;
                this.Length = 0;
                if (size > 0)
                {
                    Array.Clear(this._array, 0, size);
                }
            }
            else
            {
                this.Length = 0;
            }
        }

        /// <summary>
        ///     Appends a specified element to the list.
        /// </summary>
        /// <param name="item">The element to append to the list.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLast(T item)
        {
            var array = this._array;
            var size = this.Length;
            if (size < array.Length)
            {
                array[size] = item;
                this.Length = size + 1;
            }
            else
            {
                this.AddWithResize(item);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddWithResize(T item)
        {
            var size = this.Length;
            var newSize = GrowingCapacity.Grow(size, size + 1);
            var newItems = new T[newSize];
            if (size > 0)
            {
                Array.Copy(this._array, newItems, this.Length);
            }

            this._array = newItems;
            newItems[size] = item;
            this.Length = size + 1;
        }

        /// <summary>
        ///     Extracts the last element from the list.
        /// </summary>
        /// <returns>The last element of the list.</returns>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public T RemoveLast()
        {
            var newLength = this.Length - 1;
            if (newLength < 0)
            {
                throw new InvalidOperationException("The list is empty.");
            }

            var result = this._array[newLength];
            this.Length = newLength;
            if (Helpers.MayBeReferenceOrContainReferences<T>())
            {
                this._array[newLength] = default!;
            }

            return result;
        }

        /// <summary>
        ///     Gets a span representing the contents of the list.
        /// </summary>
        /// <returns>The span representing the contents of the list.</returns>
        /// <remarks>The span is guaranteed to be valid as long as the list does not change it's <see cref="Length"/>.</remarks>
        [Pure]
        public readonly Span<T> AsSpan()
        {
            return new(this._array, 0, this.Length);
        }

        [ExcludeFromCodeCoverage]
        private readonly string DebuggerDisplay => $"{{{TypeFormatter.Format(typeof(T))}[{this.Length} + {this.Capacity - this.Length}C]}}";

        [ExcludeFromCodeCoverage]
        private class DebuggerView
        {
            private readonly DynamicArray<T> _array;

            public DebuggerView(DynamicArray<T> array)
            {
                this._array = array;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items => this._array.AsSpan().ToArray();
        }
    }
}