namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve the tag of edges.
    /// </summary>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    /// <typeparam name="TTag">The type used to tag edges.</typeparam>
    public interface ITaggedEdges<TEdgeId, TTag>
        where TEdgeId : notnull
    {
        /// <summary>
        ///     Gets the tag of a specified edge.
        /// </summary>
        /// <param name="edge">The edge to get the tag of.</param>
        /// <returns>The tag of the specified edge.</returns>
        TTag GetEdgeTag(TEdgeId edge);
    }
}