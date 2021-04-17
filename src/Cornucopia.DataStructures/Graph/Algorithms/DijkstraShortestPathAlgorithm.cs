using System;
using System.Collections.Generic;

using Cornucopia.DataStructures.Utils;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the shortest path between two vertices in an edge-weighted graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph to compute shortest paths in.</typeparam>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    /// <typeparam name="TDistance">The type used for distance calculations.</typeparam>
    public class DijkstraShortestPathAlgorithm<TGraph, TVertexId, TEdgeId, TDistance>
        where TGraph : IImplicitOutEdgesIndices<TVertexId, TEdgeId>, ITaggedEdges<TEdgeId, TDistance>, IEqualityComparerProvider<TVertexId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        private TGraph _graph;
        private readonly IDistanceCalculator<TDistance> _calculator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DijkstraShortestPathAlgorithm{TGraph,TVertexId,TEdgeId,TDistance}"/> class with the graph and the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute shortest paths in.</param>
        /// <param name="calculator">The basic methods used to initialize, add and compare distances.</param>
        public DijkstraShortestPathAlgorithm(TGraph graph, IDistanceCalculator<TDistance> calculator)
        {
            this._graph = graph;
            this._calculator = calculator;
        }

        /// <summary>
        ///     Computes the shortest path between two specified vertices.
        /// </summary>
        /// <param name="startVertex">The vertex where the path should start.</param>
        /// <param name="targetVertex">The vertex where the path should end.</param>
        /// <param name="distance">The shortest distance from <paramref name="startVertex"/> to <paramref name="targetVertex"/>.</param>
        /// <returns>The shortest path, represented as a span of edges.</returns>
        /// <exception cref="ArgumentException">There is no path from <paramref name="startVertex"/> to <paramref name="targetVertex"/>.</exception>
        public ReadOnlySpan<TEdgeId> ComputeShortestPath(TVertexId startVertex, TVertexId targetVertex, out TDistance distance)
        {
            var minimalDistance = new Dictionary<TVertexId, TDistance>(this._graph.Comparer);
            var minimalSource = new Dictionary<TVertexId, KeyValuePair<TVertexId, TEdgeId>>(this._graph.Comparer);
            var queue = new BinaryHeap<TVertexId, TDistance>(this._calculator);
            queue.Insert(new(startVertex, this._calculator.Zero));
            while (queue.TryExtractMinimum(out var next))
            {
                if (this._graph.Equals(next.Key, targetVertex))
                {
                    distance = next.Value;
                    return this.ResolveShortestPath(startVertex, targetVertex, minimalSource);
                }

                var edges = this._graph.GetOutEdges(next.Key);
                foreach (var edgeId in edges)
                {
                    var target = this._graph.GetTarget(edgeId);
                    var currentDistance = this._calculator.Add(next.Value, this._graph.GetEdgeTag(edgeId));
                    if (minimalDistance.TryGetValue(target, out var oldDistance))
                    {
                        if (this._calculator.Compare(currentDistance, oldDistance) >= 0)
                        {
                            continue;
                        }
                    }

                    minimalDistance[target] = currentDistance;
                    minimalSource[target] = new(next.Key, edgeId);
                    queue.Insert(new(target, currentDistance));
                }
            }

            throw new ArgumentException("Target vertex not reachable from start vertex.", nameof(targetVertex));
        }

        private ReadOnlySpan<TEdgeId> ResolveShortestPath(TVertexId startVertex, TVertexId targetVertex, Dictionary<TVertexId, KeyValuePair<TVertexId, TEdgeId>> sources)
        {
            var result = new DynamicArray<TEdgeId>(true);
            var vertex = targetVertex;
            while (!this._graph.Equals(vertex, startVertex))
            {
                var pair = sources[vertex];
                result.AddLast(pair.Value);
                vertex = pair.Key;
            }

            var span = result.AsSpan();
            span.Reverse();
            return span;
        }
    }
}