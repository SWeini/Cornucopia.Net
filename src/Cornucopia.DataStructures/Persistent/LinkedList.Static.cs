using System;
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
            return new(null, value);
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

        /// <summary>
        ///     Ensures that a list is not empty.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to examine.</param>
        public static void EnsureNotEmpty<T>([NotNull] this LinkedList<T>? list)
        {
            if (list.IsEmpty())
            {
                throw new InvalidOperationException("Collection is empty.");
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of a list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to process.</param>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
        public static void ForEach<T>(this LinkedList<T>? list, Action<T> action)
        {
            while (list.Any())
            {
                action(list.Head);
                list = list.Tail;
            }
        }

        /// <summary>
        ///     Reverses the elements of a list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to process.</param>
        /// <returns>A list with reversed elements.</returns>
        [Pure]
        [return: NotNullIfNotNull("list")]
        public static LinkedList<T>? Reverse<T>(this LinkedList<T>? list)
        {
            if (list.IsEmpty() || list.Tail.IsEmpty())
            {
                return list;
            }

            var result = LinkedList<T>.Empty;
            do
            {
                result = result.Prepend(list.Head);
                list = list.Tail;
            } while (list.Any());

            return result;
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
        ///     Counts the elements in a list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the list.</typeparam>
        /// <param name="list">The list to examine.</param>
        /// <returns>The number of elements in <paramref name="list"/>.</returns>
        [Pure]
        public static int Count<T>(this LinkedList<T>? list)
        {
            var result = 0;
            while (list.Any())
            {
                list = list.Tail;
                result++;
            }

            return result;
        }
    }
}