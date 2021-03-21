namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to access the edges of a graph.
    /// </summary>
    public interface IEdges
    {
        /// <summary>
        ///     Gets the source vertex of a specified edge in this graph.
        /// </summary>
        /// <param name="index">The edge of which to get the source vertex.</param>
        /// <returns>The source vertex of the specified edge.</returns>
        VertexIdx GetSource(EdgeIdx index);

        /// <summary>
        ///     Gets the target vertex of a specified edge in this graph.
        /// </summary>
        /// <param name="index">The edge of which to get the target vertex.</param>
        /// <returns>The target vertex of the specified edge.</returns>
        VertexIdx GetTarget(EdgeIdx index);
    }

    /// <summary>
    ///     Defines methods to access the edges of a graph.
    /// </summary>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    public interface IEdges<TEdge> : IEdges
    {
        /// <summary>
        ///     Gets a reference to the data tagged to a specified edge.
        /// </summary>
        /// <param name="index">The edge of the data reference to get.</param>
        /// <returns>The data reference of the specified ege.</returns>
        /// <remarks>The reference is guaranteed to be valid as long as no edges are added to or removed from the graph.</remarks>
        ref TEdge this[EdgeIdx index] { get; }

        /// <summary>
        ///     Gets a specified edge of this graph.
        /// </summary>
        /// <param name="index">The index of the edge to get.</param>
        /// <returns>The edge corresponding to <paramref name="index"/>.</returns>
        Edge<TEdge> GetEdge(EdgeIdx index);

    }
}
