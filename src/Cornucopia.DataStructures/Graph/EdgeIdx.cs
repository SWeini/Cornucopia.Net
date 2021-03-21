using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     An index that identifies an edge of a graph.
    /// </summary>
    public readonly struct EdgeIdx : IEquatable<EdgeIdx>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgeIdx"/> structure with the specified index.
        /// </summary>
        /// <param name="index">The index of the edge.</param>
        public EdgeIdx(int index)
        {
            this.Index = index;
        }

        /// <summary>
        ///     Gets the index of the edge.
        /// </summary>
        /// <value>The index of the edge.</value>
        public int Index { get; }

        /// <inheritdoc/>
        public bool Equals(EdgeIdx other)
        {
            return this.Index == other.Index;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is EdgeIdx other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Index;
        }

        /// <summary>
        ///     Determines whether two specified <see cref="EdgeIdx"/> instances represent the same edge.
        /// </summary>
        /// <param name="left">The first edge to compare.</param>
        /// <param name="right">The second edge to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(EdgeIdx left, EdgeIdx right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified <see cref="EdgeIdx"/> instances represent different edges.
        /// </summary>
        /// <param name="left">The first edge to compare.</param>
        /// <param name="right">The second edge to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(EdgeIdx left, EdgeIdx right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"#{this.Index}";
        }
    }
}
