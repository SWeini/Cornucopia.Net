using System;
using System.Diagnostics.CodeAnalysis;

namespace Cornucopia.DataStructures.Persistent
{
    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowInvalidOperationException()
        {
            throw new InvalidOperationException();
        }
    }
}