using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    [TestFixture]
    public class AStarShortestPathAlgorithmTests
    {
        [Test]
        public void ComputeShortestPath_WikipediaExample_ReturnsCorrectDistance()
        {
            var g = new DirectedGraph<int, int>();
            var saarbrücken = g.AddVertex(222);
            var kaiserslautern = g.AddVertex(158);
            var ludwigshafen = g.AddVertex(108);
            var würzburg = g.AddVertex(0);
            var frankfurt = g.AddVertex(96);
            var karlsruhe = g.AddVertex(140);
            var heilbronn = g.AddVertex(87);
            g.AddEdge(saarbrücken, kaiserslautern, 70);
            g.AddEdge(kaiserslautern, frankfurt, 103);
            g.AddEdge(kaiserslautern, ludwigshafen, 53);
            g.AddEdge(frankfurt, würzburg, 116);
            g.AddEdge(ludwigshafen, würzburg, 183);
            g.AddEdge(saarbrücken, karlsruhe, 145);
            g.AddEdge(karlsruhe, heilbronn, 84);
            g.AddEdge(heilbronn, würzburg, 102);

            var a = new AStarShortestPathAlgorithm<DirectedGraph<int, int>, VertexIdx, EdgeIdx, int>(g, new IntCalculator(), (source, target) => g[source]);
            a.ComputeShortestPath(saarbrücken, würzburg, out var distance);
            Assert.That(distance, Is.EqualTo(289));
        }

        [Test]
        public void ComputeShortestPath_NonConsistent_ReturnsCorrectDistance()
        {
            var g = new DirectedGraph<int, int>();
            var start = g.AddVertex(40);
            var k1 = g.AddVertex(30);
            var k2 = g.AddVertex(0);
            var u = g.AddVertex(0);
            var ziel = g.AddVertex(0);
            g.AddEdge(start, k1, 10);
            g.AddEdge(start, u, 25);
            g.AddEdge(k1, k2, 20);
            g.AddEdge(u, k2, 10);
            g.AddEdge(k2, ziel, 10);

            var a = new AStarShortestPathAlgorithm<DirectedGraph<int, int>, VertexIdx, EdgeIdx, int>(g, new IntCalculator(), (source, _) => g[source]);
            a.ComputeShortestPath(start, ziel, out var distance);
            Assert.That(distance, Is.EqualTo(40));
        }

        [Test]
        public void ComputeShortestPath_NotReachable_Throws()
        {
            var g = new DirectedGraph<Empty, int>();
            var start = g.AddVertex(Empty.Default);
            var target = g.AddVertex(Empty.Default);
            var a = new AStarShortestPathAlgorithm<DirectedGraph<Empty, int>, VertexIdx, EdgeIdx, int>(g, new IntCalculator(), (_, _) => 0);
            Assert.That(() => a.ComputeShortestPath(start, target, out _), Throws.ArgumentException);
        }

        [Test]
        public void ComputeShortestPath_MultiplePaths_ReturnsShortest()
        {
            var g = new DirectedGraph<Empty, int>();
            var start = g.AddVertex(Empty.Default);
            var middle = g.AddVertex(Empty.Default);
            var target = g.AddVertex(Empty.Default);
            g.AddEdge(start, middle, 1);
            g.AddEdge(start, middle, 2);
            g.AddEdge(middle, target, 3);
            var a = new AStarShortestPathAlgorithm<DirectedGraph<Empty, int>, VertexIdx, EdgeIdx, int>(g, new IntCalculator(), (_, _) => 0);
            a.ComputeShortestPath(start, target, out var distance);
            Assert.That(distance, Is.EqualTo(4));
        }
    }
}