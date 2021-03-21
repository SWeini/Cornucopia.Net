namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to get the number of edges in a graph.
    /// </summary>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    public interface IEdgeSet<TEdge> : IEdges<TEdge>
    {
        /// <summary>
        ///     Gets the number of edges in this graph.
        /// </summary>
        /// <value>The number of edges in this graph.</value>
        int EdgeCount { get; }
    }
}
