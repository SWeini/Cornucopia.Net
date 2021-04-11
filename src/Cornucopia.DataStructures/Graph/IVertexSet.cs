namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to get the number of vertices in a graph.
    /// </summary>
    public interface IVertexSet
    {
        /// <summary>
        ///     Gets the number of vertices in this graph.
        /// </summary>
        /// <value>The number of vertices in this graph.</value>
        int VertexCount { get; }
    }
}
