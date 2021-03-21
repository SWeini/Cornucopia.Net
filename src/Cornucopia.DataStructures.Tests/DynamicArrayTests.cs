using System;
using System.Runtime.CompilerServices;

using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class DynamicArrayTests
    {
        [Test]
        public void Ctor_LengthIsZero()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(arr.Length, Is.Zero);
        }

        [Test]
        public void Ctor_CapacityIsZero()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(arr.Capacity, Is.Zero);
        }

        [Test]
        public void Ctor_WithCapacity_LengthIsZero()
        {
            var arr = new DynamicArray<int>(5);
            Assert.That(arr.Length, Is.Zero);
        }

        [Test]
        public void Ctor_WithCapacity_CapacityIsCorrect()
        {
            var arr = new DynamicArray<int>(42);
            Assert.That(arr.Capacity, Is.EqualTo(42));
        }

        [Test]
        public void SetLength_EmptyToHigher_LengthIsCorrect()
        {
            var arr = new DynamicArray<int>(true);
            arr.Length = 42;
            Assert.That(arr.Length, Is.EqualTo(42));
        }

        [Test]
        public void SetLength_Negative_Throws()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(() => arr.Length = -1, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetLength_NonEmptyGrowInsideCapacity_PreservesContent()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(42);
            arr.Length = 3;
            Assert.That(arr[0], Is.EqualTo(42));
        }

        [Test]
        public void SetLength_NonEmptyGrowInsideCapacity_PreservesCapacity()
        {
            var arr = new DynamicArray<int>(42);
            arr.Length = 13;
            Assert.That(arr.Capacity, Is.EqualTo(42));
        }

        [Test]
        public void SetLength_NonEmptyShrink_PreservesContent()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(42);
            arr.AddLast(13);
            arr.AddLast(5);
            arr.Length = 1;
            Assert.That(arr[0], Is.EqualTo(42));
        }

        [Test]
        public void SetLength_ReferenceTypesShrink_DoesNotHoldOnToReferences()
        {
            var weakReference = Initialize();
            GC.Collect();
            Assert.That(weakReference.TryGetTarget(out _), Is.False);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static WeakReference<object> Initialize()
            {
                var obj = new object();
                var arr = new DynamicArray<object>(true);
                arr.AddLast(obj);
                arr.Length = 0;
                return new(obj);
            }
        }

        [Test]
        public void SetCapacity_EmptyToHigher_CapacityIsCorrect()
        {
            var arr = new DynamicArray<int>(true);
            arr.Capacity = 42;
            Assert.That(arr.Capacity, Is.EqualTo(42));
        }

        [Test]
        public void SetCapacity_NonEmptyShrink_PreservesContent()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(42);
            arr.Capacity = 1;
            Assert.That(arr[0], Is.EqualTo(42));
        }

        [Test]
        public void SetCapacity_NonEmptyGrow_PreservesContent()
        {
            var arr = new DynamicArray<int>(1);
            arr.AddLast(42);
            arr.Capacity = 4;
            Assert.That(arr[0], Is.EqualTo(42));
        }

        [Test]
        public void SetCapacity_NotFull_CopiesOnlyValidContent()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(5);
            arr.AddLast(13);
            var span = arr.AsSpan();
            arr.RemoveLast();
            span[1] = 42;
            arr.Capacity = 2;

            ref var unusedMemory = ref Unsafe.Add(ref arr[0], 1);
            Assert.That(unusedMemory, Is.Zero);
        }

        [Test]
        public void SetCapacity_Negative_Throws()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(() => arr.Capacity = -1, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetCapacity_BelowLength_Throws()
        {
            var arr = new DynamicArray<int>(true);
            arr.AddLast(0);
            Assert.That(() => arr.Capacity = 0, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetCapacity_Zero_CapacityIsZero()
        {
            var arr = new DynamicArray<int>(5);
            arr.Capacity = 0;
            Assert.That(arr.Capacity, Is.Zero);
        }


        [Test]
        public void Item_IndexNegative_Throws()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(() => arr[-1], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Item_IndexIsLength_Throws(int size)
        {
            var arr = new DynamicArray<int>(true);
            for (var i = 0; i < size; i++)
            {
                arr.AddLast(i);
            }

            Assert.That(() => arr[size], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Item_IndexAboveLength_Throws(int size)
        {
            var arr = new DynamicArray<int>(true);
            for (var i = 0; i < size; i++)
            {
                arr.AddLast(i);
            }

            Assert.That(() => arr[size + 1], Throws.TypeOf<ArgumentOutOfRangeException>());
        }


        [Test]
        public void Item_ValidIndex_HasCorrectContent()
        {
            var arr = new DynamicArray<int>(true);
            for (var i = 0; i < 5; i++)
            {
                arr.AddLast(i);
            }

            for (var i = 0; i < 5; i++)
            {
                Assert.That(arr[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void Clear_Empty_LengthIsZero()
        {
            var arr = new DynamicArray<int>(true);
            arr.Clear();
            Assert.That(arr.Length, Is.Zero);
        }

        [Test]
        public void Clear_NonEmpty_CapacityIsNotChanged()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(0);
            arr.Clear();
            Assert.That(arr.Capacity, Is.EqualTo(4));
        }

        [Test]
        public void Clear_ReferenceTypes_DoesNotHoldOnToReferences()
        {
            var weakReference = Initialize();
            GC.Collect();
            Assert.That(weakReference.TryGetTarget(out _), Is.False);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static WeakReference<object> Initialize()
            {
                var obj = new object();
                var arr = new DynamicArray<object>(true);
                arr.AddLast(obj);
                arr.Clear();
                return new(obj);
            }
        }

        [Test]
        public void AddLast_Empty_LengthIsOne()
        {
            var arr = new DynamicArray<int>(true);
            arr.AddLast(0);
            Assert.That(arr.Length, Is.EqualTo(1));
        }

        [Test]
        public void AddLast_AboveCapacity_CapacityIsIncreased()
        {
            var arr = new DynamicArray<int>(4);
            for (var i = 0; i < 5; i++)
            {
                arr.AddLast(0);
            }

            Assert.That(arr.Capacity, Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public void RemoveLast_Empty_Throws()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(() => arr.RemoveLast(), Throws.InvalidOperationException);
        }

        [Test]
        public void RemoveLast_ReferenceTypes_DoesNotHoldOnToReferences()
        {
            var weakReference = Initialize();
            GC.Collect();
            Assert.That(weakReference.TryGetTarget(out _), Is.False);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static WeakReference<object> Initialize()
            {
                var obj = new object();
                var arr = new DynamicArray<object>(true);
                arr.AddLast(obj);
                arr.RemoveLast();
                return new(obj);
            }
        }

        [Test]
        public void TryRemoveList_Empty_ReturnsFalse()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(arr.TryRemoveLast(out _), Is.False);
        }

        [Test]
        public void TryRemoveLast_NonEmpty_ReturnsTrue()
        {
            var arr = new DynamicArray<int>(true);
            arr.AddLast(0);
            Assert.That(arr.TryRemoveLast(out _), Is.True);
        }

        [Test]
        public void TryRemoveLast_NonEmpty_ExtractsValue()
        {
            var arr = new DynamicArray<int>(true);
            arr.AddLast(42);
            arr.TryRemoveLast(out var lastItem);
            Assert.That(lastItem, Is.EqualTo(42));
        }

        [Test]
        public void TryRemoveLast_ReferenceTypes_DoesNotHoldOnToReferences()
        {
            var weakReference = Initialize();
            GC.Collect();
            Assert.That(weakReference.TryGetTarget(out _), Is.False);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static WeakReference<object> Initialize()
            {
                var obj = new object();
                var arr = new DynamicArray<object>(true);
                arr.AddLast(obj);
                arr.TryRemoveLast(out _);
                return new(obj);
            }
        }

        [Test]
        public void AsSpan_Empty_IsEmpty()
        {
            var arr = new DynamicArray<int>(true);
            Assert.That(arr.AsSpan().IsEmpty, Is.True);
        }

        [Test]
        public void AsSpan_NonEmpty_HasCorrectLength()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(0);
            var span = arr.AsSpan();
            Assert.That(span.Length, Is.EqualTo(1));
        }

        [Test]
        public void AsSpan_NonEmpty_HasCorrectContent()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(42);
            var span = arr.AsSpan();
            Assert.That(span[0], Is.EqualTo(42));
        }

        [Test]
        public void AsSpan_NonEmpty_SameAsItem()
        {
            var arr = new DynamicArray<int>(4);
            arr.AddLast(0);
            var span = arr.AsSpan();
            Assert.That(Unsafe.AreSame(ref span[0], ref arr[0]), Is.True);
        }
    }
}