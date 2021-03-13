using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A set of initialization methods for instances of <see cref="RandomAccessList{T}"/>.
    /// </summary>
    public static class RandomAccessList
    {
        /// <summary>
        ///     Creates a list with the specified element as its only member.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="item">The element to store in the list.</param>
        /// <returns>A new list with the specified element.</returns>
        [Pure]
        public static RandomAccessList<T> Create<T>(T item)
        {
            return RandomAccessList<T>.Empty.AddFirst(item);
        }

        /// <summary>
        /// Creates a list with the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="items">The elements to store in the list.</param>
        /// <returns>A new list with the specified elements.</returns>
        [Pure]
        public static RandomAccessList<T> Create<T>(params T[] items)
        {
            var result = RandomAccessList<T>.Empty;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                result = result.AddFirst(items[i]);
            }

            return result;
        }
    }
}