using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cornucopia.DataStructures.Persistent
{
    partial struct FingerTree<T>
    {
        public abstract class BaseNode
        {
            protected BaseNode(int count)
            {
                this.Count = count;
            }

            public int Count { get; }
            public abstract ref readonly T GetElementAt(int index);
            public abstract BaseNode SetElementAt(int index, in T value);
            public abstract void ForEach(Action<T> action);
        }

        public class View<TNode>
            where TNode : BaseNode
        {
            public View(TNode node, Func<StemNode<TNode>> tree)
            {
                this.Node = node;
                this.Tree = tree;
            }

            public TNode Node { get; }
            public Func<StemNode<TNode>> Tree { get; }
        }

        public readonly struct Split<TNode>
            where TNode : BaseNode
        {
            public Split(StemNode<TNode> leftTree, TNode node, StemNode<TNode> rightTree)
            {
                this.LeftTree = leftTree;
                this.Node = node;
                this.RightTree = rightTree;
            }

            public StemNode<TNode> LeftTree { get; }
            public TNode Node { get; }
            public StemNode<TNode> RightTree { get; }
        }

        public static TreeNode<TNode>[] Nodes<TNode>(TNode[] nodes)
            where TNode : BaseNode
        {
            Debug.Assert(nodes.Length >= 2);

            switch (nodes.Length)
            {
                case 2:
                    return new TreeNode<TNode>[] { new TreeNode2<TNode>(nodes[0], nodes[1]) };
                case 3:
                    return new TreeNode<TNode>[] { new TreeNode3<TNode>(nodes[0], nodes[1], nodes[2]) };
                case 4:
                    return new TreeNode<TNode>[] { new TreeNode2<TNode>(nodes[0], nodes[1]), new TreeNode2<TNode>(nodes[2], nodes[3]) };
                case 5:
                    return new TreeNode<TNode>[] { new TreeNode3<TNode>(nodes[0], nodes[1], nodes[2]), new TreeNode2<TNode>(nodes[3], nodes[4]) };
                case 6:
                    return new TreeNode<TNode>[] { new TreeNode3<TNode>(nodes[0], nodes[1], nodes[2]), new TreeNode3<TNode>(nodes[3], nodes[4], nodes[5]) };
            }

            var result = new TreeNode<TNode>[nodes.Length / 2];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new TreeNode2<TNode>(nodes[2 * i], nodes[2 * i + 1]);
            }

            if (nodes.Length % 2 == 1)
            {
                var i = result.Length - 1;
                result[i] = new TreeNode3<TNode>(nodes[2 * i], nodes[2 * i + 1], nodes[2 * i + 2]);
            }

            return result;
        }

        public abstract class StemNode<TNode> : BaseNode
            where TNode : BaseNode
        {
            protected StemNode(int count)
                : base(count)
            {
            }

            public abstract StemNode<TNode> Append(TNode x);
            public abstract StemNode<TNode> Prepend(TNode x);
            public abstract View<TNode>? First { get; }
            public abstract View<TNode>? Last { get; }
            public abstract StemNode<TNode> ConcatWithMiddle(ReadOnlySpan<TNode> middle, StemNode<TNode> right);
            public abstract StemNode<TNode> ConcatWithMiddle(DeepNode<TNode> left, ReadOnlySpan<TNode> middle);
            public abstract Split<TNode> Split(int index);
        }

        public sealed class EmptyNode<TNode> : StemNode<TNode>
            where TNode : BaseNode
        {
            public static EmptyNode<TNode> Instance { get; } = new();

            public static Func<StemNode<TNode>> InstanceFunc { get; } = () => Instance;

            private EmptyNode()
                : base(0)
            {
            }

            [ExcludeFromCodeCoverage]
            public override ref readonly T GetElementAt(int index)
            {
                throw new InvalidOperationException();
            }

            [ExcludeFromCodeCoverage]
            public override BaseNode SetElementAt(int index, in T value)
            {
                throw new InvalidOperationException();
            }

            public override void ForEach(Action<T> action)
            {
            }

            public override StemNode<TNode> Append(TNode x)
            {
                return new SingleNode<TNode>(x);
            }

            public override StemNode<TNode> Prepend(TNode x)
            {
                return new SingleNode<TNode>(x);
            }

            public override View<TNode>? First => null;
            public override View<TNode>? Last => null;
            public override StemNode<TNode> ConcatWithMiddle(ReadOnlySpan<TNode> middle, StemNode<TNode> right)
            {
                for (var i = middle.Length - 1; i >= 0; i--)
                {
                    right = right.Prepend(middle[i]);
                }

                return right;
            }

            public override StemNode<TNode> ConcatWithMiddle(DeepNode<TNode> left, ReadOnlySpan<TNode> middle)
            {
                StemNode<TNode> l = left;
                foreach (var m in middle)
                {
                    l = l.Append(m);
                }

                return l;
            }

            [ExcludeFromCodeCoverage]
            public override Split<TNode> Split(int index)
            {
                throw new InvalidOperationException();
            }
        }

        public sealed class SingleNode<TNode> : StemNode<TNode>
            where TNode : BaseNode
        {
            private readonly TNode _child;

            public SingleNode(TNode child)
                : base(child.Count)
            {
                this._child = child;
            }

            public override ref readonly T GetElementAt(int index)
            {
                return ref this._child.GetElementAt(index);
            }

            public override BaseNode SetElementAt(int index, in T value)
            {
                return new SingleNode<TNode>((TNode) this._child.SetElementAt(index, value));
            }

            public override void ForEach(Action<T> action)
            {
                this._child.ForEach(action);
            }

            public override StemNode<TNode> Append(TNode x)
            {
                return new DeepNode<TNode>(new TreeNode1<TNode>(this._child), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(x));
            }

            public override StemNode<TNode> Prepend(TNode x)
            {
                return new DeepNode<TNode>(new TreeNode1<TNode>(x), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(this._child));
            }

            public override View<TNode> First => new(this._child, EmptyNode<TNode>.InstanceFunc);
            public override View<TNode> Last => new(this._child, EmptyNode<TNode>.InstanceFunc);

            public override StemNode<TNode> ConcatWithMiddle(ReadOnlySpan<TNode> middle, StemNode<TNode> right)
            {
                return EmptyNode<TNode>.Instance.ConcatWithMiddle(middle, right).Prepend(this._child);
            }

            public override StemNode<TNode> ConcatWithMiddle(DeepNode<TNode> left, ReadOnlySpan<TNode> middle)
            {
                return EmptyNode<TNode>.Instance.ConcatWithMiddle(left, middle).Append(this._child);
            }

            public override Split<TNode> Split(int index)
            {
                return new(EmptyNode<TNode>.Instance, this._child, EmptyNode<TNode>.Instance);
            }
        }

        public sealed class DeepNode<TNode> : StemNode<TNode>
            where TNode : BaseNode
        {
            private readonly TreeNode<TNode> _prefix;
            private Func<StemNode<TreeNode<TNode>>>? _deeperFunc;
            private StemNode<TreeNode<TNode>>? _deeperValue;
            private readonly TreeNode<TNode> _suffix;

            private StemNode<TreeNode<TNode>> Deeper
            {
                get
                {
                    if (this._deeperValue == null)
                    {
                        lock (this)
                        {
                            if (this._deeperFunc != null)
                            {
                                this._deeperValue = this._deeperFunc();
                                this._deeperFunc = null;
                            }
                        }
                    }

                    return this._deeperValue!;
                }
            }

            public DeepNode(TreeNode<TNode> prefix, Func<StemNode<TreeNode<TNode>>> deeper, int deeperCount, TreeNode<TNode> suffix)
                : base(prefix.Count + deeperCount + suffix.Count)
            {
                this._prefix = prefix;
                this._deeperFunc = deeper;
                this._suffix = suffix;
            }

            public DeepNode(TreeNode<TNode> prefix, StemNode<TreeNode<TNode>> deeper, TreeNode<TNode> suffix)
                : base(prefix.Count + deeper.Count + suffix.Count)
            {
                this._prefix = prefix;
                this._deeperValue = deeper;
                this._suffix = suffix;
            }

            public override ref readonly T GetElementAt(int index)
            {
                var m0 = this._prefix.Count;
                if (index < m0)
                {
                    return ref this._prefix.GetElementAt(index);
                }

                var m1 = m0 + this.Deeper.Count;
                if (index < m1)
                {
                    return ref this.Deeper.GetElementAt(index - m0);
                }

                return ref this._suffix.GetElementAt(index - m1);
            }

            public override BaseNode SetElementAt(int index, in T value)
            {
                var m0 = this._prefix.Count;
                if (index < m0)
                {
                    return new DeepNode<TNode>((TreeNode<TNode>) this._prefix.SetElementAt(index, value), this.Deeper, this._suffix);
                }

                var m1 = m0 + this.Deeper.Count;
                if (index < m1)
                {
                    return new DeepNode<TNode>(this._prefix, (StemNode<TreeNode<TNode>>) this.Deeper.SetElementAt(index - m0, value), this._suffix);
                }

                return new DeepNode<TNode>(this._prefix, this.Deeper, (TreeNode<TNode>) this._suffix.SetElementAt(index - m1, value));
            }

            public override void ForEach(Action<T> action)
            {
                this._prefix.ForEach(action);
                this.Deeper.ForEach(action);
                this._suffix.ForEach(action);
            }

            public override StemNode<TNode> Append(TNode x)
            {
                var suffix = this._suffix;
                if (suffix.Length == 4)
                {
                    var inner = suffix.RemoveLast();
                    return new DeepNode<TNode>(this._prefix, () => this.Deeper.Append(inner), this.Deeper.Count + inner.Count, new TreeNode2<TNode>(suffix.Last, x));
                }

                return new DeepNode<TNode>(this._prefix, this.Deeper, this._suffix.Append(x));
            }

            public override StemNode<TNode> Prepend(TNode x)
            {
                var prefix = this._prefix;
                if (prefix.Length == 4)
                {
                    var inner = prefix.RemoveFirst();
                    return new DeepNode<TNode>(new TreeNode2<TNode>(x, prefix.First), () => this.Deeper.Prepend(inner), this.Deeper.Count + inner.Count, this._suffix);
                }

                return new DeepNode<TNode>(prefix.Prepend(x), this.Deeper, this._suffix);
            }

            public override View<TNode> First
            {
                get
                {
                    var prefix = this._prefix;
                    if (prefix.Length == 1)
                    {
                        return new View<TNode>(prefix.First, () =>
                        {
                            var deeper = this.Deeper.First;
                            var tree = deeper != null ? new DeepNode<TNode>(deeper.Node, deeper.Tree, this.Deeper.Count - deeper.Node.Count, this._suffix) : this._suffix.ToTree();
                            return tree;
                        });
                    }

                    return new View<TNode>(prefix.First, () => new DeepNode<TNode>(prefix.RemoveFirst(), this.Deeper, this._suffix));
                }
            }

            public override View<TNode> Last
            {
                get
                {
                    var suffix = this._suffix;
                    if (suffix.Length == 1)
                    {
                        return new View<TNode>(suffix.Last, () =>
                        {
                            var deeper = this.Deeper.Last;
                            var tree = deeper != null ? new DeepNode<TNode>(this._prefix, deeper.Tree, this.Deeper.Count - deeper.Node.Count, deeper.Node) : this._prefix.ToTree();
                            return tree;
                        });
                    }

                    return new View<TNode>(suffix.Last, () => new DeepNode<TNode>(this._prefix, this.Deeper, suffix.RemoveLast()));
                }
            }

            public override StemNode<TNode> ConcatWithMiddle(ReadOnlySpan<TNode> middle, StemNode<TNode> right)
            {
                return right.ConcatWithMiddle(this, middle);
            }

            public override StemNode<TNode> ConcatWithMiddle(DeepNode<TNode> left, ReadOnlySpan<TNode> middle)
            {
                var nodes = new TNode[left._suffix.Length + middle.Length + this._prefix.Length];
                left._suffix.AsSpan().CopyTo(nodes.AsSpan(0, left._suffix.Length));
                middle.CopyTo(nodes.AsSpan(left._suffix.Length, middle.Length));
                this._prefix.AsSpan().CopyTo(nodes.AsSpan(left._suffix.Length + middle.Length, this._prefix.Length));
                var mid = Nodes(nodes);
                var deeper = left.Deeper.ConcatWithMiddle(mid, this.Deeper);
                return new DeepNode<TNode>(left._prefix, deeper, this._suffix);
            }

            public override Split<TNode> Split(int index)
            {
                var prefixCount = this._prefix.Count;
                if (index < prefixCount)
                {
                    var span = this._prefix.AsSpan();
                    var splitIndex = FindSplitIndex(span, index);
                    var leftTree = ChunkToTree(span.Slice(0, splitIndex));
                    var rightTree = Create(TreeNode<TNode>.Create(span.Slice(splitIndex + 1)), this.Deeper, this._suffix);
                    return new Split<TNode>(leftTree, span[splitIndex], rightTree);
                }

                index -= prefixCount;
                var deeperCount = this.Deeper.Count;
                if (index < deeperCount)
                {
                    var split = this.Deeper.Split(index);
                    var span = split.Node.AsSpan();
                    var splitIndex = FindSplitIndex(span, index - split.LeftTree.Count);
                    var leftTree = Create(this._prefix, split.LeftTree, TreeNode<TNode>.Create(span.Slice(0, splitIndex)));
                    var rightTree = Create(TreeNode<TNode>.Create(span.Slice(splitIndex + 1)), split.RightTree, this._suffix);
                    return new Split<TNode>(leftTree, span[splitIndex], rightTree);
                }

                index -= deeperCount;
                {
                    var span = this._suffix.AsSpan();
                    var splitIndex = FindSplitIndex(span, index);
                    var leftTree = Create(this._prefix, this.Deeper, TreeNode<TNode>.Create(span.Slice(0, splitIndex)));
                    var rightTree = ChunkToTree(span.Slice(splitIndex + 1));
                    return new Split<TNode>(leftTree, span[splitIndex], rightTree);
                }
            }

            private static int FindSplitIndex(ReadOnlySpan<TNode> nodes, int index)
            {
                for (var i = 0; ; i++)
                {
                    index -= nodes[i].Count;
                    if (index < 0)
                    {
                        return i;
                    }
                }
            }

            private static StemNode<TNode> Create(TreeNode<TNode>? prefix, StemNode<TreeNode<TNode>> deeper, TreeNode<TNode>? suffix)
            {
                if (prefix == null)
                {
                    var view = deeper.First;
                    if (view == null)
                    {
                        // never called with empty prefix and empty suffix
                        return suffix!.ToTree();
                    }

                    prefix = view.Node;
                    deeper = view.Tree();
                }

                if (suffix == null)
                {
                    var view = deeper.Last;
                    if (view == null)
                    {
                        return prefix.ToTree();
                    }

                    suffix = view.Node;
                    deeper = view.Tree();
                }

                return new DeepNode<TNode>(prefix, deeper, suffix);
            }

            private static StemNode<TNode> ChunkToTree(ReadOnlySpan<TNode> nodes)
            {
                switch (nodes.Length)
                {
                    case 0:
                        return EmptyNode<TNode>.Instance;
                    case 1:
                        return new SingleNode<TNode>(nodes[0]);
                    case 2:
                        return new DeepNode<TNode>(new TreeNode1<TNode>(nodes[0]), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(nodes[1]));
                    case 3:
                        return new DeepNode<TNode>(new TreeNode2<TNode>(nodes[0], nodes[1]), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(nodes[2]));
                    case 4:
                        return new DeepNode<TNode>(new TreeNode2<TNode>(nodes[0], nodes[1]), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode2<TNode>(nodes[2], nodes[3]));
                }

                StemNode<TNode> result = EmptyNode<TNode>.Instance;
                foreach (var node in nodes)
                {
                    result = result.Append(node);
                }

                return result;
            }
        }

        public abstract class TreeNode<TNode> : BaseNode
            where TNode : BaseNode
        {
            public int Length { get; set; }
            private readonly TNode _child0;

            protected TreeNode(int count, int length, TNode child0)
                : base(count)
            {
                this.Length = length;
                this._child0 = child0;
            }

            private TNode Child1 => Unsafe.Add(ref Unsafe.AsRef(this._child0), 1);
            private TNode Child2 => Unsafe.Add(ref Unsafe.AsRef(this._child0), 2);
            private TNode Child3 => Unsafe.Add(ref Unsafe.AsRef(this._child0), 3);

            public override ref readonly T GetElementAt(int index)
            {
                var children = this.AsSpan();
                for (var i = 0; ; i++)
                {
                    if (index < children[i].Count)
                    {
                        return ref children[i].GetElementAt(index);
                    }

                    index -= children[i].Count;
                }
            }

            public override BaseNode SetElementAt(int index, in T value)
            {
                var children = this.ToArray();
                for (var i = 0; ; i++)
                {
                    var child = children[i];
                    if (index < child.Count)
                    {
                        children[i] = (TNode) child.SetElementAt(index, value);
                        return Create(children)!;
                    }

                    index -= child.Count;
                }
            }

            public override void ForEach(Action<T> action)
            {
                for (var i = 0; i < this.Length; i++)
                {
                    Unsafe.Add(ref Unsafe.AsRef(this._child0), i).ForEach(action);
                }
            }

            public TNode First => this._child0;
            public TNode Last => Unsafe.Add(ref Unsafe.AsRef(this._child0), this.Length - 1);

            public ReadOnlySpan<TNode> AsSpan()
            {
#if NETCOREAPP3_1
                return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(this._child0), this.Length);
#else
                return this.ToArray();
#endif
            }

            public TNode[] ToArray()
            {
                switch (this.Length)
                {
                    case 1:
                        return new[] { this._child0 };
                    case 2:
                        return new[] { this._child0, this.Child1 };
                    case 3:
                        return new[] { this._child0, this.Child1, this.Child2 };
                    default:
                        return new[] { this._child0, this.Child1, this.Child2, this.Child3 };
                }
            }

            public TreeNode<TNode> Append(TNode x)
            {
                switch (this.Length)
                {
                    case 1:
                        return new TreeNode2<TNode>(this._child0, x);
                    case 2:
                        return new TreeNode3<TNode>(this._child0, this.Child1, x);
                    default:
                        return new TreeNode4<TNode>(this._child0, this.Child1, this.Child2, x);
                }
            }

            public TreeNode<TNode> Prepend(TNode x)
            {
                switch (this.Length)
                {
                    case 1:
                        return new TreeNode2<TNode>(x, this._child0);
                    case 2:
                        return new TreeNode3<TNode>(x, this._child0, this.Child1);
                    default:
                        return new TreeNode4<TNode>(x, this._child0, this.Child1, this.Child2);
                }
            }

            public TreeNode<TNode> RemoveFirst()
            {
                switch (this.Length)
                {
                    case 2:
                        return new TreeNode1<TNode>(this.Child1);
                    case 3:
                        return new TreeNode2<TNode>(this.Child1, this.Child2);
                    default:
                        return new TreeNode3<TNode>(this.Child1, this.Child2, this.Child3);
                }
            }

            public TreeNode<TNode> RemoveLast()
            {
                switch (this.Length)
                {
                    case 2:
                        return new TreeNode1<TNode>(this._child0);
                    case 3:
                        return new TreeNode2<TNode>(this._child0, this.Child1);
                    default:
                        return new TreeNode3<TNode>(this._child0, this.Child1, this.Child2);
                }
            }

            public StemNode<TNode> ToTree()
            {
                switch (this.Length)
                {
                    case 1:
                        return new SingleNode<TNode>(this._child0);
                    case 2:
                        return new DeepNode<TNode>(new TreeNode1<TNode>(this._child0), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(this.Child1));
                    case 3:
                        return new DeepNode<TNode>(new TreeNode2<TNode>(this._child0, this.Child1), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode1<TNode>(this.Child2));
                    default:
                        return new DeepNode<TNode>(new TreeNode2<TNode>(this._child0, this.Child1), EmptyNode<TreeNode<TNode>>.Instance, new TreeNode2<TNode>(this.Child2, this.Child3));
                }
            }

            public static TreeNode<TNode>? Create(ReadOnlySpan<TNode> nodes)
            {
                switch (nodes.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return new TreeNode1<TNode>(nodes[0]);
                    case 2:
                        return new TreeNode2<TNode>(nodes[0], nodes[1]);
                    case 3:
                        return new TreeNode3<TNode>(nodes[0], nodes[1], nodes[2]);
                    default:
                        return new TreeNode4<TNode>(nodes[0], nodes[1], nodes[2], nodes[3]);
                }
            }
        }

        public sealed class TreeNode1<TNode> : TreeNode<TNode>
            where TNode : BaseNode
        {
            public TreeNode1(TNode child0)
                : base(child0.Count, 1, child0)
            {
            }
        }

        [SuppressMessage("Microsoft.CodeQuality", "IDE0052:RemoveUnreadPrivateMember")]
        public sealed class TreeNode2<TNode> : TreeNode<TNode>
        where TNode : BaseNode
        {
            private readonly TNode _child1;

            public TreeNode2(TNode child0, TNode child1)
                : base(child0.Count + child1.Count, 2, child0)
            {
                this._child1 = child1;
            }
        }

        [SuppressMessage("Microsoft.CodeQuality", "IDE0052:RemoveUnreadPrivateMember")]
        public sealed class TreeNode3<TNode> : TreeNode<TNode>
        where TNode : BaseNode
        {
            private readonly TNode _child1;
            private readonly TNode _child2;

            public TreeNode3(TNode child0, TNode child1, TNode child2)
                : base(child0.Count + child1.Count + child2.Count, 3, child0)
            {
                this._child1 = child1;
                this._child2 = child2;
            }
        }

        [SuppressMessage("Microsoft.CodeQuality", "IDE0052:RemoveUnreadPrivateMember")]
        public sealed class TreeNode4<TNode> : TreeNode<TNode>
        where TNode : BaseNode
        {
            private readonly TNode _child1;
            private readonly TNode _child2;
            private readonly TNode _child3;

            public TreeNode4(TNode child0, TNode child1, TNode child2, TNode child3)
                : base(child0.Count + child1.Count + child2.Count + child3.Count, 4, child0)
            {
                this._child1 = child1;
                this._child2 = child2;
                this._child3 = child3;
            }
        }

        public sealed class ItemNode : BaseNode
        {
            private readonly T _value;

            public ItemNode(T value)
                : base(1)
            {
                this._value = value;
            }

            public T Value => this._value;

            public override ref readonly T GetElementAt(int index)
            {
                return ref this._value;
            }

            public override BaseNode SetElementAt(int index, in T value)
            {
                return new ItemNode(value);
            }

            public override void ForEach(Action<T> action)
            {
                action(this._value);
            }
        }
    }
}
