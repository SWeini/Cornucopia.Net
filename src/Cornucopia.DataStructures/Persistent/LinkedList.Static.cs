using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A set of methods for instances of <see cref="LinkedList{T}"/>.
    /// </summary>
    public static class LinkedList
    {
        /// <summary>
        ///     Creates a list with the specified element as its only member.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="value">The element to store in the list.</param>
        /// <returns>A new list with the specified element.</returns>
        [Pure]
        public static LinkedList<T> Create<T>(T value)
        {
            return new(value);
        }

        /// <summary>
        ///     Prepends a specified element to a list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to process.</param>
        /// <param name="value">The element to prepend to <paramref name="list"/>.</param>
        /// <returns>A new list with <paramref name="value"/> followed by <paramref name="list"/>.</returns>
        [Pure]
        public static LinkedList<T> Prepend<T>(this LinkedList<T>? list, T value)
        {
            return new(list, value);
        }

        /// <summary>
        ///     Gets a value indicating whether a list is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to examine.</param>
        /// <returns><c>true</c> if <paramref name="list"/> is empty; otherwise, <c>false</c>.</returns>
        [Pure]
        public static bool IsEmpty<T>([NotNullWhen(false)] this LinkedList<T>? list)
        {
            return list is null;
        }

        /// <summary>
        ///     Gets a value indicating whether a list has any elements.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to examine.</param>
        /// <returns><c>true</c> if <paramref name="list"/> has any elements; otherwise, <c>false</c>.</returns>
        [Pure]
        public static bool Any<T>([NotNullWhen(true)] this LinkedList<T>? list)
        {
            return list is not null;
        }
    }
}