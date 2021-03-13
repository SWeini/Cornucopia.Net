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
            return new(null, value);
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

        [Pure]
        public static LinkedList<T> Prepend<T>(this LinkedList<T>? list, T value)
        {
            return new(list, value);
        }
    }
}