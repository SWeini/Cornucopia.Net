using System.Collections.Generic;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class EdgeIdxTests
    {
        [Test]
        public void Index_IsAsConstructed()
        {
            var edge = new EdgeIdx(42);
            Assert.That(edge.Index, Is.EqualTo(42));
        }

        [Test]
        public void Equals_SameEdge_IsTrue()
        {
            var edge = new EdgeIdx(0);
            Assert.That(edge.Equals(new EdgeIdx(0)), Is.True);
        }

        [Test]
        public void Equals_OtherEdge_IsFalse()
        {
            var edge = new EdgeIdx(0);
            Assert.That(edge.Equals(new EdgeIdx(1)), Is.False);
        }

        [Test]
        public void Equals_SameEdgeObject_IsTrue()
        {
            var edge = new EdgeIdx(0);
            Assert.That(edge.Equals((object) new EdgeIdx(0)), Is.True);
        }

        [Test]
        public void Equals_Null_IsFalse()
        {
            var edge = new EdgeIdx(0);
            Assert.That(edge.Equals(null), Is.False);
        }

        [Test]
        public void Equals_SomeObject_IsFalse()
        {
            var edge = new EdgeIdx(0);
            Assert.That(edge.Equals(new object()), Is.False);
        }

        [Test]
        public void Equality_SameEdge_IsTrue()
        {
            var edge = new EdgeIdx(0);
            var other = new EdgeIdx(0);
            Assert.That(edge == other, Is.True);
        }

        [Test]
        public void Inequality_OtherEdge_IsTrue()
        {
            var edge = new EdgeIdx(0);
            var other = new EdgeIdx(1);
            Assert.That(edge != other, Is.True);
        }

        [Test]
        public void ToString_FormattedAsDefined()
        {
            var edge = new EdgeIdx(42);
            Assert.That(edge.ToString(), Is.EqualTo("#42"));
        }

        [Test]
        public void UseInDictionary_WorksAsExpected()
        {
            var dictionary = new Dictionary<EdgeIdx, int>();
            dictionary[new EdgeIdx(0)] = 5;
            Assert.That(dictionary.TryGetValue(new EdgeIdx(0), out var value), Is.True);
            Assert.That(value, Is.EqualTo(5));
        }
    }
}