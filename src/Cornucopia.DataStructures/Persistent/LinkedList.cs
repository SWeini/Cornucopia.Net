using System;
using System.Diagnostics.Contracts;

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

        public void ForEach(Action<T> action)
        {
            var list = this;
            do
            {
                action(list.Head);
                list = list.Tail;
            } while (list.Any());
        }

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