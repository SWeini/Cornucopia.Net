using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures.Persistent
{
    /// <summary>
    ///     A persistent list using a finger tree.
    /// </summary>
    /// <typeparam name="T">The type of elements stored by the list.</typeparam>
    public readonly partial struct FingerTree<T>
    {
        /// <summary>
        ///     The empty list.
        /// </summary>
        public static FingerTree<T> Empty => new(EmptyNode<ItemNode>.Instance);

        private readonly StemNode<ItemNode> _root;

        private FingerTree(StemNode<ItemNode> root)
        {
            this._root = root;
        }

        /// <summary>
        ///     Gets a value indicating whether this list is empty.
        /// </summary>
        /// <value><c>true</c> if the list is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => this._root.Count == 0;

        /// <summary>
        ///     Gets a value indicating whether this list has any elements.
        /// </summary>
        /// <value><c>true</c> if the list has any elements; otherwise, <c>false</c>.</value>
        public bool Any => this._root.Count != 0;

        /// <summary>
        ///     Gets the number of elements in the list.
        /// </summary>
        /// <value>The number of elements in the list.</value>
        public int Count => this._root.Count;

        /// <summary>
        ///     Gets the first element of the list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        public T First
        {
            get
            {
                var view = this._root.First;
                if (view == null)
                {
                    throw new InvalidOperationException("The list is empty.");
                }

                return view.Node.Value;
            }
        }

        /// <summary>
        ///     Prepends a specified element to the list.
        /// </summary>
        /// <param name="value">The element to prepend to the list.</param>
        /// <returns>A new list with <paramref name="value"/> followed by the list.</returns>
        [Pure]
        public FingerTree<T> AddFirst(T value)
        {
            return new(this._root.Prepend(new(value)));
        }

        /// <summary>
        ///     Removes the first element from the list.
        /// </summary>
        /// <returns>A new list with all elements of the list, except the first.</returns>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        [Pure]
        public FingerTree<T> RemoveFirst() => this.RemoveFirst(out _);

        /// <summary>
        ///     Extracts the first element from the list.
        /// </summary>
        /// <param name="first">The first element of the list.</param>
        /// <returns>A new list with all elements of the list, except the first.</returns>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        [Pure]
        public FingerTree<T> RemoveFirst(out T first)
        {
            var view = this._root.First;
            if (view == null)
            {
                throw new InvalidOperationException("The list is empty.");
            }

            first = view.Node.Value;
            return new(view.Tree());
        }

        /// <summary>
        ///     Gets the last element of the list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        public T Last
        {
            get
            {
                var view = this._root.Last;
                if (view == null)
                {
                    throw new InvalidOperationException("The list is empty.");
                }

                return view.Node.Value;
            }
        }

        /// <summary>
        ///     Appends a specified element to the list.
        /// </summary>
        /// <param name="value">The element to append to the list.</param>
        /// <returns>A new list with <paramref name="value"/> appended to the list.</returns>
        [Pure]
        public FingerTree<T> AddLast(T value)
        {
            return new(this._root.Append(new(value)));
        }

        /// <summary>
        ///     Removes the last element from the list.
        /// </summary>
        /// <returns>A new list with all elements of the list, except the last.</returns>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        [Pure]
        public FingerTree<T> RemoveLast() => this.RemoveLast(out _);

        /// <summary>
        ///     Extracts the last element from the list.
        /// </summary>
        /// <param name="last">The last element of the list.</param>
        /// <returns>A new list with all elements of the list, except the last.</returns>
        /// <exception cref="InvalidOperationException">The list contains no elements.</exception>
        [Pure]
        public FingerTree<T> RemoveLast(out T last)
        {
            var view = this._root.Last;
            if (view == null)
            {
                throw new InvalidOperationException("The list is empty.");
            }

            last = view.Node.Value;
            return new(view.Tree());
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
                var node = this._root;
                if ((uint) index >= (uint) node.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return node.GetElementAt(index);
            }
        }

        /// <summary>
        ///     Sets an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to set.</param>
        /// <param name="value">The element to store at the specified index.</param>
        /// <returns>A new list with the specified element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        [Pure]
        public FingerTree<T> SetItem(int index, T value)
        {
            var node = this._root;
            if ((uint) index >= (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return new((StemNode<ItemNode>) node.SetElementAt(index, value));
        }

        /// <summary>
        ///     Appends another list.
        /// </summary>
        /// <param name="other">The list to append to this instance.</param>
        /// <returns>A new list, representing the concatenation of both lists.</returns>
        [Pure]
        public FingerTree<T> Append(FingerTree<T> other)
        {
            return new(this._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, other._root));
        }

        /// <summary>
        ///     Prepends another list.
        /// </summary>
        /// <param name="other">The list to prepend to this instance.</param>
        /// <returns>A new list, representing the concatenation of both lists.</returns>
        [Pure]
        public FingerTree<T> Prepend(FingerTree<T> other)
        {
            return new(other._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, this._root));
        }

        /// <summary>
        ///     Inserts an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the inserted element.</param>
        /// <param name="element">The element to insert.</param>
        /// <returns>A new list with the specified element inserted at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        [Pure]
        public FingerTree<T> Insert(int index, T element)
        {
            var node = this._root;
            if ((uint) index > (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == 0)
            {
                return new(node.Prepend(new ItemNode(element)));
            }

            if (index == node.Count)
            {
                return new(node.Append(new ItemNode(element)));
            }

            var split = node.Split(index);
            return new(split.LeftTree.ConcatWithMiddle(new[] { new ItemNode(element), split.Node }, split.RightTree));
        }

        /// <summary>
        ///     Inserts another list at the specified index.
        /// </summary>
        /// <param name="index">The index of the inserted list.</param>
        /// <param name="items">The list to insert into this instance.</param>
        /// <returns>A new list with <paramref name="items"/> inserted at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        [Pure]
        public FingerTree<T> InsertRange(int index, FingerTree<T> items)
        {
            var node = this._root;
            if ((uint) index > (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (items.Count == 0)
            {
                return this;
            }

            if (index == 0)
            {
                return new(items._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, this._root));
            }

            if (index == node.Count)
            {
                return new(this._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, items._root));
            }

            var split = node.Split(index);
            return new(split.LeftTree.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, items._root).ConcatWithMiddle(new[] { split.Node }, split.RightTree));
        }

        /// <summary>
        ///     Removes an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>A new list with the element at the specified index removed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The specified index is out of range.</exception>
        [Pure]
        public FingerTree<T> RemoveAt(int index)
        {
            var node = this._root;
            if ((uint) index >= (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var split = node.Split(index);
            return new(split.LeftTree.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, split.RightTree));
        }

        /// <summary>
        ///     Gets the specified range of elements.
        /// </summary>
        /// <param name="index">The index of the first element to get.</param>
        /// <param name="count">The number of elements to get.</param>
        /// <returns>A new list representing the specified range in this list.</returns>
        [Pure]
        public FingerTree<T> GetRange(int index, int count)
        {
            var node = this._root;
            if (index > 0)
            {
                var split = node.Split(index - 1);
                node = split.RightTree;
            }

            if (count < node.Count)
            {
                var split = node.Split(count);
                node = split.LeftTree;
            }

            return new(node);
        }

        /// <summary>
        ///     Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the list.</param>
        public void ForEach([InstantHandle] Action<T> action)
        {
            this._root.ForEach(action);
        }

        [Pure]
        internal static FingerTree<T> From(T item)
        {
            return new(new SingleNode<ItemNode>(new ItemNode(item)));
        }

        [Pure]
        internal static FingerTree<T> From(IEnumerable<T> items)
        {
            StemNode<ItemNode> node = EmptyNode<ItemNode>.Instance;
            foreach (var item in items)
            {
                node = node.Append(new ItemNode(item));
            }

            return new(node);
        }
    }
}
