using System.Collections.Generic;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Persistent
{
    [TestFixture]
    public class BinaryTreeTests
    {
        [Test]
        public void Default_IsEmpty()
        {
            var tree = default(BinaryTree<int>);
            Assert.That(tree.IsEmpty, Is.True);
        }

        [Test]
        public void Create_RootValueContainsElement()
        {
            var tree = BinaryTree.Create(42);
            Assert.That(tree.RootValue, Is.EqualTo(42));
        }

        [Test]
        public void Create_SubTreesAreEmpty()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.LeftSubTree.IsEmpty, Is.True);
            Assert.That(tree.RightSubTree.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_Empty_IsTrue()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(tree.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_NonEmpty_IsFalse()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.IsEmpty, Is.False);
        }

        [Test]
        public void Any_Empty_IsFalse()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(tree.Any, Is.False);
        }

        [Test]
        public void Any_NonEmpty_IsTrue()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.Any, Is.True);
        }

        [Test]
        public void LeftSubTree_Empty_Throws()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(() => tree.LeftSubTree, Throws.InvalidOperationException);
        }

        [Test]
        public void LeftSubTree_EmptySubTree_IsEmpty()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.LeftSubTree.IsEmpty, Is.True);
        }

        [Test]
        public void LeftSubTree_NonEmpty_IsEqualToConstructionArgument()
        {
            var subtree = BinaryTree.Create(0);
            var tree = BinaryTree.Create(subtree, BinaryTree<int>.Empty, 0);
            Assert.That(tree.LeftSubTree, Is.EqualTo(subtree));
        }

        [Test]
        public void RightSubTree_Empty_Throws()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(() => tree.RightSubTree, Throws.InvalidOperationException);
        }

        [Test]
        public void RightSubTree_EmptySubTree_IsEmpty()
        {
            var tree = BinaryTree.Create(0);
            Assert.That(tree.RightSubTree.IsEmpty, Is.True);
        }

        [Test]
        public void RightSubTree_NonEmpty_IsEqualToConstructionArgument()
        {
            var subtree = BinaryTree.Create(0);
            var tree = BinaryTree.Create(BinaryTree<int>.Empty, subtree, 0);
            Assert.That(tree.RightSubTree, Is.EqualTo(subtree));
        }

        [Test]
        public void RootValue_Empty_Throws()
        {
            var tree = BinaryTree<int>.Empty;
            Assert.That(() => tree.RootValue, Throws.InvalidOperationException);
        }

        [Test]
        public void RootValue_NonEmpty_IsRootElement()
        {
            var tree = BinaryTree.Create(42);
            Assert.That(tree.RootValue, Is.EqualTo(42));
        }

        [Test]
        public void ForEachPreOrder_Empty_DoesNotCallAction()
        {
            var tree = BinaryTree<int>.Empty;
            tree.ForEachPreOrder(_ => Assert.Fail());
        }

        [Test]
        public void ForEachPreOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = BinaryTree.Create(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachPreOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void ForEachInOrder_Empty_DoesNotCallAction()
        {
            var tree = BinaryTree<int>.Empty;
            tree.ForEachInOrder(_ => Assert.Fail());
        }

        [Test]
        public void ForEachInOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = BinaryTree.Create(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachInOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] { 2, 1, 3 }));
        }

        [Test]
        public void ForEachPostOrder_Empty_DoesNotCallAction()
        {
            var tree = BinaryTree<int>.Empty;
            tree.ForEachPostOrder(_ => Assert.Fail());
        }

        [Test]
        public void ForEachPostOrder_SmallTree_ActionIsCalledWithElementsInOrder()
        {
            var tree = BinaryTree.Create(BinaryTree.Create(2), BinaryTree.Create(3), 1);
            var list = new List<int>();
            tree.ForEachPostOrder(list.Add);
            Assert.That(list, Is.EqualTo(new[] { 2, 3, 1 }));
        }
    }
}