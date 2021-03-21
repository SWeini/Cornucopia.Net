namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to access the data tagged to vertices of a graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of data tagged to a vertex.</typeparam>
    public interface IVertices<TVertex>
    {
        /// <summary>
        ///     Gets a reference to the data tagged to a specified vertex.
        /// </summary>
        /// <param name="index">The vertex of the data reference to get.</param>
        /// <returns>The data reference of the specified vertex.</returns>
        /// <remarks>The reference is guaranteed to be valid as long as no vertices are added to or removed from the graph.</remarks>
        ref TVertex this[VertexIdx index] { get; }
    }
}
