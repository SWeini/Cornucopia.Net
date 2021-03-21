using System;

using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class DynamicArrayFreeListAllocator
    {
        [Test]
        public void Ctor_CountIsZero()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            Assert.That(arr.Count, Is.Zero);
        }

        [Test]
        public void Add_Empty_CountIsOne()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            arr.Add(0);
            Assert.That(arr.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_Empty_CanAccessByIndex()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            var index = arr.Add(42);
            Assert.That(arr[index], Is.EqualTo(42));
        }

        [Test]
        public void Add_AfterRemoval_ReusesIndex()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            var index = arr.Add(0);
            arr.RemoveAt(index);
            var index2 = arr.Add(0);
            Assert.That(index2, Is.EqualTo(index));
        }

        [Test]
        public void RemoveAt_NegativeIndex_Throws()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            Assert.That(() => arr.RemoveAt(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RemoveAt_AboveCount_Throws()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            arr.Add(0);
            Assert.That(() => arr.RemoveAt(1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RemoveAt_ValidIndex_DecreasesCount()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            var index = arr.Add(0);
            arr.RemoveAt(index);
            Assert.That(arr.Count, Is.Zero);
        }

        [Test]
        public void RemoveAt_ExistingIndex_OverwritesValues()
        {
            var arr = new DynamicArrayFreeListAllocator<int>(0);
            var index = arr.Add(42);
            arr.RemoveAt(index);
            Assert.That(arr[index], Is.Zero);
        }
    }
}