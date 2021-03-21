using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class UndirectedGraphTests
    {
        [Test]
        public void Ctor_VertexCount_IsZero()
        {
            var graph = new UndirectedGraph<int, int>();
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void Ctor_EdgeCount_IsZero()
        {
            var graph = new UndirectedGraph<int, int>();
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void AddVertex_Empty_VertexCountIsOne()
        {
            var graph = new UndirectedGraph<int, int>();
            graph.AddVertex(0);
            Assert.That(graph.VertexCount, Is.EqualTo(1));
        }

        [Test]
        public void AddVertex_Empty_GetItemReturnsData()
        {
            var graph = new UndirectedGraph<int, int>();
            var v = graph.AddVertex(42);
            Assert.That(graph[v], Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_EdgeCountIsOne()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.EdgeCount, Is.EqualTo(1));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_GetItemReturnsData()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 42);
            Assert.That(graph[e], Is.EqualTo(42));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_SourceHasOutEdge()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
            var outEdges = graph.GetOutEdges(v1);
            Assert.That(outEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetTarget(outEdges[0]), Is.EqualTo(v2));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_FirstVertexHasEdgeToSecondVertex()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var outEdges = graph.GetOutEdges(v1);
            Assert.That(outEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetSource(outEdges[0]), Is.EqualTo(v1));
            Assert.That(graph.GetTarget(outEdges[0]), Is.EqualTo(v2));
        }

        [Test]
        public void AddEdge_GraphWithTwoVertices_SecondVertexHasEdgeToFirstVertex()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var outEdges = graph.GetOutEdges(v2);
            Assert.That(outEdges.Length, Is.EqualTo(1));
            Assert.That(graph.GetSource(outEdges[0]), Is.EqualTo(v2));
            Assert.That(graph.GetTarget(outEdges[0]), Is.EqualTo(v1));
        }

        [Test]
        public void AddEdge_SecondOutEdge_SourceHasOutDegreeTwo()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(2));
        }

        [Test]
        public void AddEdge_SelfEdge_VertexHasDegreeOne()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            graph.AddEdge(v1, v1, 0);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
        }

        [Test]
        public void RemoveVertex_GraphWithSingleVertex_VertexCountIsZero()
        {
            var graph = new UndirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.RemoveVertex(v);
            Assert.That(graph.VertexCount, Is.Zero);
        }

        [Test]
        public void RemoveVertex_VertexHasSelfEdge_Throws()
        {
            var graph = new UndirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            graph.AddEdge(v, v, 0);
            Assert.That(() => graph.RemoveVertex(v), Throws.ArgumentException);
        }

        [Test]
        public void RemoveEdge_NormalEdge_EdgeCountIsZero()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void RemoveEdge_SelfEdge_EdgeCountIsZero()
        {
            var graph = new UndirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            var e = graph.AddEdge(v, v, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.EdgeCount, Is.Zero);
        }

        [Test]
        public void RemoveEdge_SecondEdge_OutDegreeIsOne()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var e = graph.AddEdge(v1, v2, 0);
            graph.RemoveEdge(e);
            Assert.That(graph.GetOutDegree(v1), Is.EqualTo(1));
            Assert.That(graph.GetOutDegree(v2), Is.EqualTo(1));
        }

        [Test]
        public void GetEdge_GraphWithEdge_SourceAndTargetAreCorrect()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetEdge(e).Source, Is.EqualTo(v1));
            Assert.That(graph.GetEdge(e).Target, Is.EqualTo(v2));
        }

        [Test]
        public void GetEdge_GraphWithEdge_DataIsCorrect()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            var e = graph.AddEdge(v1, v2, 42);
            Assert.That(graph.GetEdge(e).Data, Is.EqualTo(42));
        }

        [Test]
        public void GetEdge_ReverseEdge_SourceAndTargetAreCorrect()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            var e = graph.GetOutEdges(v2)[0];
            Assert.That(graph.GetEdge(e).Source, Is.EqualTo(v2));
            Assert.That(graph.GetEdge(e).Target, Is.EqualTo(v1));
        }

        [Test]
        public void GetTarget_GraphWithEdge_IsCorrectTargetVertex()
        {
            var graph = new UndirectedGraph<int, int>();
            var v1 = graph.AddVertex(0);
            var v2 = graph.AddVertex(0);
            graph.AddEdge(v1, v2, 0);
            Assert.That(graph.GetTarget(graph.GetOutEdges(v1)[0]), Is.EqualTo(v2));
            Assert.That(graph.GetTarget(graph.GetOutEdges(v2)[0]), Is.EqualTo(v1));
        }

        [Test]
        public void GetOutEdges_NoOutEdges_IsEmpty()
        {
            var graph = new UndirectedGraph<int, int>();
            var v = graph.AddVertex(0);
            var edges = graph.GetOutEdges(v);
            Assert.That(edges.IsEmpty, Is.True);
        }
    }
}