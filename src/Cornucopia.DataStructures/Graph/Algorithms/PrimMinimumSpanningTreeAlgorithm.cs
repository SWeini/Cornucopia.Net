using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the minimum spanning tree for an undirected graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of graph to compute the minimum spanning tree for.</typeparam>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    /// <typeparam name="TDistance">The type used for distance calculations.</typeparam>
    public class PrimMinimumSpanningTreeAlgorithm<TGraph, TVertexId, TEdgeId, TDistance>
        where TGraph : IImplicitOutEdgesIndices<TVertexId, TEdgeId>, ITaggedEdges<TEdgeId, TDistance>, IEqualityComparerProvider<TVertexId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        private TGraph _graph;
        private readonly IComparer<TDistance> _comparer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TGraph,TVertexId,TEdgeId,TDistance}"/> class with the graph and the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute the minimum spanning tree for.</param>
        /// <param name="comparer">The comparer used to compare distances.</param>
        public PrimMinimumSpanningTreeAlgorithm(TGraph graph, IComparer<TDistance> comparer)
        {
            this._graph = graph;
            this._comparer = comparer;
        }

        /// <summary>
        ///     Computes the minimum spanning tree for the connected component containing a specified vertex.
        /// </summary>
        /// <param name="startVertex">The vertex where the algorithm should start.</param>
        /// <returns>The set of edges that define the minimum spanning tree.</returns>
        public ReadOnlySpan<TEdgeId> ComputeMinimumSpanningTree(TVertexId startVertex)
        {
            var heapNodes = new Dictionary<TVertexId, PairingHeap<VertexWithDistanceAndEdge>.ElementPointer>(this._graph.Comparer) { { startVertex, PairingHeap<VertexWithDistanceAndEdge>.ElementPointer.Undefined } };
            var todo = new PairingHeap<VertexWithDistanceAndEdge>(new DistanceComparer(this._comparer));
            var result = new DynamicArray<TEdgeId>(true);
            ProcessEdges(startVertex);
            while (todo.TryExtractMinimum(out var next))
            {
                heapNodes[next.Vertex] = PairingHeap<VertexWithDistanceAndEdge>.ElementPointer.Undefined;
                result.AddLast(next.Edge);
                ProcessEdges(next.Vertex);
            }

            return result.AsSpan();

            void ProcessEdges(TVertexId vertex)
            {
                foreach (var outEdgeIdx in this._graph.GetOutEdges(vertex))
                {
                    var target = this._graph.GetTarget(outEdgeIdx);
                    if (heapNodes.TryGetValue(target, out var vertexState))
                    {
                        if (vertexState.IsUndefined)
                        {
                            continue;
                        }

                        var currentBestDistanceToTarget = todo[vertexState].Distance;
                        var distance = this._graph.GetEdgeTag(outEdgeIdx);
                        if (this._comparer.Compare(distance, currentBestDistanceToTarget) >= 0)
                        {
                            continue;
                        }

                        todo.Decrease(vertexState, new(target, distance, outEdgeIdx));
                    }
                    else
                    {
                        var distance = this._graph.GetEdgeTag(outEdgeIdx);
                        var node = todo.Insert(new(target, distance, outEdgeIdx));
                        heapNodes.Add(target, node);
                    }
                }
            }
        }

        private readonly struct VertexWithDistanceAndEdge
        {
            public VertexWithDistanceAndEdge(TVertexId vertex, TDistance distance, TEdgeId edge)
            {
                this.Vertex = vertex;
                this.Distance = distance;
                this.Edge = edge;
            }

            public TVertexId Vertex { get; }
            public TDistance Distance { get; }
            public TEdgeId Edge { get; }
        }

        private class DistanceComparer : IComparer<VertexWithDistanceAndEdge>
        {
            private readonly IComparer<TDistance> _comparer;

            public DistanceComparer(IComparer<TDistance> comparer)
            {
                this._comparer = comparer;
            }

            public int Compare(VertexWithDistanceAndEdge x, VertexWithDistanceAndEdge y)
            {
                return this._comparer.Compare(x.Distance, y.Distance);
            }
        }
    }
}