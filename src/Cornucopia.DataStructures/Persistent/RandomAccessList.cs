using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures.Persistent
{
    public readonly struct RandomAccessList<T>
    {
        public static RandomAccessList<T> Empty => default;

        private readonly LinkedList<Node>? root;

        private RandomAccessList(LinkedList<Node>? root)
        {
            this.root = root;
        }

        public bool IsEmpty => this.root.IsEmpty();

        public bool Any => this.root.Any();

        public int Count() => Count(this.root);

        public T First => GetFirst(this.root);

        public RandomAccessList<T> AddFirst(T value) => AddFirst(this.root, value);

        public RandomAccessList<T> RemoveFirst(out T value) => RemoveFirst(this.root, out value);

        public T this[int index] => GetNthNode(this.root, index).Value;

        public void ForEach(Action<T> action) => ForEach(this.root, action);

        public IEnumerator<T> GetEnumerator()
        {
            var list = this.root;
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

        private static int Count(LinkedList<Node>? list)
        {
            var result = 0;
            while (list.Any())
            {
                result += list.Head.Count;
                list = list.Tail;
            }

            return result;
        }

        private static T GetFirst(LinkedList<Node>? root)
        {
            root.EnsureNotEmpty();
            return root.Head.Tree.Value;
        }

        private static RandomAccessList<T> AddFirst(LinkedList<Node>? root, T value)
        {
            var first = root;
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

        private static RandomAccessList<T> RemoveFirst(LinkedList<Node>? node, out T value)
        {
            node.EnsureNotEmpty();
            var tree = node.Head.Tree;
            value = tree.Value;
            if (node.Head.Count == 1)
            {
                return new(node.Tail);
            }

            var halfCount = node.Head.Count / 2;
            return new(node
                .Tail
                .Prepend(new(tree.RightChild!, halfCount))
                .Prepend(new(tree.LeftChild!, halfCount)));
        }

        private static BinaryTree<T> GetNthNode(LinkedList<Node>? list, int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

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
                        return tree;
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

        private static void ForEach(LinkedList<Node>? list, Action<T> action)
        {
            while (list.Any())
            {
                BinaryTree.ForEachPreOrder(list.Head.Tree, action);
                list = list.Tail;
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