using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve outbound edges of a graph.
    /// </summary>
    public interface IImplicitOutEdgesIndices
    {
        /// <summary>
        ///     Gets the outdegree of a specified vertex.
        /// </summary>
        /// <param name="index">The vertex to examine.</param>
        /// <returns>The outdegree of the specified vertex.</returns>
        int GetOutDegree(VertexIdx index);

        /// <summary>
        ///     Gets the outbound edges of a specified vertex.
        /// </summary>
        /// <param name="index">The vertex to examine.</param>
        /// <returns>The outbound edges of the specified vertex.</returns>
        ReadOnlySpan<EdgeIdx> GetOutEdges(VertexIdx index);
    }
}
