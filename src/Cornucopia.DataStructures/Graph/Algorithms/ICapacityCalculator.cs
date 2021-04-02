using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph.Algorithms
{
    /// <summary>
    ///     Defines methods for basic calculations on capacities.
    /// </summary>
    /// <typeparam name="TCapacity">The type that is used to represent a capacity.</typeparam>
    public interface ICapacityCalculator<TCapacity> : IComparer<TCapacity>
    {
        /// <summary>
        ///     Gets the capacity that is zero.
        /// </summary>
        /// <value>The capacity zero.</value>
        TCapacity Zero { get; }

        /// <summary>
        ///     Adds two capacities.
        /// </summary>
        /// <param name="a">The first capacity.</param>
        /// <param name="b">The second capacity.</param>
        /// <returns>The sum of the two capacities.</returns>
        TCapacity Add(TCapacity a, TCapacity b);

        /// <summary>
        ///     Negates a capacity.
        /// </summary>
        /// <param name="a">The capacity to negate.</param>
        /// <returns>The negated capacity.</returns>
        TCapacity Negate(TCapacity a);
    }
}