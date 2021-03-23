namespace Cornucopia.DataStructures
{
    internal class OptimizedChildSiblingTreeNode<T>
    {
        public OptimizedChildSiblingTreeNode(T value)
        {
            this.Value = value;
        }

        public OptimizedChildSiblingTreeNode<T>? LeftSiblingOrParent { get; set; }
        public OptimizedChildSiblingTreeNode<T>? RightSibling { get; set; }
        public OptimizedChildSiblingTreeNode<T>? FirstChild { get; set; }

        public T Value { get; set; }
    }
}