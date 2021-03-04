using System.Collections.Generic;
using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class LinkedListTests
    {
        [Test]
        public void Empty_IsNull()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(list, Is.Null);
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(list.IsEmpty(), Is.True);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var list = LinkedList<int>.Empty;
            Assert.That(list.Any(), Is.False);
        }

        [Test]
        public void Create_HeadContainsElement()
        {
            var list = LinkedList.Create(42);
            Assert.That(list.Head, Is.EqualTo(42));
        }

        [Test]
        public void Create_TailIsNull()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.Tail, Is.Null);
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.IsEmpty(), Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var list = LinkedList.Create(0);
            Assert.That(list.Any(), Is.True);
        }

        [Test]
        public void Prepend_Empty_TailIsNull()
        {
            var list = LinkedList<int>.Empty.Prepend(0);
            Assert.That(list.Tail, Is.Null);
        }

        [Test]
        public void Prepend_Empty_HeadContainsElement()
        {
            var list = LinkedList<int>.Empty.Prepend(42);
            Assert.That(list.Head, Is.EqualTo(42));
        }

        [Test]
        public void Prepend_NonEmpty_TailInstanceIsReused()
        {
            var originalList = LinkedList.Create(0);
            var list = originalList.Prepend(0);
            Assert.That(list.Tail, Is.SameAs(originalList));
        }

        [Test]
        public void Prepend_NonEmpty_HeadContainsElement()
        {
            var list = LinkedList.Create(0).Prepend(42);
            Assert.That(list.Head, Is.EqualTo(42));
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
            Assert.That(arguments, Is.EqualTo(new[] {42}));
        }

        [Test]
        public void ForEach_ManyElements_ActionIsCalledWithElementsInOrder()
        {
            var list = LinkedList.Create(42).Prepend(10).Prepend(0);
            var arguments = new List<int>();
            list.ForEach(arguments.Add);
            Assert.That(arguments, Is.EqualTo(new[] {0, 10, 42}));
        }

        [Test]
        public void Reverse_Empty_IsNull()
        {
            var list = LinkedList<int>.Empty.Reverse();
            Assert.That(list, Is.Null);
        }

        [Test]
        public void Reverse_SingleElement_IsSameSequence()
        {
            var list = LinkedList.Create(42).Reverse();
            Assert.That(list.Head, Is.EqualTo(42));
            Assert.That(list.Tail, Is.Null);
        }

        [Test]
        public void Reverse_SingleElement_IsSameInstance()
        {
            var originalInstance = LinkedList.Create(0);
            var reversed = originalInstance.Reverse();
            Assert.That(reversed, Is.SameAs(originalInstance));
        }

        [Test]
        public void Reverse_ManyElements_ReturnsElementsInReverseOrder()
        {
            var list = LinkedList.Create(42).Prepend(10).Prepend(0).Reverse();
            var elements = new List<int>();
            list.ForEach(elements.Add);
            Assert.That(elements, Is.EqualTo(new[] {42, 10, 0}));
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
            var list = LinkedList.Create(42).Prepend(10).Prepend(0);
            var count = list.Count();
            Assert.That(count, Is.EqualTo(3));
        }
    }
}