using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public static class BinaryTree
    {
        [Pure]
        public static BinaryTree<T> Create<T>(T value)
        {
            return new(null, null, value);
        }

        [Pure]
        public static bool IsEmpty<T>([NotNullWhen(false)] this BinaryTree<T>? node)
        {
            return node is null;
        }

        [Pure]
        public static bool Any<T>([NotNullWhen(true)] this BinaryTree<T>? node)
        {
            return node is not null;
        }
    }
}