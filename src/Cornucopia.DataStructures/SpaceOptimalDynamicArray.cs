using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cornucopia.DataStructures
{
    [DebuggerTypeProxy(typeof(SpaceOptimalDynamicArray<>.DebuggerView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SpaceOptimalDynamicArray<T> : IReadOnlyList<T>
    {
        public SpaceOptimalDynamicArray()
        {
#if NETCOREAPP3_1
            _blocks = Array.Empty<T[]>();
#else
            _blocks = new T[0][];
#endif
        }

        private T[]?[] _blocks;

        private int _count;

        public int Count => this._count;

        public ref T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)this._count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                LocateIndex(index, out var blockIndex, out var elementIndex, out _);
                return ref this._blocks[blockIndex]![elementIndex];
            }
        }

        [ExcludeFromCodeCoverage]
        T IReadOnlyList<T>.this[int index] => this[index];

        public void Add(T item)
        {
            LocateIndex(this._count, out var blockIndex, out var elementIndex, out var blockSize);
            if (blockIndex == this._blocks.Length)
            {
                if (this._blocks.Length == 0)
                {
                    this._blocks = new T[1][];
                }
                else
                {
                    Array.Resize(ref this._blocks, this._blocks.Length * 2);
                }
            }

            var block = this._blocks[blockIndex] ??= new T[blockSize];
            block[elementIndex] = item;
            this._count++;
        }

        public void Grow()
        {
            LocateIndex(this._count, out var blockIndex, out _, out var blockSize);
            if (blockIndex == this._blocks.Length)
            {
                if (this._blocks.Length == 0)
                {
                    this._blocks = new T[1][];
                }
                else
                {
                    Array.Resize(ref this._blocks, this._blocks.Length * 2);
                }
            }

            this._blocks[blockIndex] ??= new T[blockSize];
            this._count++;
        }

        public void Shrink()
        {
            if (this._count == 0)
            {
                throw new InvalidOperationException("Collection is empty");
            }

            LocateIndex(this._count - 1, out var blockIndex, out var elementIndex, out _);

            if (elementIndex == 0)
            {
                if (blockIndex + 1 < this._blocks.Length)
                {
                    this._blocks[blockIndex + 1] = null;
                    if (blockIndex * 4 == this._blocks.Length)
                    {
                        Array.Resize(ref this._blocks, blockIndex * 2);
                    }
                }
            }

            this._count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void LocateIndex(int index, out int blockIndex, out int elementIndex, out int blockSize)
        {
            var r = (uint)index + 1;
            var k = BitOperations.Log2(r);
            var hi = (k + 1) / 2;
            var lo = k / 2;
            var b = r >> hi & ((1u << lo) - 1);
            var p = ((1u << hi) - 1) + ((1u << lo) - 1);
            var e = r & ((1u << hi) - 1);
            blockIndex = (int)(b + p);
            elementIndex = (int)e;
            blockSize = 1 << hi;
        }

        [ExcludeFromCodeCoverage]
        private class DebuggerView
        {
            private readonly SpaceOptimalDynamicArray<T> _array;

            public DebuggerView(SpaceOptimalDynamicArray<T> array)
            {
                _array = array;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebuggerViewSlice[] Items
            {
                get
                {
                    var list = new List<DebuggerViewSlice>();
                    var count = _array.Count;
                    var offset = 0;
                    foreach (var block in _array._blocks)
                    {
                        if (count == 0)
                        {
                            break;
                        }

                        var blockSize = Math.Min(count, block!.Length);
                        list.Add(new DebuggerViewSlice(block, offset, blockSize));
                        count -= blockSize;
                        offset += blockSize;
                    }

                    return list.ToArray();
                }
            }
        }

        [DebuggerDisplay("Count = {" + nameof(Count) + "}", Name = "[{" + nameof(Indices) + ",nq}]")]
        [ExcludeFromCodeCoverage]
        private class DebuggerViewSlice
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly T[] _array;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly int _offset;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly int _length;

            public DebuggerViewSlice(T[] array, int offset, int length)
            {
                _array = array;
                _offset = offset;
                _length = length;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebuggerViewEntry[] Items
            {
                get
                {
                    return _array.Take(_length).Select((x, i) => new DebuggerViewEntry(x, i + _offset)).ToArray();
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Indices => $"{this._offset}..{this._offset + this._length - 1}";

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Count => _length;
        }

        [DebuggerDisplay("{" + nameof(Value) + "}", Name = "[{" + nameof(Index) + "}]")]
        [ExcludeFromCodeCoverage]
        private class DebuggerViewEntry
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
            public T Value { get; }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Index { get; }

            public DebuggerViewEntry(T value, int index)
            {
                Value = value;
                Index = index;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var blocks = this._blocks;
            var count = this._count;

            foreach (var block in blocks)
            {
                if (block != null)
                {
                    foreach (var item in block)
                    {
                        if (count-- > 0)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}