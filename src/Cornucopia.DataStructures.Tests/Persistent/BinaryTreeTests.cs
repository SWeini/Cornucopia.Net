using System.Collections.Generic;
using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class BinaryTreeTests
    {
        [Test]
        public void Empty_IsNull()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(tree, Is.Null);
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(tree.IsEmpty(), Is.True);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(tree.Any(), Is.False);
        }

        [Test]
        public void Create_ValueContainsElement()
        {
            var tree = BinaryTree.Create(42);
            Assert.That(tree.Value, Is.EqualTo(42));
        }

        [Test]
        public void Create_ChildrenAreNull()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.LeftChild, Is.Null);
            Assert.That(tree.RightChild, Is.Null);
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.IsEmpty(), Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.Any(), Is.True);
        }
        
        [Test]
        public void ForEachPreOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = new BinaryTree<int>(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachPreOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] {1, 2, 3}));
        }

        [Test]
        public void ForEachPostOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = new BinaryTree<int>(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachPostOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] { 2, 3, 1 }));
        }

        [Test]
        public void ForEachInOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = new BinaryTree<int>(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachInOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] { 2, 1, 3 }));
        }
    }
}