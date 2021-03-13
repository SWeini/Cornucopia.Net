using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Cornucopia.DataStructures.Persistent
{
    public readonly struct HashArrayMappedTrie<T, TComparer>
        where TComparer : struct, IEqualityComparer<T>
    {
        private readonly TComparer _comparer;
        private readonly HashEntry[] _mainArray;
        private readonly uint _mainMap;

        public HashArrayMappedTrie(TComparer comparer)
        {
            this._comparer = comparer;
#if NETCOREAPP3_1
            this._mainArray = Array.Empty<HashEntry>();
#else
            this._mainArray = new HashEntry[0];
#endif
            this._mainMap = 0;
        }

        private HashArrayMappedTrie(TComparer comparer, HashEntry[] mainArray, uint mainMap)
        {
            this._comparer = comparer;
            this._mainArray = mainArray;
            this._mainMap = mainMap;
        }

        public HashArrayMappedTrie<T, TComparer> Add(T item)
        {
            var hashCode = this.GetHashCode(item);
            var (map, array) = this.Add(this._mainArray, this._mainMap, item, hashCode, 0);
            return new(this._comparer, array, map);
        }

        public bool Contains(T item)
        {
            var hashCode = this.GetHashCode(item);
            var node = this.Find(hashCode);
            if (node == null)
            {
                return false;
            }

            if (node is T singleValue)
            {
                return this._comparer.Equals(singleValue, item);
            }

            var array = (T[]) node;
            foreach (var value in array)
            {
                if (this._comparer.Equals(value, item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetFirst(ref T item)
        {
            var hashCode = this.GetHashCode(item);
            var node = this.Find(hashCode);
            if (node == null)
            {
                return false;
            }

            if (node is T singleValue)
            {
                if (this._comparer.Equals(singleValue, item))
                {
                    item = singleValue;
                    return true;
                }

                return false;
            }

            var array = (T[]) node;
            foreach (var value in array)
            {
                if (this._comparer.Equals(value, item))
                {
                    item = value;
                    return true;
                }
            }

            return false;
        }

        private uint GetHashCode(T item)
        {
            return item is null ? 0 : (uint) this._comparer.GetHashCode(item);
        }

        private static int? GetIndex(uint map, uint hashCode)
        {
            var bit = 1u << (int) hashCode;
            if ((map & bit) == 0)
            {
                return null;
            }

            return BitOperations.PopCount(map & (bit - 1));
        }

        private static int GetInsertIndex(ref uint map, uint hashCode)
        {
            var bit = 1u << (int) hashCode;
            map |= bit;
            return BitOperations.PopCount(map & (bit - 1));
        }

        private (uint map, HashEntry[] array) Add(HashEntry[] array, uint map, in T item, uint hashCode, int shift)
        {
            var idx = GetIndex(map, hashCode >> shift);
            if (!idx.HasValue)
            {
                var insertIndex = GetInsertIndex(ref map, hashCode >> shift);
                var newTable = new HashEntry[array.Length + 1];
                Array.Copy(array, 0, newTable, 0, insertIndex);
                newTable[insertIndex] = new HashEntry(hashCode, item);
                Array.Copy(array, insertIndex, newTable, insertIndex + 1, array.Length - insertIndex);
                return (map, newTable);
            }

            ref var entry = ref array[idx.Value];
            if (entry.SubHashTableOrLeaf is HashEntry[] subHashTable)
            {
                var newEntry = this.Add(subHashTable, entry.MapOrHash, item, hashCode, shift + 5);
                var copy = array.ToArray();
                copy[idx.Value] = new HashEntry(newEntry.map, newEntry.array);
                return (map, copy);
            }

            if (entry.SubHashTableOrLeaf is T[] multiLeaf)
            {
                if (entry.MapOrHash == hashCode)
                {
                    var newEntries = new T[multiLeaf.Length + 1];
                    Array.Copy(multiLeaf, newEntries, multiLeaf.Length);
                    newEntries[multiLeaf.Length] = item;
                    var copy = array.ToArray();
                    copy[idx.Value] = new HashEntry(hashCode, newEntries);
                    return (map, copy);
                }
            }
            else
            {
                if (entry.MapOrHash == hashCode)
                {
                    var newEntries = new[] { (T) entry.SubHashTableOrLeaf!, item };
                    var copy = array.ToArray();
                    copy[idx.Value] = new HashEntry(hashCode, newEntries);
                    return (map, copy);
                }
            }

            {
                subHashTable = new[] { entry };
                var subMap = 1u << (int) (entry.MapOrHash >> shift);
                var newEntry = this.Add(subHashTable, subMap, item, hashCode, shift + 5);
                var copy = array.ToArray();
                copy[idx.Value] = new HashEntry(newEntry.map, newEntry.array);
                return (map, copy);
            }
        }

        private object? Find(uint hashCode)
        {
            var array = this._mainArray;
            var map = this._mainMap;

            while (true)
            {
                var index = GetIndex(map, hashCode);
                if (!index.HasValue)
                {
                    return null;
                }

                var node = array[index.Value];
                if (node.SubHashTableOrLeaf is HashEntry[] subHashTable)
                {
                    array = subHashTable;
                    map = node.MapOrHash;
                    continue;
                }

                if (node.MapOrHash == hashCode)
                {
                    return node.SubHashTableOrLeaf;
                }

                return null;
            }
        }

        private readonly struct HashEntry
        {
            public HashEntry(uint mapOrHash, object? subHashTableOrLeaf)
            {
                this.MapOrHash = mapOrHash;
                this.SubHashTableOrLeaf = subHashTableOrLeaf;
            }

            public uint MapOrHash { get; }
            public object? SubHashTableOrLeaf { get; }
        }
    }
}
