using System;
using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    [TestFixture]
    public class PrimMinimumSpanningTreeAlgorithmTests
    {
        [Test]
        public void ComputeMinimumSpanningTree_GraphWithoutEdges_IsEmpty()
        {
            var g = CreateGraph(1);
            var a = new PrimMinimumSpanningTreeAlgorithm<UndirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            var spanningTree = a.ComputeMinimumSpanningTree(new VertexIdx(0));
            Assert.That(spanningTree.IsEmpty, Is.True);
        }

        [Test]
        public void ComputeMinimumSpanningTree_SimpleGraph_IsCorrect()
        {
            var g = CreateGraph(3, (0, 1, 1), (1, 2, 2));
            var a = new PrimMinimumSpanningTreeAlgorithm<UndirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            var spanningTree = a.ComputeMinimumSpanningTree(new VertexIdx(0));
            Assert.That(spanningTree.ToArray(), Is.EquivalentTo(Edges(0, 1)));
        }

        [Test]
        public void ComputeMinimumSpanningTree_WikipediaExample_IsCorrect()
        {
            var g = CreateGraph(6, (0, 1, 1), (0, 3, 4), (0, 4, 3), (1, 3, 4), (1, 4, 2), (2, 4, 4), (2, 5, 5), (3, 4, 4), (4, 5, 7));
            var a = new PrimMinimumSpanningTreeAlgorithm<UndirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            var spanningTree = a.ComputeMinimumSpanningTree(new VertexIdx(0));
            Assert.That(spanningTree.Length, Is.EqualTo(5));
            Assert.That(spanningTree.ToArray().Sum(e => g.GetEdge(e).Data), Is.EqualTo(16));
        }

        private static UndirectedGraph<Empty, int> CreateGraph(int numVertices, params (int Source, int Target, int Distance)[] edges)
        {
            var g = new UndirectedGraph<Empty, int>();
            for (var i = 0; i < numVertices; i++)
            {
                g.AddVertex(default);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(new VertexIdx(edge.Source), new VertexIdx(edge.Target), edge.Distance);
            }

            return g;
        }

        private static EdgeIdx[] Edges(params int[] indices)
        {
            return Array.ConvertAll(indices, x => new EdgeIdx(x));
        }
    }
}