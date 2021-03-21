using System;
using System.Numerics;

namespace Cornucopia.DataStructures
{
    internal struct DynamicArrayBlockAllocator<T>
    {
        private DynamicArray<T> _array;
        private readonly DynamicArray<int>[] _freeLists;

        public DynamicArrayBlockAllocator(int capacity, int maxLogSize)
        {
            this._array = new DynamicArray<T>(capacity);
            this._freeLists = new DynamicArray<int>[maxLogSize + 1];
            for (var i = 0; i < this._freeLists.Length; i++)
            {
                this._freeLists[i] = new DynamicArray<int>(0);
            }
        }

        public ref T this[int index] => ref this._array[index];

        public Span<T> AsSpan(int start, int length) => this._array.AsSpan().Slice(start, length);

        public int Allocate(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            var sizeLog = LogSize(size);
            size = 1 << sizeLog;

            ref var freeList = ref this._freeLists[sizeLog];
            if (freeList.TryRemoveLast(out var index))
            {
                return index;
            }
            ref var nextFreeList = ref this._freeLists[sizeLog + 1];
            if (nextFreeList.TryRemoveLast(out index))
            {
                freeList.AddLast(index + size);
                return index;
            }

            index = this._array.Length;
            this._array.Length = index + size;
            return index;
        }

        public int Resize(int index, int oldSize, int size)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (oldSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(oldSize));
            }
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            var oldSizeLog = LogSize(oldSize);
            var sizeLog = LogSize(size);
            if (oldSizeLog == sizeLog)
            {
                return index;
            }

            var newIndex = this.Allocate(size);
            var span = this._array.AsSpan();
            span.Slice(index, oldSize).CopyTo(span.Slice(newIndex, oldSize));
            this.Deallocate(index, oldSize);
            return newIndex;
        }

        public void Deallocate(int index, int oldSize)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (oldSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(oldSize));
            }

            this._array.AsSpan().Slice(index, oldSize).Clear();
            var oldSizeLog = LogSize(oldSize);
            ref var freeList = ref this._freeLists[oldSizeLog];
            freeList.AddLast(index);
        }

        private static int LogSize(int i)
        {
            return BitOperations.Log2((uint) i - 1) + 1;
        }
    }
}
