using System;
using System.Diagnostics;

namespace Cornucopia.DataStructures.Graph
{
    internal struct DirectedOutInEdgeStorage
    {
        public int OutDegree { get; private set; }
        public int InDegree { get; private set; }

        public void AddOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            if (start < 0)
            {
                start = links.Allocate(1);
                this.OutDegree = 1;
                links[start] = edge;
                return;
            }

            var oldCapacity = this.OutDegree + this.InDegree;
            start = links.Resize(start, oldCapacity, oldCapacity + 1);
            if (this.InDegree > 0)
            {
                links[start + oldCapacity] = links[start + this.OutDegree];
                links[start + this.OutDegree] = edge;
            }
            else
            {
                links[start + oldCapacity] = edge;
            }
            this.OutDegree++;
        }

        public void AddInEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            if (start < 0)
            {
                start = links.Allocate(1);
                this.InDegree = 1;
                links[start] = edge;
                return;
            }

            var oldCapacity = this.OutDegree + this.InDegree;
            start = links.Resize(start, oldCapacity, oldCapacity + 1);
            links[start + oldCapacity] = edge;
            this.InDegree++;
        }

        public void RemoveOutEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            var oldCapacity = this.OutDegree + this.InDegree;
            var span = links.AsSpan(start, oldCapacity);
            var i = span.IndexOf(edge);
            Debug.Assert(i < this.OutDegree);

            if (this.InDegree > 0)
            {
                span[i] = span[this.OutDegree];
                span[this.OutDegree] = span[span.Length - 1];
            }
            else
            {
                span[i] = span[span.Length - 1];
            }

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

        public void RemoveInEdge(ref DynamicArrayBlockAllocator<EdgeIdx> links, ref int start, EdgeIdx edge)
        {
            var oldCapacity = this.OutDegree + this.InDegree;
            var span = links.AsSpan(start + this.OutDegree, this.InDegree);
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

            this.InDegree--;
        }
    }
}
