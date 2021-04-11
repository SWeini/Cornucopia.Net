using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Cornucopia.DataStructures.Utils;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the maximum flow between two vertices in an edge-weighted graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph to compute maximum flow in.</typeparam>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    /// <typeparam name="TCapacity">The type used for capacity and flow calculations.</typeparam>
    public class EdmondsKarpMaximumFlowAlgorithm<TGraph, TVertexId, TEdgeId, TCapacity>
        where TGraph : IImplicitOutEdgesIndices<TVertexId, TEdgeId>, IImplicitInEdgesIndices<TVertexId, TEdgeId>, ITaggedEdges<TEdgeId, TCapacity>, IEqualityComparerProvider<TVertexId>, IEqualityComparerProvider<TEdgeId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        private TGraph _graph;
        private readonly ICapacityCalculator<TCapacity> _calculator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdmondsKarpMaximumFlowAlgorithm{TGraph,TVertexId,TEdgeId,TCapacity}"/> class with the graph end the necessary basic calculations.
        /// </summary>
        /// <param name="graph">The graph to compute maximum flow in.</param>
        /// <param name="calculator">The basic methods used to initialize, add, negate and compare capacity and flow.</param>
        public EdmondsKarpMaximumFlowAlgorithm(TGraph graph, ICapacityCalculator<TCapacity> calculator)
        {
            this._graph = graph;
            this._calculator = calculator;
        }

        /// <summary>
        ///     Computes the maximum flow between two specified vertices.
        /// </summary>
        /// <param name="source">The vertex where the flow should start.</param>
        /// <param name="target">The vertex where the flow should end.</param>
        /// <param name="sourcePartition">The partition of the corresponding minimum cut, which contains <paramref name="source"/>.</param>
        /// <returns>The maximum flow from <paramref name="source"/> to <paramref name="target"/>.</returns>
        public TCapacity ComputeMaximumFlow(TVertexId source, TVertexId target, out TVertexId[] sourcePartition)
        {
            var result = this._calculator.Zero;
            var flows = new Dictionary<TEdgeId, TCapacity>(this._graph.GetComparer<TGraph, TEdgeId>());
            var predecessors = new Dictionary<TVertexId, DirectedEdge>(this._graph.GetComparer<TGraph, TVertexId>());
            var queue = new Queue<TVertexId>();
            while (true)
            {
                queue.Enqueue(source);
                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();
                    var outEdges = this._graph.GetOutEdges(currentVertex);
                    foreach (var edgeIdx in outEdges)
                    {
                        var t = this._graph.GetTarget(edgeIdx);
                        if (!this._graph.Equals(t, source) && !predecessors.ContainsKey(t))
                        {
                            if (!flows.TryGetValue(edgeIdx, out var flow))
                            {
                                flow = this._calculator.Zero;
                            }

                            var capacity = this._graph.GetEdgeTag(edgeIdx);
                            if (this._calculator.Compare(capacity, flow) > 0)
                            {
                                predecessors[t] = new(edgeIdx, false);
                                queue.Enqueue(t);
                            }
                        }
                    }

                    var inEdges = this._graph.GetInEdges(currentVertex);
                    foreach (var edgeIdx in inEdges)
                    {
                        var s = this._graph.GetSource(edgeIdx);
                        if (!this._graph.Equals(s, source) && !predecessors.ContainsKey(s))
                        {
                            if (flows.TryGetValue(edgeIdx, out var flow))
                            {
                                if (this._calculator.Compare(flow, this._calculator.Zero) > 0)
                                {
                                    predecessors[s] = new(edgeIdx, true);
                                    queue.Enqueue(s);
                                }
                            }
                        }
                    }
                }

                var augmentingPath = GetAugmentingPath(target);
                if (augmentingPath.IsEmpty)
                {
                    predecessors[source] = default;
                    sourcePartition = predecessors.Keys.ToArray();
                    return result;
                }

                var deltaFlow = GetResidualCapacity(augmentingPath[0]);
                for (var i = 1; i < augmentingPath.Length; i++)
                {
                    var residualCapacity = GetResidualCapacity(augmentingPath[i]);
                    if (this._calculator.Compare(residualCapacity, deltaFlow) < 0)
                    {
                        deltaFlow = residualCapacity;
                    }
                }

                var negativeDeltaFlow = this._calculator.Negate(deltaFlow);
                foreach (var e in augmentingPath)
                {
                    if (!flows.TryGetValue(e.Edge, out var flow))
                    {
                        flow = this._calculator.Zero;
                    }

                    flow = this._calculator.Add(flow, e.IsReverse ? negativeDeltaFlow : deltaFlow);
                    flows[e.Edge] = flow;
                }

                result = this._calculator.Add(result, deltaFlow);
                predecessors.Clear();
            }

            TCapacity GetResidualCapacity(DirectedEdge e)
            {
                if (flows.TryGetValue(e.Edge, out var f))
                {
                    if (e.IsReverse)
                    {
                        return f;
                    }

                    var capacity = this._graph.GetEdgeTag(e.Edge);
                    return this._calculator.Add(capacity, this._calculator.Negate(f));
                }

                Debug.Assert(!e.IsReverse);
                return this._graph.GetEdgeTag(e.Edge);
            }

            ReadOnlySpan<DirectedEdge> GetAugmentingPath(TVertexId t)
            {
                var path = new DynamicArray<DirectedEdge>(true);
                while (true)
                {
                    if (!predecessors.TryGetValue(t, out var e))
                    {
                        return path.AsSpan();
                    }

                    path.AddLast(e);
                    t = e.IsReverse ? this._graph.GetTarget(e.Edge) : this._graph.GetSource(e.Edge);
                }
            }
        }

        private readonly struct DirectedEdge
        {
            public DirectedEdge(TEdgeId edge, bool isReverse)
            {
                this.Edge = edge;
                this.IsReverse = isReverse;
            }

            public TEdgeId Edge { get; }
            public bool IsReverse { get; }
        }
    }
}