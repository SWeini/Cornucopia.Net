﻿using System;
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
        ///     Gets the element inside an element pointer.
        /// </summary>
        /// <param name="element">The pointer to the element.</param>
        /// <returns>The element referenced by the element pointer.</returns>
        public T this[ElementPointer element] => element.Node.Value;

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
            Debug.Assert(this._head != null);
            var node = element.Node;
            if (this._head == node)
            {
                this._head = this.MergePairs(this._head.FirstChild);
                return;
            }

            Cut(node);
            if (node.FirstChild != null)
            {
                this._head = this.Meld(this._head!, this.MergePairs(node.FirstChild));
            }
        }

        /// <summary>
        ///     Decreases the element inside a specified element pointer.
        /// </summary>
        /// <param name="element">The pointer to the element to decrease.</param>
        /// <param name="item">The new element.</param>
        /// <remarks>It is undefined behavior to increase the element.</remarks>
        public void Decrease(ElementPointer element, T item)
        {
            Debug.Assert(this._head != null);
            var node = element.Node;
            if (this._head == node)
            {
                node.Value = item;
                return;
            }

            Cut(node);
            node.LeftSiblingOrParent = null;
            node.RightSibling = null;
            node.Value = item;
            this._head = this.Meld(this._head!, node);
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

            this._head = this._head == null ? heap._head : this.Meld(heap._head, this._head);
            heap._head = null;
        }

        private static void Cut(OptimizedChildSiblingTreeNode<T> node)
        {
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
            /// <summary>
            ///     Gets the undefined pointer.
            /// </summary>
            /// <value>The undefined pointer.</value>
            public static ElementPointer Undefined => default;

            /// <summary>
            ///     Gets a value indicating whether this pointer is undefined.
            /// </summary>
            /// <returns><c>true</c> if the pointer is undefined; otherwise, <c>false</c>.</returns>
            public bool IsUndefined => this._node == null;

            private readonly OptimizedChildSiblingTreeNode<T>? _node;

            internal ElementPointer(OptimizedChildSiblingTreeNode<T> node)
            {
                this._node = node;
            }

            internal OptimizedChildSiblingTreeNode<T> Node => this._node!;
        }
    }

    /// <summary>
    ///     A min-heap based on the pairing heap structure.
    /// </summary>
    /// <typeparam name="TKey">The type of elements to store in the heap.</typeparam>
    /// <typeparam name="TValue">The type of priorities in the heap.</typeparam>
    public class PairingHeap<TKey, TValue> : PairingHeap<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PairingHeap{TKey,TValue}"/> class that is empty.
        /// </summary>
        /// <param name="comparer">The comparer used to compare priorities.</param>
        public PairingHeap(IComparer<TValue> comparer)
            : base(new ValueComparer(comparer))
        {
        }

        /// <summary>
        ///     Inserts an element into the heap.
        /// </summary>
        /// <param name="item">The element to insert into the heap.</param>
        /// <param name="priority">The priority of the inserted element.</param>
        /// <returns>The pointer to the inserted element.</returns>
        public ElementPointer Insert(TKey item, TValue priority)
        {
            return this.Insert(new KeyValuePair<TKey, TValue>(item, priority));
        }

        /// <summary>
        ///     Decreases the priority of a specified element.
        /// </summary>
        /// <param name="element">The pointer to the element to decrease.</param>
        /// <param name="priority">The new priority of the element.</param>
        /// <remarks>It is undefined behavior to increase the priority.</remarks>
        public void Decrease(ElementPointer element, TValue priority)
        {
            this.Decrease(element, new KeyValuePair<TKey, TValue>(this[element].Key, priority));
        }

        private class ValueComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            private readonly IComparer<TValue> _comparer;

            public ValueComparer(IComparer<TValue> comparer)
            {
                this._comparer = comparer;
            }

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return this._comparer.Compare(x.Value, y.Value);
            }
        }
    }
}