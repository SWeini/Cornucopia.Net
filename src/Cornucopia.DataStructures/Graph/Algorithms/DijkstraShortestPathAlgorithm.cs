using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the shortest path between two vertices in an edge-weighted graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph to compute shortest paths in.</typeparam>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    /// <typeparam name="TDistance">The type used for distance calculations.</typeparam>
    public class DijkstraShortestPathAlgorithm<TGraph, TEdge, TDistance>
        where TGraph : IEdges<TEdge>, IImplicitOutEdgesIndices
    {
        private readonly TGraph _graph;
        private readonly IDistanceCalculator<TDistance> _calculator;
        private readonly IEdgeDistances<TEdge, TDistance> _distances;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DijkstraShortestPathAlgorithm{TGraph,TEdge,TDistance}"/> class with the graph and the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute shortest paths in.</param>
        /// <param name="calculator">The basic methods used to initialize, add and compare distances.</param>
        /// <param name="distances">The basic method to extract a distance from a tagged edge.</param>
        public DijkstraShortestPathAlgorithm(TGraph graph, IDistanceCalculator<TDistance> calculator, IEdgeDistances<TEdge, TDistance> distances)
        {
            this._graph = graph;
            this._calculator = calculator;
            this._distances = distances;
        }

        /// <summary>
        ///     Computes the shortest path between two specified vertices.
        /// </summary>
        /// <param name="startVertex">The vertex where the path should start.</param>
        /// <param name="targetVertex">The vertex where the path should end.</param>
        /// <param name="distance">The shortest distance from <paramref name="startVertex"/> to <paramref name="targetVertex"/>.</param>
        /// <returns>The shortest path, represented as a span of edges.</returns>
        /// <exception cref="ArgumentException">There is no path from <paramref name="startVertex"/> to <paramref name="targetVertex"/>.</exception>
        public ReadOnlySpan<EdgeIdx> ComputeShortestPath(VertexIdx startVertex, VertexIdx targetVertex, out TDistance distance)
        {
            var minimalDistance = new Dictionary<VertexIdx, TDistance>();
            var minimalSource = new Dictionary<VertexIdx, EdgeIdx>();
            var queue = new BinaryHeap<VertexWithDistance>(new DistanceComparer(this._calculator));
            queue.Insert(new(startVertex, this._calculator.Zero));
            while (queue.TryExtractMinimum(out var next))
            {
                if (next.Vertex == targetVertex)
                {
                    distance = next.Distance;
                    return ResolveShortestPath(startVertex, targetVertex, minimalSource);
                }

                var edges = this._graph.GetOutEdges(next.Vertex);
                foreach (var edgeIdx in edges)
                {
                    var edge = this._graph.GetEdge(edgeIdx);
                    var target = edge.Target;
                    var currentDistance = this._calculator.Add(next.Distance, this._distances.GetDistance(edge.Data));
                    if (minimalDistance.TryGetValue(target, out var oldDistance))
                    {
                        if (this._calculator.Compare(currentDistance, oldDistance) > 0)
                        {
                            continue;
                        }
                    }

                    minimalDistance[target] = currentDistance;
                    minimalSource[target] = edgeIdx;
                    queue.Insert(new(target, currentDistance));
                }
            }

            throw new ArgumentException("Target vertex not reachable from start vertex.", nameof(targetVertex));
        }

        private ReadOnlySpan<EdgeIdx> ResolveShortestPath(VertexIdx startVertex, VertexIdx targetVertex, Dictionary<VertexIdx, EdgeIdx> sources)
        {
            var result = new DynamicArray<EdgeIdx>(true);
            var vertex = targetVertex;
            while (vertex != startVertex)
            {
                var edgeIdx = sources[vertex];
                result.AddLast(edgeIdx);
                vertex = this._graph.GetEdge(edgeIdx).Source;
            }

            var span = result.AsSpan();
            span.Reverse();
            return span;
        }

        private readonly struct VertexWithDistance
        {
            public VertexWithDistance(VertexIdx vertex, TDistance distance)
            {
                this.Vertex = vertex;
                this.Distance = distance;
            }

            public VertexIdx Vertex { get; }
            public TDistance Distance { get; }
        }

        private class DistanceComparer : IComparer<VertexWithDistance>
        {
            private readonly IComparer<TDistance> _comparer;

            public DistanceComparer(IComparer<TDistance> comparer)
            {
                this._comparer = comparer;
            }

            public int Compare(VertexWithDistance x, VertexWithDistance y)
            {
                return this._comparer.Compare(x.Distance, y.Distance);
            }
        }
    }
}