using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the minimum spanning tree for an undirected graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of graph to compute the minimum spanning tree for.</typeparam>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    /// <typeparam name="TDistance">The type used for distance calculations.</typeparam>
    public class PrimMinimumSpanningTreeAlgorithm<TGraph, TEdge, TDistance>
        where TGraph : IEdges<TEdge>, IImplicitOutEdgesIndices
    {

        private readonly TGraph _graph;
        private readonly IComparer<TDistance> _comparer;
        private readonly IEdgeDistances<TEdge, TDistance> _distances;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TGraph,TEdge,TDistance}"/> class with the graph and the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute the minimum spanning tree for.</param>
        /// <param name="comparer">The comparer used to compare distances.</param>
        /// <param name="distances">The basic method to extract a distance from a tagged edge.</param>
        public PrimMinimumSpanningTreeAlgorithm(TGraph graph, IDistanceCalculator<TDistance> comparer, IEdgeDistances<TEdge, TDistance> distances)
        {
            this._graph = graph;
            this._comparer = comparer;
            this._distances = distances;
        }

        /// <summary>
        ///     Computes the minimum spanning tree for the connected component containing a specified vertex.
        /// </summary>
        /// <param name="startVertex">The vertex where the algorithm should start.</param>
        /// <returns>The set of edges that define the minimum spanning tree.</returns>
        public ReadOnlySpan<EdgeIdx> ComputeMinimumSpanningTree(VertexIdx startVertex)
        {
            var heapNodes = new Dictionary<VertexIdx, PairingHeap<VertexWithDistanceAndEdge>.ElementPointer> { { startVertex, PairingHeap<VertexWithDistanceAndEdge>.ElementPointer.Undefined } };
            var todo = new PairingHeap<VertexWithDistanceAndEdge>(new DistanceComparer(this._comparer));
            var result = new DynamicArray<EdgeIdx>(true);
            ProcessEdges(startVertex);
            while (todo.TryExtractMinimum(out var next))
            {
                heapNodes[next.Vertex] = PairingHeap<VertexWithDistanceAndEdge>.ElementPointer.Undefined;
                result.AddLast(next.Edge);
                ProcessEdges(next.Vertex);
            }

            return result.AsSpan();

            void ProcessEdges(VertexIdx vertex)
            {
                foreach (var outEdgeIdx in this._graph.GetOutEdges(vertex))
                {
                    var outEdge = this._graph.GetEdge(outEdgeIdx);
                    if (heapNodes.TryGetValue(outEdge.Target, out var vertexState))
                    {
                        if (vertexState.IsUndefined)
                        {
                            continue;
                        }

                        var currentBestDistanceToTarget = todo[vertexState].Distance;
                        var distance = this._distances.GetDistance(outEdge.Data);
                        if (this._comparer.Compare(distance, currentBestDistanceToTarget) >= 0)
                        {
                            continue;
                        }

                        todo.Decrease(vertexState, new(outEdge.Target, distance, outEdgeIdx));
                    }
                    else
                    {
                        var distance = this._distances.GetDistance(outEdge.Data);
                        var node = todo.Insert(new(outEdge.Target, distance, outEdgeIdx));
                        heapNodes.Add(outEdge.Target, node);
                    }
                }
            }
        }

        private readonly struct VertexWithDistanceAndEdge
        {
            public VertexWithDistanceAndEdge(VertexIdx vertex, TDistance distance, EdgeIdx edge)
            {
                this.Vertex = vertex;
                this.Distance = distance;
                this.Edge = edge;
            }

            public VertexIdx Vertex { get; }
            public TDistance Distance { get; }
            public EdgeIdx Edge { get; }
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