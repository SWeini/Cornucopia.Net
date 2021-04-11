using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class DirectedGraphTests
    {
        [Test]
        public void Ctor_VertexCount_IsZero()
        {
            var graph = new DirectedGraph<int, int>();
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void Ctor_EdgeCount_IsZero()
        {
            var graph = new DirectedGraph<int, int>();
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void AddVertex_Empty_VertexCountIsOne()
        {
            var graph = new DirectedGraph<int, int>();
            graph.AddVertex(0);
            Assert.That(graph.VertexCount, Is.EqualTo(1));
        }

        [Test]
        public void AddVertex_Empty_GetVertexTagReturnsData()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(42);
            Assert.That(graph.GetVertexTag(v), Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_EdgeCountIsOne()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.EdgeCount, Is.EqualTo(1));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_GetEdgeTagReturnsData()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 42);
            Assert.That(graph.GetEdgeTag(e), Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_SourceHasOutEdge()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
            var outEdges = graph.GetOutEdges(v1);
            Assert.That(outEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetTarget(outEdges[0]), Is.EqualTo(v2));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_TargetHasInEdge()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetInDegree(v2), Is.EqualTo(1));
            var inEdges = graph.GetInEdges(v2);
            Assert.That(inEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetSource(inEdges[0]), Is.EqualTo(v1));
        }

        [Test]
        public void AddEdge_SecondOutEdge_SourceHasOutDegreeTwo()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(2));
        }

        [Test]
        public void AddEdge_HasSelfEdge_SourceHasOutDegreeTwo()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v1, 0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(2));
        }

        [Test]
        public void AddEdge_SelfEdge_VertexHasOutDegreeOne()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.AddEdge(v, v, 0);
            Assert.That(graph.GetOutDegree(v), Is.EqualTo(1));
        }

        [Test]
        public void AddEdge_SelfEdge_VertexHasInDegreeOne()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.AddEdge(v, v, 0);
            Assert.That(graph.GetInDegree(v), Is.EqualTo(1));
        }

        [Test]
        public void RemoveVertex_GraphWithSingleVertex_VertexCountIsZero()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.RemoveVertex(v);
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void RemoveVertex_VertexHasSelfEdge_Throws()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.AddEdge(v, v, 0);
            Assert.That(() => graph.RemoveVertex(v), Throws.ArgumentException);
        }

        [Test]
        public void RemoveEdge_SelfEdge_EdgeCountIsZero()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            var e = graph.AddEdge(v, v, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void RemoveEdge_SingleEdge_OutDegreeIsZero()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.GetOutDegree(v1), Is.Zero);
        }

        [Test]
        public void RemoveEdge_SecondEdge_OutDegreeIsOne()
        {
            var graph = new DirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var e = graph.AddEdge(v1, v2, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
        }

        [Test]
        public void GetOutEdges_NoOutEdges_IsEmpty()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            var edges = graph.GetOutEdges(v);
            Assert.That(edges.IsEmpty, Is.True);
        }

        [Test]
        public void GetInEdges_NoInEdges_IsEmpty()
        {
            var graph = new DirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            var edges = graph.GetInEdges(v);
            Assert.That(edges.IsEmpty, Is.True);
        }
    }
}