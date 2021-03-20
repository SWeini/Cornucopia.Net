using System.Runtime.CompilerServices;

namespace Cornucopia.DataStructures.Utils
{
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
