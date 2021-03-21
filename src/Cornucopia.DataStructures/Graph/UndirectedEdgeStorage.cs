using System;

namespace Cornucopia.DataStructures.Graph
{
    internal struct UndirectedEdgeStorage
    {
        public int Degree { get; private set; }

        public static bool IsReverse(EdgeIdx edge)
        {
            return edge.Index < 0;
        }

        public static EdgeIdx Reverse(EdgeIdx edge)
        {
            return new(~edge.Index);
        }

        public void AddOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            if (start < 0)
            {
                start = links.Allocate(1);
                this.Degree = 1;
                links[start] = edge;
                return;
            }

            var oldCapacity = this.Degree;
            start = links.Resize(start, oldCapacity, oldCapacity + 1);
            links[start + oldCapacity] = edge;
            this.Degree++;
        }

        public void AddInEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            this.AddOutEdge(ref links, ref start, Reverse(edge));
        }

        public void RemoveOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            var oldCapacity = this.Degree;
            var span = links.AsSpan(start, this.Degree);
            var i = span.IndexOf(edge);
            span[i] = span[span.Length - 1];
            if (oldCapacity == 1)
            {
                links.Deallocate(start, 1);
                start = -1;
            }
            else
            {
                start = links.Resize(start, oldCapacity, oldCapacity - 1);
            }

            this.Degree--;
        }

        public void RemoveInEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            this.RemoveOutEdge(ref links, ref start, Reverse(edge));
        }
    }
}
