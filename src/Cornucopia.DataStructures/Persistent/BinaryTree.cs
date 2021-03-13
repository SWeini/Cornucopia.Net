using System;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent binary tree.
    /// </summary>
    /// <remarks>The value <c>null</c> is valid and represents an empty tree.</remarks>
    /// <typeparam name="T">The type of elements stored by the tree.</typeparam>
    public sealed class BinaryTree<T>
    {
        /// <summary>
        ///     The empty tree.
        /// </summary>
        public static BinaryTree<T>? Empty => null;

        internal BinaryTree(T value)
        {
            this.Value = value;
        }

        internal BinaryTree(BinaryTree<T>? leftChild, BinaryTree<T>? rightChild, T value)
        {
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
            this.Value = value;
        }

        /// <summary>
        ///     The left child node.
        /// </summary>
        public BinaryTree<T>? LeftChild { get; }

        /// <summary>
        ///     The right child node.
        /// </summary>
        public BinaryTree<T>? RightChild { get; }

        /// <summary>
        ///     The element in this node.
        /// </summary>
        public T Value { get; }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using a pre-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachPreOrder(Action<T> action)
        {
            action(this.Value);
            this.LeftChild?.ForEachPreOrder(action);
            this.RightChild?.ForEachPreOrder(action);
        }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using an in-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachInOrder(Action<T> action)
        {
            this.LeftChild?.ForEachInOrder(action);
            action(this.Value);
            this.RightChild?.ForEachInOrder(action);
        }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using a post-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachPostOrder(Action<T> action)
        {
            this.LeftChild?.ForEachPostOrder(action);
            this.RightChild?.ForEachPostOrder(action);
            action(this.Value);
        }
    }
}