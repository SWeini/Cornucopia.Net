namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to get the number of edges in a graph.
    /// </summary>
    public interface IEdgeSet
    {
        /// <summary>
        ///     Gets the number of edges in this graph.
        /// </summary>
        /// <value>The number of edges in this graph.</value>
        int EdgeCount { get; }
    }
}
