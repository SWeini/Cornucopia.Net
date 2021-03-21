using JetBrains.Annotations;

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
    }
}