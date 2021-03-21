using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class DirectedGraphWithoutInEdgesTests
    {
        [Test]
        public void Ctor_VertexCount_IsZero()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void Ctor_EdgeCount_IsZero()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void AddVertex_Empty_VertexCountIsOne()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            graph.AddVertex(0);
            Assert.That(graph.VertexCount, Is.EqualTo(1));
        }

        [Test]
        public void AddVertex_Empty_GetItemReturnsData()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v = graph.AddVertex(42);
            Assert.That(graph[v], Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_EdgeCountIsOne()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.EdgeCount, Is.EqualTo(1));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_GetItemReturnsData()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 42);
            Assert.That(graph[e], Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_SourceHasOutEdge()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
            var outEdges = graph.GetOutEdges(v1);
            Assert.That(outEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetSource(outEdges[0]), Is.EqualTo(v1));
            Assert.That(graph.GetTarget(outEdges[0]), Is.EqualTo(v2));
        }

        [Test]
        public void AddEdge_SecondOutEdge_SourceHasOutDegreeTwo()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(2));
        }

        [Test]
        public void AddEdge_HasSelfEdge_SourceHasOutDegreeTwo()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v1, 0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(2));
        }

        [Test]
        public void RemoveVertex_GraphWithSingleVertex_VertexCountIsZero()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v = graph.AddVertex(0);
            graph.RemoveVertex(v);
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void RemoveVertex_VertexHasSelfEdge_Throws()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v = graph.AddVertex(0);
            graph.AddEdge(v, v, 0);
            Assert.That(() => graph.RemoveVertex(v), Throws.ArgumentException);
        }

        [Test]
        public void RemoveEdge_SelfEdge_EdgeCountIsZero()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v = graph.AddVertex(0);
            var e = graph.AddEdge(v, v, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void RemoveEdge_SecondEdge_OutDegreeIsOne()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var e = graph.AddEdge(v1, v2, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
        }

        [Test]
        public void GetEdge_GraphWithEdge_SourceAndTargetAreCorrect()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v0 = graph.AddVertex(0);
            var v1 = graph.AddVertex(0);
            var e = graph.AddEdge(v0, v1, 0);
            var edge = graph.GetEdge(e);
            Assert.That(edge.Source, Is.EqualTo(v0));
            Assert.That(edge.Target, Is.EqualTo(v1));
        }

        [Test]
        public void GetEdge_GraphWithEdge_DataIsCorrect()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v0 = graph.AddVertex(0);
            var v1 = graph.AddVertex(0);
            var e = graph.AddEdge(v0, v1, 42);
            Assert.That(graph.GetEdge(e).Data, Is.EqualTo(42));
        }

        [Test]
        public void GetOutEdges_NoOutEdges_IsEmpty()
        {
            var graph = new DirectedGraphWithoutInEdges<int, int>();
            var v = graph.AddVertex(0);
            var edges = graph.GetOutEdges(v);
            Assert.That(edges.IsEmpty, Is.True);
        }
    }
}