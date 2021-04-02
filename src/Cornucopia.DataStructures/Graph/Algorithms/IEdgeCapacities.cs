namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Defines a method to get the capacity of tagged edges.
    /// </summary>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    /// <typeparam name="TCapacity">The type that is used to represent a capacity.</typeparam>
    public interface IEdgeCapacities<TEdge, TCapacity>
    {
        /// <summary>
        ///     Gets the capacity of the specified edge tag.
        /// </summary>
        /// <param name="edge">The edge tag to get the capacity of.</param>
        /// <returns>The capacity of the edge tag.</returns>
        TCapacity GetCapacity(TEdge edge);
    }
}