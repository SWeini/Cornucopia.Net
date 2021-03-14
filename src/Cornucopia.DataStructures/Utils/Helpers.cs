namespace Cornucopia.DataStructures.Utils
{
    using System.Runtime.CompilerServices;

    internal static class Helpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MayBeReferenceOrContainReferences<T>()
        {
#if NETCOREAPP3_1
            return RuntimeHelpers.IsReferenceOrContainsReferences<T>();
#else
            return true;
#endif
        }
    }
}
