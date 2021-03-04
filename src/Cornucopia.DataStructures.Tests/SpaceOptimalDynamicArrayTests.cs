using System;
using System.Linq;
using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class SpaceOptimalDynamicArrayTests
    {
        [Test]
        public void Ctor_CountZero()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            Assert.That(arr.Count, Is.Zero);
        }

        [Test]
        public void Ctor_EnumerableIsEmpty()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            Assert.That(arr.ToList(), Is.Empty);
        }

        [Test]
        public void Item_IndexNegative_Throws()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            Assert.That(() => arr[-1], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Item_IndexIsCount_Throws(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(i);
            }

            Assert.That(() => arr[size], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Item_IndexAboveCount_Throws(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(i);
            }

            Assert.That(() => arr[size + 1], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Add_Empty_CountIsOne()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            arr.Add(0);
            Assert.That(arr.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_Empty_ValueIsAtZeroIndex()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            arr.Add(42);
            Assert.That(arr[0], Is.EqualTo(42));
        }

        [Test]
        public void Add_Empty_EnumerableHasValue()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            arr.Add(42);
            Assert.That(arr.ToList(), Is.EqualTo(new[] {42}));
        }

        [TestCase(1000)]
        public void Add_MultipleTimes_CountIsCorrect(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(0);
            }

            Assert.That(arr.Count, Is.EqualTo(size));
        }

        [TestCase(1000)]
        public void Add_MultipleTimes_ItemsAreCorrect(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(i);
            }

            for (var i = 0; i < size; i++)
            {
                Assert.That(arr[i], Is.EqualTo(i));
            }
        }

        [TestCase(1000)]
        public void Add_MultipleTimes_EnumerableIsCorrect(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(i);
            }

            Assert.That(arr.ToList(), Is.EqualTo(Enumerable.Range(0, size)));
        }

        [Test]
        public void Shrink_Empty_Throws()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            Assert.That(() => arr.Shrink(), Throws.InvalidOperationException);
        }

        [TestCase(1000)]
        public void Shrink_All_CountIsZero(int size)
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < size; i++)
            {
                arr.Add(i);
            }

            for (var i = 0; i < size; i++)
            {
                arr.Shrink();
            }

            Assert.That(arr.Count, Is.Zero);
        }

        [Test]
        public void GrowToThousandAndShrinkAgain()
        {
            var arr = new SpaceOptimalDynamicArray<int>();
            for (var i = 0; i < 1000; i++)
            {
                arr.Grow();
                arr[i] = i;
            }

            for (var i = 0; i < 1000; i++)
            {
                Assert.That(arr[i], Is.EqualTo(i));
            }

            Assert.That(arr.ToList(), Is.EqualTo(Enumerable.Range(0, 1000)));

            for (var i = 0; i < 1000; i++)
            {
                arr.Shrink();
            }

            Assert.That(arr.Count, Is.Zero);
        }
    }
}