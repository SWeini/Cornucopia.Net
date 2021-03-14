using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures
{
    internal static class BinaryHeapUtilies
    {
        public static int GetLeftChildIndex(int idx)
        {
            return (idx << 1) + 1;
        }

        public static int GetRightChildIndex(int idx)
        {
            return (idx << 1) + 2;
        }

        public static int GetParentIndex(int idx)
        {
            return (idx - 1) >> 1;
        }

        public static void Down<T>(Span<T> arr, int idx, IComparer<T> comparer)
        {
            var j = LeafSearch(arr, idx, comparer);
            var item = arr[idx];
            while (comparer.Compare(item, arr[j]) < 0)
            {
                j = GetParentIndex(j);
            }

            while (j > idx)
            {
                ref var itemJ = ref arr[j];
                var tmp = itemJ;
                itemJ = item;
                item = tmp;
                j = GetParentIndex(j);
            }

            arr[j] = item;
        }

        private static int LeafSearch<T>(ReadOnlySpan<T> arr, int idx, IComparer<T> comparer)
        {
            var j = idx;
            while (true)
            {
                var rightChildIndex = GetRightChildIndex(j);
                if (rightChildIndex >= arr.Length)
                {
                    break;
                }

                if (comparer.Compare(arr[rightChildIndex - 1], arr[rightChildIndex]) < 0)
                {
                    j = rightChildIndex - 1;
                }
                else
                {
                    j = rightChildIndex;
                }
            }

            var leftChildIndex = GetLeftChildIndex(j);
            if (leftChildIndex < arr.Length)
            {
                j = leftChildIndex;
            }

            return j;
        }

        public static void Up<T>(Span<T> arr, int idx, IComparer<T> comparer)
        {
            var item = arr[idx];
            while (idx > 0)
            {
                var parentIdx = GetParentIndex(idx);
                var parent = arr[parentIdx];
                if (comparer.Compare(item, parent) < 0)
                {
                    arr[idx] = parent;
                    idx = parentIdx;
                }
                else
                {
                    break;
                }
            }

            arr[idx] = item;
        }
    }
}