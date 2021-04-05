using System;
using System.Collections.Generic;

using Cornucopia.DataStructures.Utils;

namespace Cornucopia.DataStructures.Graph
{
    /// <summary>
    ///     A directed graph representing dependencies, that is evaluated on demand.
    /// </summary>
    /// <typeparam name="TVertex">The type of elements represented by vertices.</typeparam>
    public class DependencyGraph<TVertex> : IImplicitOutEdgesIndices<TVertex, DependencyGraph<TVertex>.Edge>, IEqualityComparerProvider<TVertex>
        where TVertex : notnull
    {
        private readonly Func<TVertex, TVertex[]> _dependencyCalculator;
        private readonly IEqualityComparer<TVertex>? _comparer;
        private readonly Dictionary<TVertex, Edge[]> _dependencies;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DependencyGraph{TVertex}"/> class.
        /// </summary>
        /// <param name="dependencyCalculator">The function that calculates dependencies of an element.</param>
        /// <param name="comparer">The comparer used to compare elements.</param>
        public DependencyGraph(Func<TVertex, TVertex[]> dependencyCalculator, IEqualityComparer<TVertex>? comparer = null)
        {
            this._dependencyCalculator = dependencyCalculator;
            this._comparer = comparer;
            this._dependencies = new(comparer);
        }

        private ReadOnlySpan<Edge> EvaluateDependencies(TVertex vertex)
        {
            if (this._dependencies.TryGetValue(vertex, out var result))
            {
                return result;
            }

            var dependencies = this._dependencyCalculator(vertex);
            result = ArrayTools.ConvertAll(dependencies, x => new Edge(x));
            this._dependencies.Add(vertex, result);
            return result;
        }

        /// <inheritdoc/>
        public int GetOutDegree(TVertex vertex)
        {
            return this.EvaluateDependencies(vertex).Length;
        }

        /// <inheritdoc/>
        public ReadOnlySpan<Edge> GetOutEdges(TVertex vertex)
        {
            return this.EvaluateDependencies(vertex);
        }

        /// <inheritdoc/>
        public TVertex GetTarget(Edge edge)
        {
            return edge.Target;
        }

        IEqualityComparer<TVertex>? IEqualityComparerProvider<TVertex>.Comparer => this._comparer;

        /// <summary>
        ///     An edge in the dependency graph.
        /// </summary>
        public readonly struct Edge
        {
            internal Edge(TVertex target)
            {
                this.Target = target;
            }

            internal TVertex Target { get; }
        }
    }
}