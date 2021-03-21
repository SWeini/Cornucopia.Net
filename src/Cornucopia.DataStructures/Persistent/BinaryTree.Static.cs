using JetBrains.Annotations;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A set of methods for instances of <see cref="BinaryTree{T}"/>.
    /// </summary>
    public static class BinaryTree
    {
        /// <summary>
        ///     Creates a tree with the specified element at the root node.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="value">The element to store at the root node.</param>
        /// <returns>A tree with the specified element at the root node.</returns>
        [Pure]
        public static BinaryTree<T> Create<T>(T value)
        {
            return new(value);
        }

        /// <summary>
        ///     Creates a node with the specified element and subtrees.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="leftSubTree">The left subtree of the root node.</param>
        /// <param name="rightSubTree">The right subtree of the root node.</param>
        /// <param name="value">The element to store in the root node.</param>
        /// <returns>A node with the specified element and subtrees.</returns>
        [Pure]
        public static BinaryTree<T> Create<T>(BinaryTree<T> leftSubTree, BinaryTree<T> rightSubTree, T value)
        {
            return new(leftSubTree, rightSubTree, value);
        }

        /// <summary>
        ///     Creates a leaf node with the specified element.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="value">The element to store in the leaf node.</param>
        /// <returns>A leaf node with the specified element.</returns>
        [Pure]
        public static BinaryTree<T>.Node CreateNode<T>(T value)
        {
            return new(value);
        }

        /// <summary>
        ///     Creates a node with the specified element and children.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the tree.</typeparam>
        /// <param name="leftChild">The left child of the created node.</param>
        /// <param name="rightChild">The right child of the created node.</param>
        /// <param name="value">The element to store in the created node.</param>
        /// <returns>A node with the specified element and children.</returns>
        [Pure]
        public static BinaryTree<T>.Node CreateNode<T>(BinaryTree<T>.Node? leftChild, BinaryTree<T>.Node? rightChild, T value)
        {
            return new(leftChild, rightChild, value);
        }
    }
}