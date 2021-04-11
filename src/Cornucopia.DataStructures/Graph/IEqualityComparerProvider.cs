using System.Collections.Generic;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     Defines a method to provide an instance of the <see cref="IEqualityComparer{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of elements to compare.</typeparam>
    public interface IEqualityComparerProvider<T>
    {
        /// <summary>
        ///     Gets an equality comparer for the elements of the requested type.
        /// </summary>
        /// <value>An equality comparer for the elements of the requested type, or <c>null</c> if the default comparer is to be used.</value>
        IEqualityComparer<T>? Comparer { get; }
    }
}