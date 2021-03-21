namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Defines a method to get the distance of tagged edges.
    /// </summary>
    /// <typeparam name="TEdge">The type of data tagged to an edge.</typeparam>
    /// <typeparam name="TDistance">The type that is used to represent a distance.</typeparam>
    public interface IEdgeDistances<TEdge, TDistance>
    {
        /// <summary>
        ///     Gets the distance of the specified edge tag.
        /// </summary>
        /// <param name="edge">The edge tag to get the distance of.</param>
        /// <returns>The distance of the edge tag.</returns>
        TDistance GetDistance(TEdge edge);
    }
}