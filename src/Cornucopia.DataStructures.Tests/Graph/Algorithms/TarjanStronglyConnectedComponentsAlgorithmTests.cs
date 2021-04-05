using System;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    [TestFixture]
    public class TarjanStronglyConnectedComponentsAlgorithmTests
    {
        [Test]
        public void ComputeStronglyConnectedComponents_SimpleGraph_IsCorrect()
        {
            var g = CreateGraph(5, false, (0, 1), (1, 2), (2, 3), (3, 4), (0, 4));
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DirectedGraph<Empty, Empty>, VertexIdx, EdgeIdx>(g);
            var components = a.ComputeStronglyConnectedComponents(new VertexIdx(0));

            Assert.That(components.Length, Is.EqualTo(5));
            Assert.That(components[0].ToArray(), Is.EqualTo(new[] { new VertexIdx(4) }));
            Assert.That(components[1].ToArray(), Is.EqualTo(new[] { new VertexIdx(3) }));
            Assert.That(components[2].ToArray(), Is.EqualTo(new[] { new VertexIdx(2) }));
            Assert.That(components[3].ToArray(), Is.EqualTo(new[] { new VertexIdx(1) }));
            Assert.That(components[4].ToArray(), Is.EqualTo(new[] { new VertexIdx(0) }));
        }

        [Test]
        public void ComputeStronglyConnectedComponents_SimpleGraph_CanRunPartially()
        {
            var g = CreateGraph(5, false, (0, 1), (1, 2), (2, 3), (3, 4), (0, 4));
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DirectedGraph<Empty, Empty>, VertexIdx, EdgeIdx>(g);
            var components = a.ComputeStronglyConnectedComponents(new VertexIdx(2));

            Assert.That(components.Length, Is.EqualTo(3));
            Assert.That(components[0].ToArray(), Is.EqualTo(Vertices(4)));
            Assert.That(components[1].ToArray(), Is.EqualTo(Vertices(3)));
            Assert.That(components[2].ToArray(), Is.EqualTo(Vertices(2)));

            components = a.ComputeStronglyConnectedComponents(new VertexIdx(0));

            Assert.That(components.Length, Is.EqualTo(2));
            Assert.That(components[0].ToArray(), Is.EqualTo(Vertices(1)));
            Assert.That(components[1].ToArray(), Is.EqualTo(Vertices(0)));
        }

        [Test]
        public void ComputeStronglyConnectedComponents_SimpleGraph_SecondRunReturnsNoComponents()
        {
            var g = CreateGraph(5, false, (0, 1), (1, 2), (2, 3), (3, 4), (0, 4));
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DirectedGraph<Empty, Empty>, VertexIdx, EdgeIdx>(g);
            a.ComputeStronglyConnectedComponents(new VertexIdx(0));
            var components = a.ComputeStronglyConnectedComponents(new VertexIdx(0));
            Assert.That(components.Length, Is.Zero);
        }

        [Test]
        public void ComputeStronglyConnectedComponents_WikipediaExample_IsCorrect()
        {
            var g = CreateGraph(8, false, (0, 4), (1, 0), (2, 1), (2, 3), (3, 2), (4, 1), (5, 1), (5, 4), (5, 6),
                (6, 2), (6, 5), (7, 3), (7, 6), (7, 7));
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DirectedGraph<Empty, Empty>, VertexIdx, EdgeIdx>(g);
            var components = a.ComputeStronglyConnectedComponents(new VertexIdx(7));

            Assert.That(components.Length, Is.EqualTo(4));
            Assert.That(components[0].ToArray(), Is.EquivalentTo(Vertices(0, 1, 4)));
            Assert.That(components[1].ToArray(), Is.EquivalentTo(Vertices(2, 3)));
            Assert.That(components[2].ToArray(), Is.EquivalentTo(Vertices(5, 6)));
            Assert.That(components[3].ToArray(), Is.EquivalentTo(Vertices(7)));
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void ComputeStronglyConnectedComponents_DeepGraph_IsCorrect(int size)
        {
            var g = new DirectedGraph<Empty, Empty>();
            var v0 = g.AddVertex(Empty.Default);
            var v1 = v0;
            for (var i = 0; i < size; i++)
            {
                var v2 = g.AddVertex(Empty.Default);
                g.AddEdge(v1, v2, Empty.Default);
                v1 = v2;
            }

            var a = new TarjanStronglyConnectedComponentsAlgorithm<DirectedGraph<Empty, Empty>, VertexIdx, EdgeIdx>(g);
            var components = a.ComputeStronglyConnectedComponents(v0);
            Assert.That(components.Length, Is.EqualTo(size + 1));
        }

        private static DirectedGraph<Empty, Empty> CreateGraph(int numVertices, bool addReverseEdges, params (int Source, int Target)[] edges)
        {
            var g = new DirectedGraph<Empty, Empty>();
            for (var i = 0; i < numVertices; i++)
            {
                g.AddVertex(default);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(new VertexIdx(edge.Source), new VertexIdx(edge.Target), default);
                if (addReverseEdges)
                {
                    g.AddEdge(new VertexIdx(edge.Target), new VertexIdx(edge.Source), default);
                }
            }

            return g;
        }

        private static VertexIdx[] Vertices(params int[] indices)
        {
            return Array.ConvertAll(indices, x => new VertexIdx(x));
        }
    }
}