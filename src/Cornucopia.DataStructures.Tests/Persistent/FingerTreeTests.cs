using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class FingerTreeTests
    {
        [Test]
        public void Empty_IsNotNull()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(tree, Is.Not.EqualTo(default(FingerTree<int>)));
        }

        [Test]
        public void Default_ThrowsNullReferenceException()
        {
            FingerTree<int> tree = default;
            Assert.That(() => tree.Count, Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(tree.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var tree = FingerTree.Create(0);
            Assert.That(tree.IsEmpty, Is.False);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(tree.Any, Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var tree = FingerTree.Create(0);
            Assert.That(tree.Any, Is.True);
        }

        [Test]
        public void Count_Empty_IsZero()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(tree.Count, Is.Zero);
        }

        [Test]
        public void Count_SingleElement_IsOne()
        {
            var tree = FingerTree.Create(0);
            Assert.That(tree.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_ManyElements_IsNumberOfElements()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5);
            Assert.That(tree.Count, Is.EqualTo(5));
        }

        [Test]
        public void First_Empty_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.First, Throws.InvalidOperationException);
        }

        [Test]
        public void First_SingleElement_IsElement()
        {
            var tree = FingerTree.Create(42);
            Assert.That(tree.First, Is.EqualTo(42));
        }

        [Test]
        public void First_ManyElements_IsFirstElement()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5);
            Assert.That(tree.First, Is.EqualTo(1));
        }

        [Test]
        public void AddFirst_Empty_CountIsOne()
        {
            var tree = FingerTree<int>.Empty.AddFirst(0);
            Assert.That(tree.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddFirst_Empty_FirstReturnsElement()
        {
            var tree = FingerTree<int>.Empty.AddFirst(42);
            Assert.That(tree.First, Is.EqualTo(42));
        }

        [Test]
        public void AddFirst_ManyElements_CountIsIncreased()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5).AddFirst(0);
            Assert.That(tree.Count, Is.EqualTo(6));
        }

        [Test]
        public void AddFirst_ManyElements_OrderIsCorrect()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5).AddFirst(42);
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 42, 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void RemoveFirst_Empty_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.RemoveFirst(), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveFirst_EmptyWithOut_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.RemoveFirst(out _), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveFirst_SingleElement_IsEmpty()
        {
            var tree = FingerTree.Create(0).RemoveFirst();
            Assert.That(tree.IsEmpty, Is.True);
        }

        [Test]
        public void RemoveFirst_SingleElement_ExtractsElement()
        {
            var tree = FingerTree.Create(42);
            _ = tree.RemoveFirst(out var element);
            Assert.That(element, Is.EqualTo(42));
        }

        [Test]
        public void RemoveFirst_ManyElements_ExtractsElementsInOrder()
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, 15));
            var list = new List<int>();
            while (tree.Any)
            {
                tree = tree.RemoveFirst(out var value);
                list.Add(value);
            }

            Assert.That(list, Is.EqualTo(Enumerable.Range(0, 15)));
        }

        [Test]
        public void Last_Empty_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.Last, Throws.InvalidOperationException);
        }

        [Test]
        public void Last_SingleElement_IsElement()
        {
            var tree = FingerTree.Create(42);
            Assert.That(tree.Last, Is.EqualTo(42));
        }

        [Test]
        public void Last_ManyElements_IsLastElement()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5);
            Assert.That(tree.Last, Is.EqualTo(5));
        }

        [Test]
        public void AddLast_Empty_CountIsOne()
        {
            var tree = FingerTree<int>.Empty.AddLast(0);
            Assert.That(tree.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddLast_Empty_LastReturnsElement()
        {
            var tree = FingerTree<int>.Empty.AddLast(42);
            Assert.That(tree.Last, Is.EqualTo(42));
        }

        [Test]
        public void AddLast_ManyElements_CountIsIncreased()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5).AddLast(0);
            Assert.That(tree.Count, Is.EqualTo(6));
        }

        [Test]
        public void AddLast_ManyElements_OrderIsCorrect()
        {
            var tree = FingerTree.Create(1, 2, 3, 4, 5).AddLast(42);
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 1, 2, 3, 4, 5, 42 }));
        }

        [Test]
        public void RemoveLast_Empty_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.RemoveLast(), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveLast_EmptyWithOut_Throws()
        {
            var tree = FingerTree<int>.Empty;
            Assert.That(() => tree.RemoveLast(out _), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveLast_SingleElement_IsEmpty()
        {
            var tree = FingerTree.Create(0).RemoveLast();
            Assert.That(tree.IsEmpty, Is.True);
        }

        [Test]
        public void RemoveLast_SingleElement_ExtractsElement()
        {
            var tree = FingerTree.Create(42);
            _ = tree.RemoveLast(out var element);
            Assert.That(element, Is.EqualTo(42));
        }

        [Test]
        public void RemoveLast_ManyElements_ExtractsElementsInOrder()
        {
            var tree = FingerTree<int>.Empty;
            for (var i = 0; i < 15; i++)
            {
                tree = tree.AddFirst(i);
            }
            var list = new List<int>();
            while (tree.Any)
            {
                tree = tree.RemoveLast(out var value);
                list.Add(value);
            }

            Assert.That(list, Is.EqualTo(Enumerable.Range(0, 15)));
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void Item_InvalidIndex_Throws(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            Assert.That(() => tree[index], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Item_SingleElement_ReturnsElement()
        {
            var tree = FingerTree.Create(42);
            Assert.That(tree[0], Is.EqualTo(42));
        }

        [Test]
        public void Item_ManyElements_ReturnsCorrectElement()
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, 42));
            for (var i = 0; i < 42; i++)
            {
                Assert.That(tree[i], Is.EqualTo(i));
            }
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void SetItem_InvalidIndex_Throws(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            Assert.That(() => tree.SetItem(index, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Item_SingleElement_SetsElement()
        {
            var tree = FingerTree.Create(0).SetItem(0, 42);
            Assert.That(tree[0], Is.EqualTo(42));
        }

        [Test]
        public void SetItem_ManyElements_SetsElement()
        {
            var tree = FingerTree.CreateRange(Enumerable.Repeat(0, 42));
            for (var i = 0; i < 42; i++)
            {
                Assert.That(tree.SetItem(i, i)[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void Append_Empty_ReturnsOther()
        {
            var tree = FingerTree<int>.Empty.Append(FingerTree.Create(42));
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void Append_WithEmpty_ReturnsThis()
        {
            var tree = FingerTree.Create(42).Append(FingerTree<int>.Empty);
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void Append_ManyElements_ReturnsTreeWithCorrectElements()
        {
            var tree1 = FingerTree.CreateRange(Enumerable.Range(0, 15));
            var tree2 = FingerTree.CreateRange(Enumerable.Range(15, 27));
            Assert.That(ToArray(tree1.Append(tree2)), Is.EqualTo(Enumerable.Range(0, 42)));
        }

        [TestCase(2, 2)]
        [TestCase(4, 4)]
        public void Append_ManyElements_VariousTreeSizes(int size1, int size2)
        {
            var tree1 = FingerTree<int>.Empty;
            var tree2 = FingerTree<int>.Empty;
            for (var i = 0; i < size1; i++)
            {
                tree1 = tree1.AddLast(0);
            }

            for (var i = 0; i < size2; i++)
            {
                tree2 = tree2.AddFirst(0);
            }

            var combined = tree1.Append(tree2);
            Assert.That(combined.Count, Is.EqualTo(size1 + size2));
        }

        [Test]
        public void Prepend_Empty_ReturnsOther()
        {
            var tree = FingerTree<int>.Empty.Prepend(FingerTree.Create(42));
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void Prepend_WithEmpty_ReturnsThis()
        {
            var tree = FingerTree.Create(42).Prepend(FingerTree<int>.Empty);
            Assert.That(ToArray(tree), Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void Prepend_ManyElements_ReturnsTreeWithCorrectElements()
        {
            var tree1 = FingerTree.CreateRange(Enumerable.Range(0, 15));
            var tree2 = FingerTree.CreateRange(Enumerable.Range(15, 27));
            Assert.That(ToArray(tree2.Prepend(tree1)), Is.EqualTo(Enumerable.Range(0, 42)));
        }

        [TestCase(-1)]
        [TestCase(4)]
        public void Insert_InvalidIndex_Throws(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            Assert.That(() => tree.Insert(index, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Insert_AtIndexZero_FirstIsElement()
        {
            var tree = FingerTree.Create(1, 2, 3).Insert(0, 42);
            Assert.That(tree.First, Is.EqualTo(42));
        }

        [Test]
        public void Insert_AtIndexEnd_LastIsElement()
        {
            var tree = FingerTree.Create(1, 2, 3).Insert(3, 42);
            Assert.That(tree.Last, Is.EqualTo(42));
        }

        [TestCase(5)]
        [TestCase(6)]
        [TestCase(15)]
        public void Insert_InMiddle_ElementIsSet(int size)
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, size));
            for (var i = 1; i < size; i++)
            {
                var treeWithInsertion = tree.Insert(i, 42);
                Assert.That(treeWithInsertion[i], Is.EqualTo(42));
            }
        }

        [TestCase(-1)]
        [TestCase(4)]
        public void InsertRange_InvalidIndex_Throws(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            Assert.That(() => tree.InsertRange(index, FingerTree<int>.Empty), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void InsertRange_Empty_ReturnThis(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            var treeWithInsertion = tree.InsertRange(index, FingerTree<int>.Empty);
            Assert.That(treeWithInsertion, Is.EqualTo(tree));
        }

        [Test]
        public void InsertRange_AtIndexZero_ElementsInCorrectOrder()
        {
            var tree = FingerTree.Create(4, 5, 6).InsertRange(0, FingerTree.Create(0, 1, 2, 3));
            Assert.That(ToArray(tree), Is.EqualTo(Enumerable.Range(0, 7)));
        }

        [Test]
        public void InsertRange_AtIndexEnd_ElementsInCorrectOrder()
        {
            var tree = FingerTree.Create(0, 1, 2).InsertRange(3, FingerTree.Create(3, 4, 5, 6));
            Assert.That(ToArray(tree), Is.EqualTo(Enumerable.Range(0, 7)));
        }

        [Test]
        public void InsertRange_InMiddle_ElementsInCorrectOrder()
        {
            var tree = FingerTree.Create(0, 1, 5, 6).InsertRange(2, FingerTree.Create(2, 3, 4));
            Assert.That(ToArray(tree), Is.EqualTo(Enumerable.Range(0, 7)));
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void RemoveAt_InvalidIndex_Throws(int index)
        {
            var tree = FingerTree.Create(1, 2, 3);
            Assert.That(() => tree.RemoveAt(index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RemoveAt_ManyElements_ElementRemoved()
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, 15));
            for (var i = 0; i < 15; i++)
            {
                var treeWithoutElement = tree.RemoveAt(i);
                Assert.That(ToArray(treeWithoutElement), Does.Not.Contain(i));
            }
        }

        [Test]
        public void GetRange_FullIndexRange_ReturnThis()
        {
            var tree = FingerTree.Create(1, 2, 3);
            var rangeTree = tree.GetRange(0, 3);
            Assert.That(rangeTree, Is.EqualTo(tree));
        }

        [Test]
        public void GetRange_RangeInMiddle_ContainsCorrectElements()
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, 15));
            var rangeTree = tree.GetRange(2, 10);
            Assert.That(ToArray(rangeTree), Is.EqualTo(Enumerable.Range(2, 10)));
        }

        [Test]
        public void ForEach_Empty_DoesNotCallAction()
        {
            var tree = FingerTree<int>.Empty;
            tree.ForEach(_ => Assert.Fail());
        }

        [Test]
        public void ForEach_ManyElements_CallsActionInCorrectOrder()
        {
            var tree = FingerTree.CreateRange(Enumerable.Range(0, 15));
            var list = new List<int>();
            tree.ForEach(list.Add);
            Assert.That(list, Is.EqualTo(Enumerable.Range(0, 15)));
        }

        private static T[] ToArray<T>(FingerTree<T> tree)
        {
            var list = new List<T>();
            tree.ForEach(list.Add);
            return list.ToArray();
        }
    }
}