using System;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures.Persistent
{
    partial struct BinaryTree<T>
    {
        /// <summary>
        ///     A node of a persistent binary tree.
        /// </summary>
        public sealed class Node
        {
            internal Node(T value)
            {
                this.Value = value;
            }

            internal Node(Node? leftChild, Node? rightChild, T value)
            {
                this.LeftChild = leftChild;
                this.RightChild = rightChild;
                this.Value = value;
            }

            /// <summary>
            ///     The left child node.
            /// </summary>
            public Node? LeftChild { get; }

            /// <summary>
            ///     The right child node.
            /// </summary>
            public Node? RightChild { get; }

            /// <summary>
            ///     The element in this node.
            /// </summary>
            public T Value { get; }

            /// <summary>
            ///     Performs the specified action on each element of the tree, using a pre-order traversal.
            /// </summary>
            /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
            public void ForEachPreOrder([InstantHandle] Action<T> action)
            {
                action(this.Value);
                this.LeftChild?.ForEachPreOrder(action);
                this.RightChild?.ForEachPreOrder(action);
            }

            /// <summary>
            ///     Performs the specified action on each element of the tree, using an in-order traversal.
            /// </summary>
            /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
            public void ForEachInOrder([InstantHandle] Action<T> action)
            {
                this.LeftChild?.ForEachInOrder(action);
                action(this.Value);
                this.RightChild?.ForEachInOrder(action);
            }

            /// <summary>
            ///     Performs the specified action on each element of the tree, using a post-order traversal.
            /// </summary>
            /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
            public void ForEachPostOrder([InstantHandle] Action<T> action)
            {
                this.LeftChild?.ForEachPostOrder(action);
                this.RightChild?.ForEachPostOrder(action);
                action(this.Value);
            }
        }
    }
}
