using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public readonly partial struct FingerTree<T>
    {
        public static FingerTree<T> Empty => new(EmptyNode<ItemNode>.Instance);

        private readonly StemNode<ItemNode> _root;

        private FingerTree(StemNode<ItemNode> root)
        {
            this._root = root;
        }

        public bool IsEmpty => this._root.Count == 0;

        public bool Any => this._root.Count != 0;

        public int Count => this._root.Count;

        public T First
        {
            get
            {
                var view = this._root.First;
                if (view == null)
                {
                    throw new InvalidOperationException("Collection is empty.");
                }

                return view.Node.Value;
            }
        }

        [Pure]
        public FingerTree<T> AddFirst(T value)
        {
            return new(this._root.Prepend(new(value)));
        }

        [Pure]
        public FingerTree<T> RemoveFirst() => this.RemoveFirst(out _);

        [Pure]
        public FingerTree<T> RemoveFirst(out T first)
        {
            var view = this._root.First;
            if (view == null)
            {
                throw new InvalidOperationException();
            }

            first = view.Node.Value;
            return new(view.Tree());
        }

        public T Last
        {
            get
            {
                var view = this._root.Last;
                if (view == null)
                {
                    throw new InvalidOperationException("Collection is empty.");
                }

                return view.Node.Value;
            }
        }

        [Pure]
        public FingerTree<T> AddLast(T value)
        {
            return new(this._root.Append(new(value)));
        }

        [Pure]
        public FingerTree<T> RemoveLast() => this.RemoveLast(out _);

        [Pure]
        public FingerTree<T> RemoveLast(out T last)
        {
            var view = this._root.Last;
            if (view == null)
            {
                throw new InvalidOperationException();
            }

            last = view.Node.Value;
            return new(view.Tree());
        }

        public T this[int index]
        {
            get
            {
                var node = this._root;
                if ((uint) index >= (uint) node.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return node.GetElementAt(index);
            }
        }

        [Pure]
        public FingerTree<T> SetItem(int index, T value)
        {
            var node = this._root;
            if ((uint) index >= (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new((StemNode<ItemNode>) node.SetElementAt(index, value));
        }

        [Pure]
        public FingerTree<T> Append(FingerTree<T> other)
        {
            return new(this._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, other._root));
        }

        [Pure]
        public FingerTree<T> Prepend(FingerTree<T> other)
        {
            return new(other._root.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, this._root));
        }

        [Pure]
        public FingerTree<T> Insert(int index, T element)
        {
            var node = this._root;
            if ((uint) index > (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException();
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

        [Pure]
        public FingerTree<T> InsertRange(int index, FingerTree<T> items)
        {
            var node = this._root;
            if ((uint) index > (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException();
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

        [Pure]
        public FingerTree<T> RemoveAt(int index)
        {
            var node = this._root;
            if ((uint) index >= (uint) node.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var split = node.Split(index);
            return new(split.LeftTree.ConcatWithMiddle(ReadOnlySpan<ItemNode>.Empty, split.RightTree));
        }

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

        public void ForEach(Action<T> action)
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
