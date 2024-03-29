﻿using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines methods to modify a graph.
    /// </summary>
    /// <typeparam name="TVertexId">The type used to identify vertices.</typeparam>
    /// <typeparam name="TVertex">The type of data tagged to a vertex.</typeparam>
    /// <typeparam name="TEdgeId">The type used to identify edges.</typeparam>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    public interface IMutableGraph<TVertexId, TVertex, TEdgeId, TEdge>
        where TVertexId : notnull
        where TEdgeId : notnull
    {
        /// <summary>
        ///     Adds a vertex to this graph.
        /// </summary>
        /// <param name="vertex">The tagged data to store in the created vertex.</param>
        /// <returns>The index of the created vertex.</returns>
        TVertexId AddVertex(TVertex vertex);

        /// <summary>
        ///     Removes a vertex from this graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        /// <exception cref="ArgumentException">The vertex has stored edges.</exception>
        /// <remarks>Removing invalid vertices will corrupt the graph.</remarks>
        void RemoveVertex(TVertexId vertex);

        /// <summary>
        ///     Adds an edge to this graph.
        /// </summary>
        /// <param name="source">The source vertex of the created vertex.</param>
        /// <param name="target">The target vertex of the created vertex.</param>
        /// <param name="edge">The tagged data to store in the created edge.</param>
        /// <returns>The index of the created edge.</returns>
        TEdgeId AddEdge(TVertexId source, TVertexId target, TEdge edge);

        /// <summary>
        ///     Removes an edge from this graph.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        /// <remarks>Removing invalid edges will corrupt the graph.</remarks>
        void RemoveEdge(TEdgeId edge);
    }
}
