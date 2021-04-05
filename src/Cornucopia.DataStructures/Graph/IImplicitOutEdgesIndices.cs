using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve outbound edges of a graph.
    /// </summary>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    public interface IImplicitOutEdgesIndices<TVertexId, TEdgeId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        /// <summary>
        ///     Gets the outdegree of a specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to examine.</param>
        /// <returns>The outdegree of the specified vertex.</returns>
        int GetOutDegree(TVertexId vertex);

        /// <summary>
        ///     Gets the outbound edges of a specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to examine.</param>
        /// <returns>The outbound edges of the specified vertex.</returns>
        ReadOnlySpan<TEdgeId> GetOutEdges(TVertexId vertex);

        /// <summary>
        ///     Gets the target vertex of a specified edge in this graph.
        /// </summary>
        /// <param name="edge">The edge of which to get the target vertex.</param>
        /// <returns>The target vertex of the specified edge.</returns>
        TVertexId GetTarget(TEdgeId edge);
    }
}
