using System;
using System.Diagnostics.CodeAnalysis;

namespace Cornucopia.DataStructures
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