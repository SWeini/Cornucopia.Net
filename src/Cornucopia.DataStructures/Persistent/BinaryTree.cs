using System;

namespace Cornucopia.DataStructures.Persistent
{
    public sealed class BinaryTree<T>
    {
        public static BinaryTree<T>? Empty => null;

        public BinaryTree(T value)
        {
            this.Value = value;
        }

        public BinaryTree(BinaryTree<T>? leftChild, BinaryTree<T>? rightChild, T value)
        {
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
            this.Value = value;
        }

        public BinaryTree<T>? LeftChild { get; }
        public BinaryTree<T>? RightChild { get; }
        public T Value { get; }

        public void ForEachPreOrder(Action<T> action)
        {
            action(this.Value);
            this.LeftChild?.ForEachPreOrder(action);
            this.RightChild?.ForEachPreOrder(action);
        }

        public void ForEachInOrder(Action<T> action)
        {
            this.LeftChild?.ForEachInOrder(action);
            action(this.Value);
            this.RightChild?.ForEachInOrder(action);
        }

        public void ForEachPostOrder(Action<T> action)
        {
            this.LeftChild?.ForEachPostOrder(action);
            this.RightChild?.ForEachPostOrder(action);
            action(this.Value);
        }
    }
}