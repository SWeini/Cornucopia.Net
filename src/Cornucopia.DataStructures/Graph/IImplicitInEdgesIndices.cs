using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to retrieve inbound edges of a graph.
    /// </summary>
    public interface IImplicitInEdgesIndices
    {
        /// <summary>
        ///     Gets the indegree of a specified vertex.
        /// </summary>
        /// <param name="index">The vertex to examine.</param>
        /// <returns>The indegree of the specified vertex.</returns>
        int GetInDegree(VertexIdx index);

        /// <summary>
        ///     Gets the inbound edges of a specified vertex.
        /// </summary>
        /// <param name="index">The vertex to examine.</param>
        /// <returns>The inbound edges of the specified vertex.</returns>
        ReadOnlySpan<EdgeIdx> GetInEdges(VertexIdx index);
    }
}
