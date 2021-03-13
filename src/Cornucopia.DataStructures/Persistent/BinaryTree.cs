using System;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent binary tree.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the tree.</typeparam>
    public readonly partial struct BinaryTree<T>
    {
        /// <summary>
        ///     The empty tree.
        /// </summary>
        public static BinaryTree<T> Empty => default;

        private readonly Node? _root;

        private BinaryTree(Node? root)
        {
            this._root = root;
        }

        internal BinaryTree(T value)
        {
            this._root = new(value);
        }

        internal BinaryTree(BinaryTree<T> leftSubTree, BinaryTree<T> rightSubTree, T value)
        {
            this._root = new(leftSubTree._root, rightSubTree._root, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this tree is empty.
        /// </summary>
        /// <returns><c>true</c> if the tree is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty => this._root == null;

        /// <summary>
        ///     Gets a value indicating whether this tree has any elements.
        /// </summary>
        /// <returns><c>true</c> if the tree has any elements; otherwise, <c>false</c>.</returns>
        public bool Any => this._root != null;

        /// <summary>
        ///     The left subtree.
        /// </summary>
        public BinaryTree<T> LeftSubTree
        {
            get
            {
                if (this._root == null)
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return new(this._root.LeftChild);
            }
        }

        /// <summary>
        ///     The right subtree.
        /// </summary>
        public BinaryTree<T> RightSubTree
        {
            get
            {
                if (this._root == null)
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return new(this._root.RightChild);
            }
        }

        /// <summary>
        ///     The element in the root node.
        /// </summary>
        public T RootValue
        {
            get
            {
                if (this._root == null)
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return this._root.Value;
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using a pre-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachPreOrder(Action<T> action)
        {
            this._root?.ForEachPreOrder(action);
        }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using an in-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachInOrder(Action<T> action)
        {
            this._root?.ForEachInOrder(action);
        }

        /// <summary>
        ///     Performs the specified action on each element of the tree, using a post-order traversal.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the tree.</param>
        public void ForEachPostOrder(Action<T> action)
        {
            this._root?.ForEachPostOrder(action);
        }
    }
}