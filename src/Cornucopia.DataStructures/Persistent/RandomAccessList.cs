using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent random-access list.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the list.</typeparam>
    public readonly struct RandomAccessList<T>
    {
        /// <summary>
        ///     The empty list.
        /// </summary>
        public static RandomAccessList<T> Empty => default;

        private readonly LinkedList<Node>.Node? _root;

        private RandomAccessList(LinkedList<Node>.Node? root)
        {
            this._root = root;
        }

        /// <summary>
        ///     Gets a value indicating whether this list is empty.
        /// </summary>
        /// <value><c>true</c> if the list is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => this._root is null;

        /// <summary>
        ///     Gets a value indicating whether this list has any elements.
        /// </summary>
        /// <value><c>true</c> if the list has any elements; otherwise, <c>false</c>.</value>
        public bool Any => this._root is not null;

        /// <summary>
        ///     Counts the elements in the list.
        /// </summary>
        /// <returns>The number of elements in the list.</returns>
        [Pure]
        public int Count()
        {
            var list = this._root;
            var result = 0;
            while (list != null)
            {
                result += list.Head.Count;
                list = list.Tail;
            }

            return result;
        }

        /// <summary>
        ///     Gets the first element of the list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        public T First
        {
            get
            {
                if (this._root == null)
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return this._root.Head.Tree.Value;
            }
        }

        /// <summary>
        ///     Prepends a specified element to the list.
        /// </summary>
        /// <param name="value">The element to prepend to the list.</param>
        /// <returns>A new list with <paramref name="value"/> followed by the list.</returns>
        [Pure]
        public RandomAccessList<T> AddFirst(T value)
        {
            var first = this._root;
            if (first != null)
            {
                var second = first.Tail;
                if (second != null)
                {
                    if (first.Head.Count == second.Head.Count)
                    {
                        var combinedTree = BinaryTree.Create(first.Head.Tree, second.Head.Tree, value);
                        var combinedNode = new Node(combinedTree, first.Head.Count * 2 + 1);
                        return new(new(second.Tail, combinedNode));
                    }
                }
            }

            var singleValueTree = BinaryTree.Create(value);
            var singleValueNode = new Node(singleValueTree, 1);
            return new(new(first, singleValueNode));
        }

        /// <summary>
        ///     Extracts the first element from the list.
        /// </summary>
        /// <param name="value">The first element of the list.</param>
        /// <returns>A new list with all elements of the list, except the first.</returns>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        [Pure]
        public RandomAccessList<T> RemoveFirst(out T value)
        {
            if (this._root == null)
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            var tree = this._root.Head.Tree;
            value = tree.Value;
            if (this._root.Head.Count == 1)
            {
                return new(this._root.Tail);
            }

            var halfCount = this._root.Head.Count / 2;
            return new(new(new(this._root.Tail, new(tree.RightChild!, halfCount)), new(tree.LeftChild!, halfCount)));
        }

        /// <summary>
        ///     Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var list = this._root;
                while (list != null)
                {
                    var node = list.Head;
                    if (index >= node.Count)
                    {
                        index -= node.Count;
                        list = list.Tail;
                        continue;
                    }

                    var tree = node.Tree;
                    var count = node.Count / 2;
                    while (true)
                    {
                        if (index == 0)
                        {
                            return tree.Value;
                        }

                        index--;
                        if (index < count)
                        {
                            tree = tree.LeftChild!;
                        }
                        else
                        {
                            tree = tree.RightChild!;
                            index -= count;
                        }

                        count /= 2;
                    }
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
        public void ForEach(Action<T> action)
        {
            var list = this._root;
            while (list != null)
            {
                list.Head.Tree.ForEachPreOrder(action);
                list = list.Tail;
            }
        }

        /// <summary>
        ///     Gets an enumerator to iterate over the elements of the list.
        /// </summary>
        /// <returns>The enumerator to iterate over the elements of the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var list = this;
            while (list.Any)
            {
                list = list.RemoveFirst(out var next);
                yield return next;
            }
        }

        private readonly struct Node
        {
            public Node(BinaryTree<T> tree, int count)
            {
                this.Tree = tree;
                this.Count = count;
            }

            public BinaryTree<T> Tree { get; }
            public int Count { get; }
        }
    }
}