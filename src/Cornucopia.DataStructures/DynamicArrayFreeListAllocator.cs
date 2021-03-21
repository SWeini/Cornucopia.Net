using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Cornucopia.DataStructures.Utils;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [DebuggerTypeProxy(typeof(DynamicArrayFreeListAllocator<>.DebuggerView))]
    internal struct DynamicArrayFreeListAllocator<T>
    {
        private DynamicArray<T> _array;
        private DynamicArray<int> _freeList;

        public DynamicArrayFreeListAllocator(int capacity)
        {
            this._array = new DynamicArray<T>(capacity);
            this._freeList = new DynamicArray<int>(0);
        }

        public int Count => this._array.Length - this._freeList.Length;

        public ref T this[int index] => ref this._array[index];

        public int Add(T item)
        {
            if (this._freeList.TryRemoveLast(out var freeIndex))
            {
                this._array[freeIndex] = item;
                return freeIndex;
            }

            var index = this._array.Length;
            this._array.AddLast(item);
            return index;
        }

        public void RemoveAt(int index)
        {
            this._array[index] = default!;
            this._freeList.AddLast(index);
        }

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{{{TypeFormatter.Format(typeof(T))}[{this._array.Length} + {this._freeList.Length}F + {this._array.Capacity - this._array.Length}C]}}";

        [ExcludeFromCodeCoverage]
        private class DebuggerView
        {
            private DynamicArrayFreeListAllocator<T> _array;

            public DebuggerView(DynamicArrayFreeListAllocator<T> array)
            {
                this._array = array;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            [UsedImplicitly]
            public DebuggerItem[] Items
            {
                get
                {
                    var frees = new HashSet<int>(this._array._freeList.AsSpan().ToArray());
                    var result = new DebuggerItem[this._array.Count];
                    var j = 0;
                    for (var i = 0; i < this._array._array.Length; i++)
                    {
                        if (frees.Contains(i))
                        {
                            continue;
                        }

                        result[j++] = new DebuggerItem(i, this._array[i]);
                    }

                    return result;
                }
            }
        }

        [DebuggerDisplay("{" + nameof(Value) + "}", Name = "[{" + nameof(Index) + "}]")]
        [ExcludeFromCodeCoverage]
        private class DebuggerItem
        {
            public DebuggerItem(int index, T value)
            {
                this.Index = index;
                this.Value = value;
            }

            public int Index { get; }
            public T Value { get; }
        }
    }
}
