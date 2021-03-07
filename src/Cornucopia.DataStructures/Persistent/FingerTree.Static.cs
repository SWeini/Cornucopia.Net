using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public static class FingerTree
    {
        [Pure]
        public static FingerTree<T> Create<T>(T item)
        {
            return FingerTree<T>.From(item);
        }

        [Pure]
        public static FingerTree<T> Create<T>(params T[] items)
        {
            return CreateRange(items);
        }

        [Pure]
        public static FingerTree<T> CreateRange<T>(IEnumerable<T> items)
        {
            return FingerTree<T>.From(items);
        }
    }
}
