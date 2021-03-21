using System;

namespace Cornucopia.DataStructures.Graph
{
    internal struct DirectedOutEdgeStorage
    {
        public int OutDegree { get; private set; }

        public void AddOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            if (start < 0)
            {
                start = links.Allocate(1);
                this.OutDegree = 1;
                links[start] = edge;
                return;
            }

            var oldCapacity = this.OutDegree;
            start = links.Resize(start, oldCapacity, oldCapacity + 1);
            links[start + oldCapacity] = edge;
            this.OutDegree++;
        }

        public void RemoveOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            var oldCapacity = this.OutDegree;
            var span = links.AsSpan(start, this.OutDegree);
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

            this.OutDegree--;
        }
    }
}
