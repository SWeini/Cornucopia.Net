using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public static class BinaryTree
    {
        [Pure]
        public static BinaryTree<T> Create<T>(T value)
        {
            return new(value, null, null);
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

        public static void ForEachPreOrder<T>(BinaryTree<T>? node, Action<T> action)
        {
            if (node.Any())
            {
                action(node.Value);
                ForEachPreOrder(node.LeftChild, action);
                ForEachPreOrder(node.RightChild, action);
            }
        }

        public static void ForEachInOrder<T>(BinaryTree<T>? node, Action<T> action)
        {
            if (node.Any())
            {
                ForEachInOrder(node.LeftChild, action);
                action(node.Value);
                ForEachInOrder(node.RightChild, action);
            }
        }

        public static void ForEachPostOrder<T>(BinaryTree<T>? node, Action<T> action)
        {
            if (node.Any())
            {
                ForEachPostOrder(node.LeftChild, action);
                ForEachPostOrder(node.RightChild, action);
                action(node.Value);
            }
        }
    }
}