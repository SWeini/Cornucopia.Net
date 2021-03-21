using System.Diagnostics;

namespace Cornucopia.DataStructures.Graph
{
    [DebuggerDisplay("null")]
    public struct Empty
    {
        public static Empty Default => default;
    }
}
