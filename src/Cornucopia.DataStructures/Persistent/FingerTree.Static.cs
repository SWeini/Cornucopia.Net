using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A set of initialization methods for instances of <see cref="FingerTree{T}"/>.
    /// </summary>
    public static class FingerTree
    {
        /// <summary>
        ///     Creates a list with the specified element as its only member.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="item">The element to store in the list.</param>
        /// <returns>A new list with the specified element.</returns>
        [Pure]
        public static FingerTree<T> Create<T>(T item)
        {
            return FingerTree<T>.From(item);
        }

        /// <summary>
        ///     Creates a list with the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="items">The elements to store in the list.</param>
        /// <returns>A new list with the specified elements.</returns>
        [Pure]
        public static FingerTree<T> Create<T>(params T[] items)
        {
            return CreateRange(items);
        }

        /// <summary>
        ///     Creates a list with the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="items">The elements to store in the list.</param>
        /// <returns>A new list with the specified elements.</returns>
        [Pure]
        public static FingerTree<T> CreateRange<T>(IEnumerable<T> items)
        {
            return FingerTree<T>.From(items);
        }
    }
}
