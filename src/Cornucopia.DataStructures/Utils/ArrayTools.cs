using System;

namespace Cornucopia.DataStructures.Utils
{
    internal static class ArrayTools
    {
        public static T[] Empty<T>()
        {
#if NETCOREAPP3_1
            return Array.Empty<T>();
#else
            return EmptyArray<T>.Instance;
#endif
        }

#if NETCOREAPP3_1
        public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter) => Array.ConvertAll(array, converter);
#else
        public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Func<TInput, TOutput> converter)
        {
            var result = new TOutput[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                result[i] = converter(array[i]);
            }

            return result;
        }
#endif

#if !NETCOREAPP3_1
        private static class EmptyArray<T>
        {
            public static readonly T[] Instance = new T[0];
        }
#endif
    }
}