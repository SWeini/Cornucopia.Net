using System;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent singly-linked list.
    /// </summary>
    /// <remarks>The value <c>null</c> is valid and represents an empty list.</remarks>
    /// <typeparam name="T">The type of elements stored by the list.</typeparam>
    public sealed class LinkedList<T>
    {
        /// <summary>
        ///     The empty list.
        /// </summary>
        public static LinkedList<T>? Empty => null;

        internal LinkedList(T head)
        {
            this.Head = head;
        }

        internal LinkedList(LinkedList<T>? tail, T head)
        {
            this.Tail = tail;
            this.Head = head;
        }

        /// <summary>
        ///     The list without the head element.
        /// </summary>
        public LinkedList<T>? Tail { get; }

        /// <summary>
        ///     The element at the head of the list.
        /// </summary>
        public T Head { get; }

        /// <summary>
        ///     Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
        public void ForEach(Action<T> action)
        {
            var list = this;
            do
            {
                action(list.Head);
                list = list.Tail;
            } while (list.Any());
        }

        /// <summary>
        ///     Reverses the elements of the list.
        /// </summary>
        /// <returns>A list with reversed elements.</returns>
        /// <remarks>Returns the same instance if list contains only a single element.</remarks>
        [Pure]
        public LinkedList<T> Reverse()
        {
            if (this.Tail.IsEmpty())
            {
                return this;
            }

            var result = LinkedList.Create(this.Head);
            var list = this.Tail;
            do
            {
                result = result.Prepend(list.Head);
                list = list.Tail;
            } while (list.Any());

            return result;
        }

        /// <summary>
        ///     Counts the elements in the list.
        /// </summary>
        /// <returns>The number of elements in the list.</returns>
        [Pure]
        public int Count()
        {
            var result = 1;
            var node = this.Tail;
            while (node != null)
            {
                node = node.Tail;
                result++;
            }

            return result;
        }
    }
}