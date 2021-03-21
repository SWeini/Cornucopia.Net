using System;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     An index that identifies a vertex of a graph.
    /// </summary>
    public readonly struct VertexIdx : IEquatable<VertexIdx>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VertexIdx"/> structure with the specified index.
        /// </summary>
        /// <param name="index">The index of the vertex.</param>
        public VertexIdx(int index)
        {
            this.Index = index;
        }

        /// <summary>
        ///     Gets the index of the vertex.
        /// </summary>
        /// <value>The index of the vertex.</value>
        public int Index { get; }

        /// <inheritdoc/>
        public bool Equals(VertexIdx other)
        {
            return this.Index == other.Index;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is VertexIdx other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Index;
        }

        /// <summary>
        ///     Determines whether two specified <see cref="VertexIdx"/> instances represent the same vertex.
        /// </summary>
        /// <param name="left">The first vertex to compare.</param>
        /// <param name="right">The second vertex to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(VertexIdx left, VertexIdx right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified <see cref="VertexIdx"/> instances represent different vertices.
        /// </summary>
        /// <param name="left">The first vertex to compare.</param>
        /// <param name="right">The second vertex to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(VertexIdx left, VertexIdx right)
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
