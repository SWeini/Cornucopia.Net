namespace Cornucopia.DataStructures.Persistent
{
    public sealed class BinaryTree<T>
    {
        public static BinaryTree<T>? Empty => null;

        public BinaryTree(T value, BinaryTree<T>? leftChild, BinaryTree<T>? rightChild)
        {
            this.Value = value;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public T Value { get; }
        public BinaryTree<T>? LeftChild { get; }
        public BinaryTree<T>? RightChild { get; }
    }
}