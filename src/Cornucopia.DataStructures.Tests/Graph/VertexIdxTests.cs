using System.Collections.Generic;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class VertexIdxTests
    {
        [Test]
        public void Index_IsAsConstructed()
        {
            var vertex = new VertexIdx(42);
            Assert.That(vertex.Index, Is.EqualTo(42));
        }

        [Test]
        public void Equals_SameVertex_IsTrue()
        {
            var vertex = new VertexIdx(0);
            Assert.That(vertex.Equals(new VertexIdx(0)), Is.True);
        }

        [Test]
        public void Equals_OtherVertex_IsFalse()
        {
            var vertex = new VertexIdx(0);
            Assert.That(vertex.Equals(new VertexIdx(1)), Is.False);
        }

        [Test]
        public void Equals_SameVertexObject_IsTrue()
        {
            var vertex = new VertexIdx(0);
            Assert.That(vertex.Equals((object) new VertexIdx(0)), Is.True);
        }

        [Test]
        public void Equals_Null_IsFalse()
        {
            var vertex = new VertexIdx(0);
            Assert.That(vertex.Equals(null), Is.False);
        }

        [Test]
        public void Equals_SomeObject_IsFalse()
        {
            var vertex = new VertexIdx(0);
            Assert.That(vertex.Equals(new object()), Is.False);
        }

        [Test]
        public void Equality_SameVertex_IsTrue()
        {
            var vertex = new VertexIdx(0);
            var other = new VertexIdx(0);
            Assert.That(vertex == other, Is.True);
        }

        [Test]
        public void Inequality_OtherVertex_IsTrue()
        {
            var vertex = new VertexIdx(0);
            var other = new VertexIdx(1);
            Assert.That(vertex != other, Is.True);
        }

        [Test]
        public void ToString_FormattedAsDefined()
        {
            var vertex = new VertexIdx(42);
            Assert.That(vertex.ToString(), Is.EqualTo("#42"));
        }

        [Test]
        public void UseInDictionary_WorksAsExpected()
        {
            var dictionary = new Dictionary<VertexIdx, int>();
            dictionary[new VertexIdx(0)] = 5;
            Assert.That(dictionary.TryGetValue(new VertexIdx(0), out var value), Is.True);
            Assert.That(value, Is.EqualTo(5));
        }
    }
}