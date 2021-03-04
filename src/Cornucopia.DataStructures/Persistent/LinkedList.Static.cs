using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public static class LinkedList
    {
        [Pure]
        public static LinkedList<T> Create<T>(T value)
        {
            return new(value, null);
        }

        [Pure]
        public static bool IsEmpty<T>([NotNullWhen(false)] this LinkedList<T>? list)
        {
            return list is null;
        }

        [Pure]
        public static bool Any<T>([NotNullWhen(true)] this LinkedList<T>? list)
        {
            return list is not null;
        }

        public static void EnsureNotEmpty<T>([NotNull] this LinkedList<T>? list)
        {
            if (list.IsEmpty())
            {
                throw new InvalidOperationException("Collection is empty.");
            }
        }

        public static void ForEach<T>(this LinkedList<T>? list, Action<T> action)
        {
            while (list.Any())
            {
                action(list.Head);
                list = list.Tail;
            }
        }

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

        [Pure]
        public static LinkedList<T> Prepend<T>(this LinkedList<T>? list, T value)
        {
            return new(value, list);
        }

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