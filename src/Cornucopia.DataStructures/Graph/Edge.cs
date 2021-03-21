namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Information about a single edge in a graph.
    /// </summary>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    public readonly struct Edge<TEdge>
    {
        internal Edge(TEdge data, VertexIdx source, VertexIdx target)
        {
            this.Data = data;
            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        ///     Gets the data tagged to the edge.
        /// </summary>
        /// <value>The data tagged to the edge.</value>
        public TEdge Data { get; }

        /// <summary>
        ///     Gets the source vertex of the edge.
        /// </summary>
        /// <value>The source vertex of the edge.</value>
        public VertexIdx Source { get; }

        /// <summary>
        ///     Gets the target vertex of the edge.
        /// </summary>
        /// <value>The target vertex of the edge.</value>
        public VertexIdx Target { get; }
    }
}
