using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     A directed graph, stored in compact index-based adjacency lists.
    /// </summary>
    /// <typeparam name="TVertex">The type of data tagged to a vertex.</typeparam>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    public class DirectedGraph<TVertex, TEdge> : IMutableGraph<VertexIdx, TVertex, EdgeIdx, TEdge>, IVertexSet, ITaggedVertices<VertexIdx, TVertex>, IEdgeSet, ITaggedEdges<EdgeIdx, TEdge>, IImplicitOutEdgesIndices<VertexIdx, EdgeIdx>, IImplicitInEdgesIndices<VertexIdx, EdgeIdx>, IEqualityComparerProvider<VertexIdx>, IEqualityComparerProvider<EdgeIdx>
    {
        private DynamicArrayFreeListAllocator<VertexData> _vertices;
        private DynamicArrayFreeListAllocator<EdgeData> _edges;
        private DynamicArrayBlockAllocator<EdgeIdx> _links;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectedGraph{TVertex,TEdge}"/> class that is empty.
        /// </summary>
        public DirectedGraph()
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
            ref var targetVertex = ref this._vertices[target.Index];
            targetVertex.EdgeStorage.AddInEdge(ref this._links, ref targetVertex.StartIndex, index);
            return index;
        }

        /// <inheritdoc/>
        public void RemoveEdge(EdgeIdx edge)
        {
            ref var edgeData = ref this._edges[edge.Index];
            ref var sourceVertex = ref this._vertices[edgeData.Source.Index];
            sourceVertex.EdgeStorage.RemoveOutEdge(ref this._links, ref sourceVertex.StartIndex, edge);
            ref var targetVertex = ref this._vertices[edgeData.Target.Index];
            targetVertex.EdgeStorage.RemoveInEdge(ref this._links, ref targetVertex.StartIndex, edge);
            this._edges.RemoveAt(edge.Index);
        }

        /// <inheritdoc/>
        public int VertexCount => this._vertices.Count;

        /// <inheritdoc/>
        public ref TVertex this[VertexIdx index] => ref this._vertices[index.Index].Data;

        /// <inheritdoc/>
        public TVertex GetVertexTag(VertexIdx vertex)
        {
            return this[vertex];
        }

        /// <inheritdoc/>
        public int EdgeCount => this._edges.Count;

        /// <inheritdoc/>
        public ref TEdge this[EdgeIdx index] => ref this._edges[index.Index].Data;

        /// <inheritdoc/>
        public TEdge GetEdgeTag(EdgeIdx edge)
        {
            return this[edge];
        }

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

        /// <inheritdoc/>
        public VertexIdx GetTarget(EdgeIdx index)
        {
            return this._edges[index.Index].Target;
        }

        /// <inheritdoc/>
        public int GetInDegree(VertexIdx vertex)
        {
            return this._vertices[vertex.Index].EdgeStorage.InDegree;
        }

        /// <inheritdoc/>
        public ReadOnlySpan<EdgeIdx> GetInEdges(VertexIdx index)
        {
            ref var vertex = ref this._vertices[index.Index];
            if (vertex.StartIndex < 0)
            {
                return ReadOnlySpan<EdgeIdx>.Empty;
            }

            return this._links.AsSpan(vertex.StartIndex + vertex.EdgeStorage.OutDegree, vertex.EdgeStorage.InDegree);
        }

        /// <inheritdoc/>
        public VertexIdx GetSource(EdgeIdx index)
        {
            return this._edges[index.Index].Source;
        }

        IEqualityComparer<VertexIdx>? IEqualityComparerProvider<VertexIdx>.Comparer => null;

        IEqualityComparer<EdgeIdx>? IEqualityComparerProvider<EdgeIdx>.Comparer => null;

        [DebuggerDisplay("{" + nameof(Data) + "}")]
        private struct VertexData
        {
            public TVertex Data;
            public int StartIndex;
            public DirectedOutInEdgeStorage EdgeStorage;
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
