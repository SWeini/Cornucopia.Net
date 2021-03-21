using System;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures.Persistent
{
    partial struct LinkedList<T>
    {
        /// <summary>
        ///     A node of a persistent singly-linked list.
        /// </summary>
        public sealed class Node
        {
            internal Node(T head)
            {
                this.Head = head;
            }

            internal Node(Node? tail, T head)
            {
                this.Tail = tail;
                this.Head = head;
            }

            /// <summary>
            ///     The list without the head element.
            /// </summary>
            public Node? Tail { get; }

            /// <summary>
            ///     The element at the head of the list.
            /// </summary>
            public T Head { get; }

            /// <summary>
            ///     Performs the specified action on each element of the list.
            /// </summary>
            /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
            public void ForEach([InstantHandle] Action<T> action)
            {
                var node = this;
                do
                {
                    action(node.Head);
                    node = node.Tail;
                } while (node != null);
            }

            /// <summary>
            ///     Reverses the elements of the list.
            /// </summary>
            /// <returns>A list with reversed elements.</returns>
            /// <remarks>Returns the same instance if list contains only a single element.</remarks>
            [Pure]
            public Node Reverse()
            {
                var remaining = this.Tail;
                if (remaining == null)
                {
                    return this;
                }

                var reversed = new Node(this.Head);
                do
                {
                    reversed = new(reversed, remaining.Head);
                    remaining = remaining.Tail;
                } while (remaining != null);

                return reversed;
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
}