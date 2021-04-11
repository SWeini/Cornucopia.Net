using System.Collections.Generic;

using Cornucopia.DataStructures.Graph;

namespace Cornucopia.DataStructures.Utils
{
    internal static class EqualityComparerTools
    {
        public static bool Equals<TProvider, T>(this TProvider provider, T x, T y)
            where TProvider : IEqualityComparerProvider<T>
        {
            var comparer = provider.Comparer ?? EqualityComparer<T>.Default;
            return comparer.Equals(x, y);
        }

        public static IEqualityComparer<T>? GetComparer<TProvider, T>(this TProvider provider)
            where TProvider : IEqualityComparerProvider<T>
        {
            return provider.Comparer;
        }
    }
}