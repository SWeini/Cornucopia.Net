namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to get the number of vertices in a graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of data tagged to a vertex.</typeparam>
    public interface IVertexSet<TVertex> : IVertices<TVertex>
    {
        /// <summary>
        ///     Gets the number of vertices in this graph.
        /// </summary>
        /// <value>The number of vertices in this graph.</value>
        int VertexCount { get; }
    }
}
