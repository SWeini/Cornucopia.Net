namespace Cornucopia.DataStructures.Persistent
{
    public sealed class LinkedList<T>
    {
        public static LinkedList<T>? Empty => null;

        internal LinkedList(T head, LinkedList<T>? tail)
        {
            this.Head = head;
            this.Tail = tail;
        }

        public T Head { get; }

        public LinkedList<T>? Tail { get; }
    }
}