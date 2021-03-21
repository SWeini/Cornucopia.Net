using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Defines methods for basic calculations on distances.
    /// </summary>
    /// <typeparam name="TDistance">The type that is used to represent a distance.</typeparam>
    public interface IDistanceCalculator<TDistance> : IComparer<TDistance>
    {
        /// <summary>
        ///     Gets the distance that is zero.
        /// </summary>
        /// <value>The distance zero.</value>
        TDistance Zero { get; }

        /// <summary>
        ///     Adds two distances.
        /// </summary>
        /// <param name="a">The first distance.</param>
        /// <param name="b">The second distance.</param>
        /// <returns>The sum of the two distances.</returns>
        TDistance Add(TDistance a, TDistance b);
    }
}