using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A set of methods for instances of <see cref="BinaryTree{T}"/>.
    /// </summary>
    public static class BinaryTree
    {
        /// <summary>
        ///     Creates a leaf node with the specified element.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="value">The element to store in the leaf node.</param>
        /// <returns>A leaf node with the specified element.</returns>
        [Pure]
        public static BinaryTree<T> Create<T>(T value)
        {
            return new(value);
        }

        /// <summary>
        ///     Creates a node with the specified element and children.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="leftChild">The left child to store in the node.</param>
        /// <param name="rightChild">The right child to store in the node.</param>
        /// <param name="value">The element to store in the node.</param>
        /// <returns>A node with the specified element and children.</returns>
        [Pure]
        public static BinaryTree<T> Create<T>(BinaryTree<T>? leftChild, BinaryTree<T>? rightChild, T value)
        {
            return new(leftChild, rightChild, value);
        }

        /// <summary>
        ///     Gets a value indicating whether a tree is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the tree.</typeparam>
        /// <param name="node">The tree to examine.</param>
        /// <returns><c>true</c> if <paramref name="node"/> is empty; otherwise, <c>false</c>.</returns>
        [Pure]
        public static bool IsEmpty<T>([NotNullWhen(false)] this BinaryTree<T>? node)
        {
            return node is null;
        }

        /// <summary>
        ///     Gets a value indicating whether a tree has any elements.
        /// </summary>
        /// <typeparam name="T">The type of elements stored by the tree.</typeparam>
        /// <param name="node">The tree to examine.</param>
        /// <returns><c>true</c> if <paramref name="node"/> has any elements; otherwise, <c>false</c>.</returns>
        [Pure]
        public static bool Any<T>([NotNullWhen(true)] this BinaryTree<T>? node)
        {
            return node is not null;
        }
    }
}