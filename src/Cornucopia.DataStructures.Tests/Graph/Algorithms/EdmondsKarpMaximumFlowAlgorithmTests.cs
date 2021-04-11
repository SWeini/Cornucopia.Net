using System;
using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    [TestFixture]
    public class EdmondsKarpMaximumFlowAlgorithmTests
    {
        [Test]
        public void ComputeMaximumFlow_WikipediaExample_ReturnsCorrectFlow()
        {
            var g = CreateGraph(7, (0, 3, 3), (0, 1, 3), (1, 2, 4), (2, 0, 3), (2, 3, 1), (2, 4, 2), (3, 4, 2), (3, 5, 6), (4, 1, 1), (4, 6, 1), (5, 6, 9));
            var a = new EdmondsKarpMaximumFlowAlgorithm<DirectedGraph<Empty, int>, VertexIdx, EdgeIdx, int>(g, new IntCalculator());
            var maximumFlow = a.ComputeMaximumFlow(new VertexIdx(0), new VertexIdx(6), out var sourcePartition);
            Assert.That(maximumFlow, Is.EqualTo(5));
            Assert.That(sourcePartition, Is.EquivalentTo(Vertices(0, 1, 2, 4)));
        }

        private static DirectedGraph<Empty, int> CreateGraph(int numVertices, params (int Source, int Target, int Capacity)[] edges)
        {
            var g = new DirectedGraph<Empty, int>();
            for (var i = 0; i < numVertices; i++)
            {
                g.AddVertex(default);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(new VertexIdx(edge.Source), new VertexIdx(edge.Target), edge.Capacity);
            }

            return g;
        }

        private static VertexIdx[] Vertices(params int[] indices)
        {
            return Array.ConvertAll(indices, x => new VertexIdx(x));
        }
    }
}