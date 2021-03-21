using System;
using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Computes the strongly connected components in a directed graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph to compute strongly connected components for.</typeparam>
    public class TarjanStronglyConnectedComponentsAlgorithm<TGraph>
        where TGraph : IEdges, IImplicitOutEdgesIndices
    {
        private readonly TGraph _graph;
        private readonly Dictionary<VertexIdx, DataPerVertex> _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TarjanStronglyConnectedComponentsAlgorithm{TGraph}"/> class with the graph.
        /// </summary>
        /// <param name="graph">The graph to compute strongly connected components for.</param>
        public TarjanStronglyConnectedComponentsAlgorithm(TGraph graph)
        {
            this._graph = graph;
            this._data = new Dictionary<VertexIdx, DataPerVertex>();
        }

        /// <summary>
        ///     Computes the strongly connected components in the graph that are reachable from a specified vertex.
        /// </summary>
        /// <param name="startVertex">The vertex where the algorithm should start.</param>
        /// <returns>Strongly connected components of the graph, that are reachable from <paramref name="startVertex"/> and weren't reported by previous calls to this method.</returns>
        public SplitArray<VertexIdx> ComputeStronglyConnectedComponents(VertexIdx startVertex)
        {
            var result = new SplitArrayBuilder<VertexIdx>(0, 0);
            if (this._data.ContainsKey(startVertex))
            {
                return result.Build();
            }

            var s = new DynamicArray<VertexIdx>(true);
            var index = 0;

            var stack = new DynamicArray<VertexEdgeIterator>(true);
            stack.AddLast(new(startVertex, -1));
            while (stack.TryRemoveLast(out var next))
            {
            startOfLoop:
                var v = next.Vertex;
                var edges = this._graph.GetOutEdges(v);
                if (next.LastEdge < 0)
                {
                    this._data[v] = new DataPerVertex(index);
                    index++;
                    s.AddLast(v);
                }
                else
                {
                    var w = this._graph.GetTarget(edges[next.LastEdge]);
                    var vData = this._data[v];
                    vData.LowLink = Math.Min(vData.LowLink, this._data[w].LowLink);
                    this._data[v] = vData;
                }

                for (var i = next.LastEdge + 1; i < edges.Length; i++)
                {
                    var w = this._graph.GetTarget(edges[i]);
                    if (!this._data.TryGetValue(w, out var wData))
                    {
                        stack.AddLast(new(v, i));
                        next = new(w, -1);
                        goto startOfLoop;
                    }
                    else if (wData.IsOnStack)
                    {
                        var vData = this._data[v];
                        vData.LowLink = Math.Min(vData.LowLink, wData.Index);
                        this._data[v] = vData;
                    }
                }

                var finalData = this._data[v];
                if (finalData.LowLink == finalData.Index)
                {
                    VertexIdx w;
                    do
                    {
                        w = s.RemoveLast();
                        var dw = this._data[w];
                        dw.IsOnStack = false;
                        this._data[w] = dw;
                        result.AddValue(w);
                    } while (w != v);

                    result.EndPart();
                }
            }

            return result.Build();
        }

        private readonly struct VertexEdgeIterator
        {
            public VertexEdgeIterator(VertexIdx vertex, int lastEdge)
            {
                this.Vertex = vertex;
                this.LastEdge = lastEdge;
            }

            public VertexIdx Vertex { get; }
            public int LastEdge { get; }
        }

        private struct DataPerVertex
        {
            public DataPerVertex(int index)
            {
                this.Index = index;
                this.LowLink = index;
                this.IsOnStack = true;
            }

            public int Index { get; }
            public int LowLink { get; set; }
            public bool IsOnStack { get; set; }
        }
    }
}
