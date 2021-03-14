using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class BinaryHeapTests
    {
        [Test]
        public void Ctor_DefaultComparer_CountIsZero()
        {
            var heap = new BinaryHeap<int>(Comparer<int>.Default);
            Assert.That(heap.Count, Is.Zero);
        }

        [Test]
        public void Ctor_Capacity_CountIsZero()
        {
            var heap = new BinaryHeap<int>(42, Comparer<int>.Default);
            Assert.That(heap.Count, Is.Zero);
        }

        [Test]
        public void Ctor_NegativeCapacity_Throws()
        {
            Assert.That(() => new BinaryHeap<int>(-1, Comparer<int>.Default), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Count_Empty_IsZero()
        {
            var heap = new BinaryHeap<int>();
            Assert.That(heap.Count, Is.Zero);
        }

        [Test]
        public void Insert_Empty_CountIsOne()
        {
            var heap = new BinaryHeap<int>();
            heap.Insert(0);
            Assert.That(heap.Count, Is.EqualTo(1));
        }

        [TestCase(5)]
        [TestCase(42)]
        public void Insert_MultipleTimesIntoEmpty_CountIsCorrect(int size)
        {
            var heap = new BinaryHeap<int>();
            for (var i = 0; i < size; i++)
            {
                heap.Insert(0);
            }

            Assert.That(heap.Count, Is.EqualTo(size));
        }

        [Test]
        public void Minimum_Empty_Throws()
        {
            var heap = new BinaryHeap<int>();
            Assert.That(() => heap.Minimum, Throws.InvalidOperationException);
        }

        [Test]
        public void Minimum_SingleElement_ReturnsElement()
        {
            var heap = new BinaryHeap<int>();
            heap.Insert(42);
            Assert.That(heap.Minimum, Is.EqualTo(42));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0)]
        public void Minimum_ManyElements_IsCorrect(params int[] values)
        {
            var heap = new BinaryHeap<int>();
            foreach (var value in values)
            {
                heap.Insert(value);
            }

            Assert.That(heap.Minimum, Is.EqualTo(values.Min()));
        }

        [Test]
        public void ExtractMinimum_Empty_Throws()
        {
            var heap = new BinaryHeap<int>();
            Assert.That(() => heap.ExtractMinimum(), Throws.InvalidOperationException);
        }

        [Test]
        public void ExtractMinimum_SingleElement_ReturnsElement()
        {
            var heap = new BinaryHeap<int>();
            heap.Insert(42);
            Assert.That(heap.ExtractMinimum(), Is.EqualTo(42));
        }

        [Test]
        public void ExtractMinimum_SingleElement_RemovesElement()
        {
            var heap = new BinaryHeap<int>();
            heap.Insert(0);
            heap.ExtractMinimum();
            Assert.That(heap.Count, Is.Zero);
            Assert.That(() => heap.Minimum, Throws.InvalidOperationException);
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0)]
        [TestCase(5, 3, 4, 11, 8, 15)]
        public void ExtractMinimum_ManyElements_CanSortSequence(params int[] values)
        {
            var heap = new BinaryHeap<int>();
            foreach (var value in values)
            {
                heap.Insert(value);
            }

            var sorted = new List<int>();
            while (heap.Count > 0)
            {
                sorted.Add(heap.ExtractMinimum());
            }

            Assert.That(sorted, Is.EqualTo(values.OrderBy(x => x).ToList()));
        }
    }
}