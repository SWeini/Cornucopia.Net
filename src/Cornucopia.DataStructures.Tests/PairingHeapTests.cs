using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class PairingHeapTests
    {
        [Test]
        public void Ctor_DefaultComparer_CountIsZero()
        {
            var heap = new PairingHeap<int>(Comparer<int>.Default);
            Assert.That(heap.Count(), Is.Zero);
        }

        [Test]
        public void Count_Empty_IsZero()
        {
            var heap = new PairingHeap<int>();
            Assert.That(heap.Count(), Is.Zero);
        }

        [Test]
        public void Insert_Empty_IsNotEmpty()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(0);
            Assert.That(heap.IsEmpty, Is.False);
        }

        [TestCase(5)]
        [TestCase(42)]
        public void Insert_MultipleTimesIntoEmpty_CountIsCorrect(int size)
        {
            var heap = new PairingHeap<int>();
            for (var i = 0; i < size; i++)
            {
                heap.Insert(0);
            }

            Assert.That(heap.Count(), Is.EqualTo(size));
        }

        [Test]
        public void Minimum_Empty_Throws()
        {
            var heap = new PairingHeap<int>();
            Assert.That(() => heap.Minimum, Throws.InvalidOperationException);
        }

        [Test]
        public void Minimum_SingleElement_ReturnsElement()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(42);
            Assert.That(heap.Minimum, Is.EqualTo(42));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0)]
        public void Minimum_ManyElements_IsCorrect(params int[] values)
        {
            var heap = new PairingHeap<int>();
            foreach (var value in values)
            {
                heap.Insert(value);
            }

            Assert.That(heap.Minimum, Is.EqualTo(values.Min()));
        }

        [Test]
        public void ExtractMinimum_Empty_Throws()
        {
            var heap = new PairingHeap<int>();
            Assert.That(() => heap.ExtractMinimum(), Throws.InvalidOperationException);
        }

        [Test]
        public void ExtractMinimum_SingleElement_ReturnsElement()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(42);
            Assert.That(heap.ExtractMinimum(), Is.EqualTo(42));
        }

        [Test]
        public void ExtractMinimum_SingleElement_RemovesElement()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(0);
            heap.ExtractMinimum();
            Assert.That(heap.IsEmpty, Is.True);
            Assert.That(() => heap.Minimum, Throws.InvalidOperationException);
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0)]
        [TestCase(5, 3, 4, 11, 8, 15)]
        public void ExtractMinimum_ManyElements_CanSortSequence(params int[] values)
        {
            var heap = new PairingHeap<int>();
            foreach (var value in values)
            {
                heap.Insert(value);
            }

            var sorted = new List<int>();
            while (!heap.IsEmpty)
            {
                sorted.Add(heap.ExtractMinimum());
            }

            Assert.That(sorted, Is.EqualTo(values.OrderBy(x => x).ToList()));
        }

        [Test]
        public void Remove_ManyElements_DecreasesCount()
        {
            var heap = new PairingHeap<int>();
            for (var i = 0; i < 15; i++)
            {
                heap.Insert(i);
            }

            var pointer = heap.Insert(5);
            heap.Remove(pointer);

            Assert.That(heap.Count(), Is.EqualTo(15));
        }

        [Test]
        public void Remove_MiddleElement_DecreasesCount()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(2);
            var pointer = heap.Insert(1);
            heap.Insert(0);
            heap.Remove(pointer);
            Assert.That(heap.Count(), Is.EqualTo(2));
        }

        [TestCase(5, 3, 4, 11, 8, 15)]
        public void Remove_AllElements_IsEmpty(params int[] values)
        {
            var heap = new PairingHeap<int>();
            var pointers = values.Select(heap.Insert).ToList();
            pointers.ForEach(heap.Remove);
            Assert.That(heap.IsEmpty, Is.True);
        }

        [Test]
        public void GetItem_SingleElement_ReturnsValue()
        {
            var heap = new PairingHeap<int>();
            var pointer = heap.Insert(42);
            Assert.That(heap[pointer], Is.EqualTo(42));
        }

        [Test]
        public void Decrease_MinimumElement_ExtractsAllElementsCorrectly()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(2);
            heap.Insert(1);
            var pointer = heap.Insert(0);
            heap.Decrease(pointer, -1);
            var elements = ExtractAll(heap).ToArray();
            Assert.That(elements, Is.EqualTo(new[] { -1, 1, 2 }));
        }

        [Test]
        public void Decrease_MiddleElement_ExtractsAllElementsCorrectly()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(2);
            var pointer = heap.Insert(1);
            heap.Insert(0);
            heap.Decrease(pointer, -1);
            var elements = ExtractAll(heap).ToArray();
            Assert.That(elements, Is.EqualTo(new[] { -1, 0, 2 }));
        }

        [Test]
        public void Merge_WithEmpty_CountUnchanged()
        {
            var heap = new PairingHeap<int>();
            heap.Insert(0);
            heap.Merge(new());
            Assert.That(heap.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Merge_WithNonEmpty_CountChanged()
        {
            var heap = new PairingHeap<int>();
            var other = new PairingHeap<int>();
            other.Insert(0);
            heap.Merge(other);
            Assert.That(heap.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Merge_WithNonEmpty_ClearsArgument()
        {
            var heap = new PairingHeap<int>();
            var other = new PairingHeap<int>();
            other.Insert(0);
            heap.Merge(other);
            Assert.That(other.IsEmpty, Is.True);
        }

        private static IEnumerable<int> ExtractAll(PairingHeap<int> heap)
        {
            while (heap.TryExtractMinimum(out var min))
            {
                yield return min;
            }
        }
    }
}