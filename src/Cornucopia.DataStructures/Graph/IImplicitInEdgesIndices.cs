using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve inbound edges of a graph.
    /// </summary>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    public interface IImplicitInEdgesIndices<TVertexId, TEdgeId>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        /// <summary>
        ///     Gets the indegree of a specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to examine.</param>
        /// <returns>The indegree of the specified vertex.</returns>
        int GetInDegree(TVertexId vertex);

        /// <summary>
        ///     Gets the inbound edges of a specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to examine.</param>
        /// <returns>The inbound edges of the specified vertex.</returns>
        ReadOnlySpan<TEdgeId> GetInEdges(TVertexId vertex);

        /// <summary>
        ///     Gets the source vertex of a specified edge in this graph.
        /// </summary>
        /// <param name="edge">The edge of which to get the source vertex.</param>
        /// <returns>The source vertex of the specified edge.</returns>
        TVertexId GetSource(TEdgeId edge);
    }
}
