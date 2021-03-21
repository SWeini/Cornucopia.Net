using System.Collections.Generic;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class LinkedListTests
    {
        [Test]
        public void Default_IsEmpty()
        {
            var list = default(LinkedList<int>);
            Assert.That(list.IsEmpty, Is.True);
        }

        [Test]
        public void Create_FirstIsElement()
        {
            var list = LinkedList.Create(42);
            Assert.That(list.First, Is.EqualTo(42));
        }

        [Test]
        public void Create_RemoveFirstIsEmpty()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.RemoveFirst().IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(list.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.IsEmpty, Is.False);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(list.Any, Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.Any, Is.True);
        }

        [Test]
        public void First_Empty_Throws()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(() => list.First, Throws.InvalidOperationException);
        }

        [Test]
        public void First_NonEmpty_IsFirstElement()
        {
            var list = LinkedList.Create(0).AddFirst(42);
            Assert.That(list.First, Is.EqualTo(42));
        }

        [Test]
        public void AddFirst_Empty_RemoveFirstIsEmpty()
        {
            var list = LinkedList<int>.Empty.AddFirst(0);
            Assert.That(list.RemoveFirst().IsEmpty, Is.True);
        }

        [Test]
        public void AddFirst_Empty_FirstIsElement()
        {
            var list = LinkedList<int>.Empty.AddFirst(42);
            Assert.That(list.First, Is.EqualTo(42));
        }

        [Test]
        public void AddFirst_NonEmpty_TailInstanceIsReused()
        {
            var originalList = LinkedList.Create(0);
            var list = originalList.AddFirst(0);
            Assert.That(list.RemoveFirst(), Is.EqualTo(originalList));
        }

        [Test]
        public void AddFirst_NonEmpty_FirstIsElement()
        {
            var list = LinkedList.Create(0).AddFirst(42);
            Assert.That(list.First, Is.EqualTo(42));
        }

        [Test]
        public void RemoveFirst_Empty_Throws()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(() => list.RemoveFirst(), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveFirst_SingleElement_IsEmpty()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.RemoveFirst().IsEmpty, Is.True);
        }

        [Test]
        public void ForEach_Empty_DoesNotCallAction()
        {
            var list = LinkedList<int>.Empty;
            list.ForEach(_ => Assert.Fail());
        }

        [Test]
        public void ForEach_OneElement_ActionIsCalledWithElement()
        {
            var list = LinkedList.Create(42);
            var arguments = new List<int>();
            list.ForEach(arguments.Add);
            Assert.That(arguments, Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void ForEach_ManyElements_ActionIsCalledWithElementsInOrder()
        {
            var list = LinkedList.Create(42).AddFirst(10).AddFirst(0);
            var arguments = new List<int>();
            list.ForEach(arguments.Add);
            Assert.That(arguments, Is.EqualTo(new[] { 0, 10, 42 }));
        }

        [Test]
        public void Reverse_Empty_IsEmpty()
        {
            var list = LinkedList<int>.Empty.Reverse();
            Assert.That(list.IsEmpty, Is.True);
        }

        [Test]
        public void Reverse_SingleElement_IsSameSequence()
        {
            var list = LinkedList.Create(42).Reverse();
            Assert.That(list.First, Is.EqualTo(42));
            Assert.That(list.RemoveFirst().IsEmpty, Is.True);
        }

        [Test]
        public void Reverse_SingleElement_IsSameInstance()
        {
            var originalInstance = LinkedList.Create(0);
            var reversed = originalInstance.Reverse();
            Assert.That(reversed, Is.EqualTo(originalInstance));
        }

        [Test]
        public void Reverse_ManyElements_ReturnsElementsInReverseOrder()
        {
            var list = LinkedList.Create(42).AddFirst(10).AddFirst(0).Reverse();
            var elements = new List<int>();
            list.ForEach(elements.Add);
            Assert.That(elements, Is.EqualTo(new[] { 42, 10, 0 }));
        }

        [Test]
        public void Count_Empty_IsZero()
        {
            var list = LinkedList<int>.Empty;
            var count = list.Count();
            Assert.That(count, Is.Zero);
        }

        [Test]
        public void Count_SingleElement_IsOne()
        {
            var list = LinkedList.Create(0);
            var count = list.Count();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void Count_ManyElements_IsCorrect()
        {
            var list = LinkedList.Create(42).AddFirst(10).AddFirst(0);
            var count = list.Count();
            Assert.That(count, Is.EqualTo(3));
        }
    }
}