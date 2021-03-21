using System;
using System.Diagnostics;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     A directed graph, stored in compact index-based adjacency lists.
    /// </summary>
    /// <typeparam name="TVertex">The type of data tagged to a vertex.</typeparam>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    /// <remarks>Use <see cref="DirectedGraph{TVertex,TEdge}"/> if you need to query inbound edges.</remarks>
    public class DirectedGraphWithoutInEdges<TVertex, TEdge> : IMutableGraph<TVertex, TEdge>, IVertexSet<TVertex>, IEdgeSet<TEdge>, IImplicitOutEdgesIndices
    {
        private DynamicArrayFreeListAllocator<VertexData> _vertices;
        private DynamicArrayFreeListAllocator<EdgeData> _edges;
        private DynamicArrayBlockAllocator<EdgeIdx> _links;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectedGraphWithoutInEdges{TVertex,TEdge}"/> class that is empty.
        /// </summary>
        public DirectedGraphWithoutInEdges()
        {
            this._vertices = new(0);
            this._edges = new(0);
            this._links = new(0, 31);
        }

        /// <inheritdoc/>
        public VertexIdx AddVertex(TVertex vertex)
        {
            var vertexData = new VertexData { Data = vertex, StartIndex = -1, EdgeStorage = default };
            var index = new VertexIdx(this._vertices.Add(vertexData));
            return index;
        }

        /// <inheritdoc/>
        public void RemoveVertex(VertexIdx vertex)
        {
            ref var vertexData = ref this._vertices[vertex.Index];
            if (vertexData.StartIndex >= 0)
            {
                throw new ArgumentException("Can only remove isolated vertices", nameof(vertex));
            }

            this._vertices.RemoveAt(vertex.Index);
        }

        /// <inheritdoc/>
        public EdgeIdx AddEdge(VertexIdx source, VertexIdx target, TEdge edge)
        {
            var edgeData = new EdgeData { Data = edge, Source = source, Target = target };
            var index = new EdgeIdx(this._edges.Add(edgeData));
            ref var sourceVertex = ref this._vertices[source.Index];
            sourceVertex.EdgeStorage.AddOutEdge(ref this._links, ref sourceVertex.StartIndex, index);
            return index;
        }

        /// <inheritdoc/>
        public void RemoveEdge(EdgeIdx edge)
        {
            ref var edgeData = ref this._edges[edge.Index];
            ref var sourceVertex = ref this._vertices[edgeData.Source.Index];
            sourceVertex.EdgeStorage.RemoveOutEdge(ref this._links, ref sourceVertex.StartIndex, edge);
            this._edges.RemoveAt(edge.Index);
        }

        /// <inheritdoc/>
        public ref TVertex this[VertexIdx index] => ref this._vertices[index.Index].Data;

        /// <inheritdoc/>
        public int VertexCount => this._vertices.Count;

        /// <inheritdoc/>
        public ref TEdge this[EdgeIdx index] => ref this._edges[index.Index].Data;

        /// <inheritdoc/>
        public Edge<TEdge> GetEdge(EdgeIdx index)
        {
            ref var edge = ref this._edges[index.Index];
            return new Edge<TEdge>(edge.Data, edge.Source, edge.Target);
        }

        /// <inheritdoc/>
        public VertexIdx GetSource(EdgeIdx index)
        {
            return this._edges[index.Index].Source;
        }

        /// <inheritdoc/>
        public VertexIdx GetTarget(EdgeIdx index)
        {
            return this._edges[index.Index].Target;
        }

        /// <inheritdoc/>
        public int EdgeCount => this._edges.Count;

        /// <inheritdoc/>
        public int GetOutDegree(VertexIdx index)
        {
            return this._vertices[index.Index].EdgeStorage.OutDegree;
        }

        /// <inheritdoc/>
        public ReadOnlySpan<EdgeIdx> GetOutEdges(VertexIdx index)
        {
            ref var vertex = ref this._vertices[index.Index];
            if (vertex.StartIndex < 0)
            {
                return ReadOnlySpan<EdgeIdx>.Empty;
            }

            return this._links.AsSpan(vertex.StartIndex, vertex.EdgeStorage.OutDegree);
        }

        [DebuggerDisplay("{" + nameof(Data) + "}")]
        private struct VertexData
        {
            public TVertex Data;
            public int StartIndex;
            public DirectedOutEdgeStorage EdgeStorage;
        }

        [DebuggerDisplay("{" + nameof(Source) + "} -> {" + nameof(Target) + "} ({" + nameof(Data) + "})")]
        private struct EdgeData
        {
            public TEdge Data;
            public VertexIdx Source;
            public VertexIdx Target;
        }
    }
}