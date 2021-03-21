using System;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent singly-linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the list.</typeparam>
    public readonly partial struct LinkedList<T>
    {
        /// <summary>
        ///     The empty list.
        /// </summary>
        public static LinkedList<T> Empty => default;

        private readonly Node? _root;

        private LinkedList(Node? root)
        {
            this._root = root;
        }

        internal LinkedList(T value)
        {
            this._root = new(value);
        }

        /// <summary>
        ///     Gets a value indicating whether this list is empty.
        /// </summary>
        /// <value><c>true</c> if the list is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => this._root == null;

        /// <summary>
        ///     Gets a value indicating whether this list has any elements.
        /// </summary>
        /// <value><c>true</c> if the list has any elements; otherwise, <c>false</c>.</value>
        public bool Any => this._root != null;

        /// <summary>
        ///     Gets the first element of the list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        public T First
        {
            get
            {
                if (this._root == null)
                {
                    throw new InvalidOperationException("The list is empty.");
                }

                return this._root.Head;
            }
        }

        /// <summary>
        ///     Prepends a specified element to the list.
        /// </summary>
        /// <param name="value">The element to prepend to the list.</param>
        /// <returns>A new list with <paramref name="value"/> followed by this list.</returns>
        [Pure]
        public LinkedList<T> AddFirst(T value)
        {
            return new(new Node(this._root, value));
        }

        /// <summary>
        ///     Removes the first element from the list.
        /// </summary>
        /// <returns>A new list with all elements of the list, except the first.</returns>
        /// <exception cref="InvalidOperationException">the list contains no elements.</exception>
        [Pure]
        public LinkedList<T> RemoveFirst()
        {
            if (this._root == null)
            {
                throw new InvalidOperationException("The list is empty.");
            }

            return new(this._root.Tail);
        }

        /// <summary>
        ///     Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
        public void ForEach(Action<T> action)
        {
            this._root?.ForEach(action);
        }

        /// <summary>
        ///     Reverses the elements of the list.
        /// </summary>
        /// <returns>A list with reversed elements.</returns>
        /// <remarks>Returns the same instance if list contains only a single element.</remarks>
        [Pure]
        public LinkedList<T> Reverse()
        {
            return new(this._root?.Reverse());
        }

        /// <summary>
        ///     Counts the elements in the list.
        /// </summary>
        /// <returns>The number of elements in the list.</returns>
        [Pure]
        public int Count()
        {
            return this._root?.Count() ?? 0;
        }
    }
}