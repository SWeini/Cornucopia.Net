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
    public class AStarShortestPathAlgorithm<TGraph, TVertexId, TEdgeId, TDistance>
        where TGraph : IImplicitOutEdgesIndices<TVertexId, TEdgeId>, ITaggedEdges<TEdgeId, TDistance>, IEqualityComparerProvider<TVertexId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        private TGraph _graph;
        private readonly IDistanceCalculator<TDistance> _calculator;
        private readonly Func<TVertexId, TVertexId, TDistance> _heuristicDistanceCalculator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DijkstraShortestPathAlgorithm{TGraph,TVertexId,TEdgeId,TDistance}"/> class with the graph and the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute shortest paths in.</param>
        /// <param name="calculator">The basic methods used to initialize, add and compare distances.</param>
        /// <param name="heuristicDistanceCalculator">The function that calculates the minimal distance between two vertices.</param>
        public AStarShortestPathAlgorithm(TGraph graph, IDistanceCalculator<TDistance> calculator, Func<TVertexId, TVertexId, TDistance> heuristicDistanceCalculator)
        {
            this._graph = graph;
            this._calculator = calculator;
            this._heuristicDistanceCalculator = heuristicDistanceCalculator;
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
            var vertices = new Dictionary<TVertexId, VertexInfo>(this._graph.Comparer);
            var queue = new PairingHeap<TVertexId, TDistance>(this._calculator);

            var startHeapElement = queue.Insert(new(startVertex, this._calculator.Zero));
            var startVertexInfo = new VertexInfo(this._heuristicDistanceCalculator(startVertex, targetVertex))
            {
                DistanceToVertex = this._calculator.Zero,
                HeapElement = startHeapElement
            };
            vertices.Add(startVertex, startVertexInfo);

            while (queue.TryExtractMinimum(out var next))
            {
                var currentVertexInfo = vertices[next.Key];
                var distanceSoFar = currentVertexInfo.DistanceToVertex;
                currentVertexInfo.HeapElement = PairingHeap<KeyValuePair<TVertexId, TDistance>>.ElementPointer.Undefined;
                if (this._graph.Equals(next.Key, targetVertex))
                {
                    distance = distanceSoFar;
                    return this.ResolveShortestPath(startVertex, targetVertex, vertices);
                }

                var edges = this._graph.GetOutEdges(next.Key);
                foreach (var edgeId in edges)
                {
                    var target = this._graph.GetTarget(edgeId);
                    var currentDistance = this._calculator.Add(distanceSoFar, this._graph.GetEdgeTag(edgeId));
                    if (vertices.TryGetValue(target, out var vertexInfo))
                    {
                        if (this._calculator.Compare(currentDistance, vertexInfo.DistanceToVertex) >= 0)
                        {
                            continue;
                        }

                        vertexInfo.DistanceToVertex = currentDistance;
                        vertexInfo.ShortestPathSource = next.Key;
                        vertexInfo.ShortestPathEdge = edgeId;
                        var priority = this._calculator.Add(currentDistance, vertexInfo.HeuristicDistanceToTarget);
                        if (vertexInfo.HeapElement.IsUndefined)
                        {
                            vertexInfo.HeapElement = queue.Insert(target, priority);
                        }
                        else
                        {
                            queue.Decrease(vertexInfo.HeapElement, priority);
                        }
                    }
                    else
                    {
                        var heuristicDistance = this._heuristicDistanceCalculator(target, targetVertex);
                        var priority = this._calculator.Add(currentDistance, heuristicDistance);
                        var heapElement = queue.Insert(target, priority);
                        vertexInfo = new(heuristicDistance)
                        {
                            HeapElement = heapElement,
                            DistanceToVertex = currentDistance,
                            ShortestPathSource = next.Key,
                            ShortestPathEdge = edgeId
                        };
                        vertices.Add(target, vertexInfo);
                    }
                }
            }

            throw new ArgumentException("Target vertex not reachable from start vertex.", nameof(targetVertex));
        }

        private ReadOnlySpan<TEdgeId> ResolveShortestPath(TVertexId startVertex, TVertexId targetVertex, Dictionary<TVertexId, VertexInfo> sources)
        {
            var result = new DynamicArray<TEdgeId>(true);
            var vertex = targetVertex;
            while (!this._graph.Equals(vertex, startVertex))
            {
                var info = sources[vertex];
                result.AddLast(info.ShortestPathEdge);
                vertex = info.ShortestPathSource;
            }

            var span = result.AsSpan();
            span.Reverse();
            return span;
        }

        private class VertexInfo
        {
            public VertexInfo(TDistance heuristicDistanceToTarget)
            {
                this.HeuristicDistanceToTarget = heuristicDistanceToTarget;
            }

            public PairingHeap<TVertexId, TDistance>.ElementPointer HeapElement { get; set; }
            public TDistance HeuristicDistanceToTarget { get; }
            public TDistance DistanceToVertex { get; set; } = default!;
            public TVertexId ShortestPathSource { get; set; } = default!;
            public TEdgeId ShortestPathEdge { get; set; } = default!;
        }
    }
}