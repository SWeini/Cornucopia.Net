namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve the tag of vertices.
    /// </summary>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TTag">The type used to tag vertices.</typeparam>
    public interface ITaggedVertices<TVertexId, TTag>
        where TVertexId : notnull
    {
        /// <summary>
        ///     Gets the tag of a specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the tag of.</param>
        /// <returns>The tag of the specified vertex.</returns>
        TTag GetVertexTag(TVertexId vertex);
    }
}
