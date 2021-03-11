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
    }
}