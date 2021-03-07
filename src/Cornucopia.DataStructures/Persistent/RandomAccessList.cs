using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cornucopia.DataStructures.Persistent
{
    public readonly struct RandomAccessList<T>
    {
        public static RandomAccessList<T> Empty => default;

        private readonly LinkedList<Node>? _root;

        private RandomAccessList(LinkedList<Node>? root)
        {
            this._root = root;
        }

        public bool IsEmpty => this._root.IsEmpty();

        public bool Any => this._root.Any();

        [Pure]
        public int Count()
        {
            var list = this._root;
            var result = 0;
            while (list.Any())
            {
                result += list.Head.Count;
                list = list.Tail;
            }

            return result;
        }

        public T First
        {
            get
            {
                this._root.EnsureNotEmpty();
                return this._root.Head.Tree.Value;
            }
        }

        [Pure]
        public RandomAccessList<T> AddFirst(T value)
        {
            var first = this._root;
            if (first.Any())
            {
                var second = first.Tail;
                if (second.Any())
                {
                    if (first.Head.Count == second.Head.Count)
                    {
                        var combinedTree = new BinaryTree<T>(first.Head.Tree, second.Head.Tree, value);
                        var combinedNode = new Node(combinedTree, first.Head.Count * 2 + 1);
                        return new(second.Tail.Prepend(combinedNode));
                    }
                }
            }

            var singleValueTree = BinaryTree.Create(value);
            var singleValueNode = new Node(singleValueTree, 1);
            return new(first.Prepend(singleValueNode));
        }

        [Pure]
        public RandomAccessList<T> RemoveFirst(out T value)
        {
            this._root.EnsureNotEmpty();
            var tree = this._root.Head.Tree;
            value = tree.Value;
            if (this._root.Head.Count == 1)
            {
                return new(this._root.Tail);
            }

            var halfCount = this._root.Head.Count / 2;
            return new(this._root
                .Tail
                .Prepend(new(tree.RightChild!, halfCount))
                .Prepend(new(tree.LeftChild!, halfCount)));
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var list = this._root;
                while (list.Any())
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

        public void ForEach(Action<T> action)
        {
            var list = this._root;
            while (list.Any())
            {
                BinaryTree.ForEachPreOrder(list.Head.Tree, action);
                list = list.Tail;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var list = this._root;
            while (list.Any())
            {
                var node = list.Head;
                list = list.Tail;

                var todo = LinkedList.Create(node.Tree);
                while (todo.Any())
                {
                    var tree = todo.Head;
                    yield return tree.Value;
                    todo = todo.Tail;

                    if (tree.RightChild.Any())
                    {
                        todo = todo.Prepend(tree.RightChild);
                    }

                    if (tree.LeftChild.Any())
                    {
                        todo = todo.Prepend(tree.LeftChild);
                    }
                }
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