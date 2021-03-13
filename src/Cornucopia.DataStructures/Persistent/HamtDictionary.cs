using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cornucopia.DataStructures.Persistent
{
    public struct HamtDictionary<TKey, TValue>
    {
        private HashArrayMappedTrie<KeyValuePair<TKey, TValue>, KeyComparer> _trie;

        public HamtDictionary(IEqualityComparer<TKey> keyComparer)
        {
            this._trie = new(new(keyComparer));
            this.Count = 0;
        }

        public int Count { get; private set; }

        public bool ContainsKey(TKey key)
        {
            return this._trie.Contains(new(key, default!));
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var pair = new KeyValuePair<TKey, TValue>(key, default!);
            if (this._trie.TryGetFirst(ref pair))
            {
                value = pair.Value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                var pair = new KeyValuePair<TKey, TValue>(key, default!);
                if (this._trie.TryGetFirst(ref pair))
                {
                    return pair.Value;
                }

                throw new KeyNotFoundException();
            }
            set
            {
                var pair = new KeyValuePair<TKey, TValue>(key, value);
                if (this._trie.Contains(pair))
                {
                    return;
                }

                this._trie.Add(pair);
                this.Count++;
            }
        }

        private readonly struct KeyComparer : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            private readonly IEqualityComparer<TKey> _keyComparer;

            public KeyComparer(IEqualityComparer<TKey> keyComparer)
            {
                this._keyComparer = keyComparer;
            }

            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return this._keyComparer.Equals(x.Key, y.Key);
            }

            public int GetHashCode(KeyValuePair<TKey, TValue> obj)
            {
                return obj.Key is null ? 0 : this._keyComparer.GetHashCode(obj.Key);
            }
        }
    }
}