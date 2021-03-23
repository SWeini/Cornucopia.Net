using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Cornucopia.DataStructures
{
    /// <summary>
    ///     A min-heap based on the pairing heap structure.
    /// </summary>
    /// <typeparam name="T">The type of elements to store in the heap.</typeparam>
    public class PairingHeap<T>
    {
        private readonly IComparer<T> _comparer;
        private OptimizedChildSiblingTreeNode<T>? _head;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PairingHeap{T}"/> class that is empty and uses the default comparer.
        /// </summary>
        public PairingHeap()
        {
            this._comparer = Comparer<T>.Default;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PairingHeap{T}"/> class that is empty.
        /// </summary>
        /// <param name="comparer">The comparer used to compare elements.</param>
        public PairingHeap(IComparer<T> comparer)
        {
            this._comparer = comparer;
        }

        /// <summary>
        ///     Gets a value indicating whether this heap is empty.
        /// </summary>
        /// <returns><c>true</c> if the heap is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty => this._head == null;

        /// <summary>
        ///     Counts the elements in the heap.
        /// </summary>
        /// <returns>The number of elements in the heap.</returns>
        [Pure]
        public int Count()
        {
            if (this._head == null)
            {
                return 0;
            }

            var result = 0;
            var queue = new DynamicArray<OptimizedChildSiblingTreeNode<T>>(true);
            queue.AddLast(this._head);

            while (queue.TryRemoveLast(out var next))
            {
                result++;
                if (next.RightSibling != null)
                {
                    queue.AddLast(next.RightSibling);
                }

                if (next.FirstChild != null)
                {
                    queue.AddLast(next.FirstChild);
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets or sets the value inside an element pointer.
        /// </summary>
        /// <param name="element">The pointer to the element.</param>
        /// <param name="value">The new value inside the element pointer.</param>
        /// <returns>The value inside <paramref name="element"/>.</returns>
        public T this[ElementPointer element]
        {
            get => element.Node.Value;
            set
            {
                var node = element.Node;
                this.Remove(node);
                node.Value = value;
                node.LeftSiblingOrParent = null;
                node.RightSibling = null;
                node.FirstChild = null;
                this.Merge(node);
            }
        }

        /// <summary>
        ///     Inserts an element into the heap.
        /// </summary>
        /// <param name="item">The element to insert into the heap.</param>
        /// <returns>The pointer to the inserted element.</returns>
        public ElementPointer Insert(T item)
        {
            var result = new OptimizedChildSiblingTreeNode<T>(item);
            this._head = this._head == null ? result : this.Meld(result, this._head);
            return new(result);
        }

        /// <summary>
        ///     A minimum element contained in the heap.
        /// </summary>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T Minimum
        {
            get
            {
                if (this._head == null)
                {
                    throw new InvalidOperationException("The heap is empty.");
                }

                return this._head.Value;
            }
        }

        /// <summary>
        ///     Extracts a minimum element from the heap.
        /// </summary>
        /// <returns>A minimum element that was contained in the heap.</returns>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T ExtractMinimum()
        {
            if (!this.TryExtractMinimum(out var result))
            {
                throw new InvalidOperationException("The heap is empty.");
            }

            return result;
        }

        /// <summary>
        ///     Extracts a minimum element from the heap.
        /// </summary>
        /// <param name="result">A minimum element that was contained in the heap.</param>
        /// <returns><c>true</c> if an element was removed from the heap; otherwise, <c>false</c>.</returns>
        public bool TryExtractMinimum([MaybeNullWhen(false)] out T result)
        {
            if (this._head == null)
            {
                result = default;
                return false;
            }

            result = this._head.Value;
            this._head = this.MergePairs(this._head.FirstChild);
            return true;
        }

        /// <summary>
        ///     Removes an element from the heap.
        /// </summary>
        /// <param name="element">The pointer to the element to be removed.</param>
        public void Remove(ElementPointer element)
        {
            this.Remove(element.Node);
        }

        private void Remove(OptimizedChildSiblingTreeNode<T> node)
        {
            Debug.Assert(this._head != null);
            if (this._head == node)
            {
                this._head = this.MergePairs(this._head.FirstChild);
                return;
            }

            var reference = node.LeftSiblingOrParent!;
            if (reference.FirstChild == node)
            {
                reference.FirstChild = node.RightSibling;
            }
            else
            {
                Debug.Assert(reference.RightSibling == node);
                reference.RightSibling = node.RightSibling;
            }

            if (node.FirstChild != null)
            {
                this._head = this.Meld(this._head, this.MergePairs(node.FirstChild));
            }
        }

        /// <summary>
        ///     Destructively merges another heap.
        /// </summary>
        /// <param name="heap">Another heap with elements to add to this heap.</param>
        /// <remarks><paramref name="heap"/> is emptied in the process.</remarks>
        public void Merge(PairingHeap<T> heap)
        {
            if (heap._head == null)
            {
                return;
            }

            this.Merge(heap._head);
            heap._head = null;
        }

        private void Merge(OptimizedChildSiblingTreeNode<T> node)
        {
            this._head = this._head == null ? node : this.Meld(node, this._head);

        }

        [return: NotNullIfNotNull("firstChild")]
        private OptimizedChildSiblingTreeNode<T>? MergePairs(OptimizedChildSiblingTreeNode<T>? firstChild)
        {
            if (firstChild == null)
            {
                return null;
            }

            var last = PassOne(firstChild);
            var result = PassTwo(last);
            result.LeftSiblingOrParent = null;
            result.RightSibling = null;
            return result;

            OptimizedChildSiblingTreeNode<T> PassOne(OptimizedChildSiblingTreeNode<T> current)
            {
                OptimizedChildSiblingTreeNode<T>? previous = null;
                while (current.RightSibling != null)
                {
                    var other = current.RightSibling;
                    var next = other.RightSibling;
                    var melded = this.Meld(current, other);
                    melded.LeftSiblingOrParent = previous;
                    if (next == null)
                    {
                        return melded;
                    }

                    previous = melded;
                    current = next;
                }

                current.LeftSiblingOrParent = previous;
                return current;
            }

            OptimizedChildSiblingTreeNode<T> PassTwo(OptimizedChildSiblingTreeNode<T> current)
            {
                var merging = current.LeftSiblingOrParent;
                while (merging != null)
                {
                    var next = merging.LeftSiblingOrParent;
                    current = this.Meld(current, merging);
                    merging = next;
                }

                return current;
            }
        }

        private OptimizedChildSiblingTreeNode<T> Meld(OptimizedChildSiblingTreeNode<T> head1, OptimizedChildSiblingTreeNode<T> head2)
        {
            if (this._comparer.Compare(head1.Value, head2.Value) < 0)
            {
                MeldInto(head1, head2);
                return head1;
            }
            else
            {
                MeldInto(head2, head1);
                return head2;
            }

            static void MeldInto(OptimizedChildSiblingTreeNode<T> parent, OptimizedChildSiblingTreeNode<T> child)
            {
                child.LeftSiblingOrParent = parent;
                var oldFirstChild = parent.FirstChild;
                if (oldFirstChild != null)
                {
                    oldFirstChild.LeftSiblingOrParent = child;
                }

                child.RightSibling = oldFirstChild;
                parent.FirstChild = child;
            }
        }

        /// <summary>
        ///     A pointer to an element in the heap.
        /// </summary>
        public readonly struct ElementPointer
        {
            internal ElementPointer(OptimizedChildSiblingTreeNode<T> node)
            {
                this.Node = node;
            }

            internal OptimizedChildSiblingTreeNode<T> Node { get; }
        }
    }
}