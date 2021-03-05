using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class RandomAccessListTests
    {
        [Test]
        public void Empty_IsDefault()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(list, Is.EqualTo(default(RandomAccessList<int>)));
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(list.IsEmpty, Is.True);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(list.Any, Is.False);
        }

        [Test]
        public void Count_Empty_IsZero()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(list.Count(), Is.Zero);
        }

        [Test]
        public void Create_CountIsOne()
        {
            var list = RandomAccessList.Create(0);
            Assert.That(list.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Create_ManyElements_CountIsCorrect()
        {
            var list = RandomAccessList.Create(1, 2, 3);
            Assert.That(list.Count(), Is.EqualTo(3));
        }

        [TestCase(5)]
        public void Create_ManyElements_IndexedAccessIsCorrect(int size)
        {
            var list = RandomAccessList.Create(Enumerable.Range(0, size).ToArray());
            for (var i = 0; i < size; i++)
            {
                Assert.That(list[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var list = RandomAccessList.Create(0);
            Assert.That(list.IsEmpty, Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var list = RandomAccessList.Create(0);
            Assert.That(list.Any, Is.True);
        }

        [Test]
        public void First_Empty_Throws()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(() => list.First, Throws.InvalidOperationException);
        }

        [Test]
        public void First_SingleElement_ReturnsElement()
        {
            var list = RandomAccessList.Create(42);
            Assert.That(list.First, Is.EqualTo(42));
        }

        [Test]
        public void First_ManyElements_ReturnsFirstElement()
        {
            var list = RandomAccessList.Create(1, 2, 3);
            Assert.That(list.First, Is.EqualTo(1));
        }

        [Test]
        public void RemoveFirst_Empty_Throws()
        {
            var list = RandomAccessList<int>.Empty;
            Assert.That(() => list.RemoveFirst(out _), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveFirst_SingleElement_ReturnsEmpty()
        {
            var list = RandomAccessList.Create(0);
            Assert.That(list.RemoveFirst(out _), Is.EqualTo(RandomAccessList<int>.Empty));
        }

        [Test]
        public void RemoveFirst_SingleElement_ExtractsElement()
        {
            var list = RandomAccessList.Create(42);
            list.RemoveFirst(out var first);
            Assert.That(first, Is.EqualTo(42));
        }

        [Test]
        public void RemoveFirst_MultipleElements_CountIsDecremented()
        {
            var list = RandomAccessList.Create(0, 1, 2);
            Assert.That(list.RemoveFirst(out _).Count(), Is.EqualTo(2));
        }

        [Test]
        public void RemoveFirst_MultipleElements_ExtractsFirstElement()
        {
            var list = RandomAccessList.Create(1, 2, 3);
            list.RemoveFirst(out var first);
            Assert.That(first, Is.EqualTo(1));
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void Item_InvalidIndex_Throws(int index)
        {
            var list = RandomAccessList.Create(1, 2, 3);
            Assert.That(() => list[index], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ForEach_Empty_DoesNotCallAction()
        {
            var list = RandomAccessList<int>.Empty;
            list.ForEach(_ => Assert.Fail());
        }

        [Test]
        public void ForEach_MultipleElements_ActionIsCalledWithElementsInOrder()
        {
            var list = RandomAccessList.Create(0, 1, 2, 3, 4);
            var arguments = new List<int>();
            list.ForEach(arguments.Add);
            Assert.That(arguments, Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
        }

        [Test]
        public void GetEnumerator_Empty_MoveNextIsFalse()
        {
            var list = RandomAccessList<int>.Empty;
            var arguments = new List<int>();
            foreach (var value in list)
            {
                arguments.Add(value);
            }

            Assert.That(arguments, Is.Empty);
        }

        [Test]
        public void GetEnumerator_MultipleElements_YieldsElementsInOrder()
        {
            var list = RandomAccessList.Create(0, 1, 2, 3, 4);
            var arguments = new List<int>();
            foreach (var value in list)
            {
                arguments.Add(value);
            }

            Assert.That(arguments, Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
        }

    }
}