using System.Linq;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    [TestFixture]
    public class DijkstraShortestPathAlgorithmTests
    {
        [TestCase(0, 4, 4)]
        [TestCase(1, 3, 2)]
        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 0)]
        [TestCase(4, 4, 0)]
        public void ComputeShortestPath_SimpleGraph_ReturnsCorrectDistance(int start, int target, int distance)
        {
            var g = CreateGraph(5, false, (0, 1, 1), (1, 2, 1), (2, 3, 1), (3, 4, 1), (0, 4, 5));
            var a = new DijkstraShortestPathAlgorithm<DirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            a.ComputeShortestPath(new VertexIdx(start), new VertexIdx(target), out var shortestDistance);
            Assert.That(shortestDistance, Is.EqualTo(distance));
        }

        [Test]
        public void ComputeShortestPath_SimpleGraphNotReachable_Throws()
        {
            var g = CreateGraph(5, false, (0, 1, 1), (1, 2, 1), (2, 3, 1), (3, 4, 1), (0, 4, 5));
            var a = new DijkstraShortestPathAlgorithm<DirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            Assert.That(() => a.ComputeShortestPath(new VertexIdx(2), new VertexIdx(0), out _), Throws.ArgumentException);
        }

        [TestCase(0, 4, 0, 1, 2, 3)]
        [TestCase(1, 3, 1, 2)]
        public void ComputeShortestPath_SimpleGraph_IsCorrectPath(int start, int target, params int[] edges)
        {
            var g = CreateGraph(5, false, (0, 1, 1), (1, 2, 1), (2, 3, 1), (3, 4, 1), (0, 4, 5));
            var a = new DijkstraShortestPathAlgorithm<DirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            var path = a.ComputeShortestPath(new VertexIdx(start), new VertexIdx(target), out _);
            Assert.That(path.ToArray(), Is.EqualTo(edges.Select(x => new EdgeIdx(x))));
        }

        [Test]
        public void ComputeShortestPath_WikipediaExample_ReturnsCorrectDistance()
        {
            var g = CreateGraph(6, true, (0, 1, 7), (0, 2, 9), (0, 5, 15), (1, 2, 10), (1, 3, 15), (2, 3, 11), (2, 5, 2), (3, 4, 6), (4, 5, 9));
            var a = new DijkstraShortestPathAlgorithm<DirectedGraph<Empty, int>, int, int>(g, new IntDistances(), new IntDistances());
            a.ComputeShortestPath(new VertexIdx(0), new VertexIdx(4), out var shortestDistance);
            Assert.That(shortestDistance, Is.EqualTo(20));
        }

        private static DirectedGraph<Empty, int> CreateGraph(int numVertices, bool addReverseEdges, params (int Source, int Target, int Distance)[] edges)
        {
            var g = new DirectedGraph<Empty, int>();
            for (var i = 0; i < numVertices; i++)
            {
                g.AddVertex(default);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(new VertexIdx(edge.Source), new VertexIdx(edge.Target), edge.Distance);
                if (addReverseEdges)
                {
                    g.AddEdge(new VertexIdx(edge.Target), new VertexIdx(edge.Source), edge.Distance);
                }
            }

            return g;
        }
    }
}