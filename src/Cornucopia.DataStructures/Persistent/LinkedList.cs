namespace Cornucopia.DataStructures.Persistent
{
    public sealed class LinkedList<T>
    {
        public static LinkedList<T>? Empty => null;

        internal LinkedList(LinkedList<T>? tail, T head)
        {
            this.Tail = tail;
            this.Head = head;
        }

        public LinkedList<T>? Tail { get; }
        public T Head { get; }
    }
}