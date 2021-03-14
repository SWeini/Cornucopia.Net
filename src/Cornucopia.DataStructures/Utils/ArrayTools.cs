namespace Cornucopia.DataStructures.Utils
{
    internal static class ArrayTools
    {
        public static T[] Empty<T>()
        {
#if NETCOREAPP3_1
            return System.Array.Empty<T>();
#else
            return EmptyArray<T>.Instance;
#endif
        }

#if !NETCOREAPP3_1
        private static class EmptyArray<T>
        {
            public static readonly T[] Instance = new T[0];
        }
#endif
    }
}