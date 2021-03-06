namespace Cornucopia.DataStructures.Persistent
{
    public sealed class BinaryTree<T>
    {
        public static BinaryTree<T>? Empty => null;

        public BinaryTree(BinaryTree<T>? leftChild, BinaryTree<T>? rightChild, T value)
        {
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
            this.Value = value;
        }

        public BinaryTree<T>? LeftChild { get; }
        public BinaryTree<T>? RightChild { get; }
        public T Value { get; }
    }
}