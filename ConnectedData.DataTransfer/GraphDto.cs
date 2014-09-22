using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.DataTransfer
{
    public class GraphDto
    {
        public GraphDto()
        {
            Edges = new List<EdgeDto>();
            Vertices = new List<VertexDto>();
        }
        public IEnumerable<EdgeDto> Edges { get; set; }
        public IEnumerable<VertexDto> Vertices { get; set; }
    }

    public class EdgeDto
    {
        public int SourceVertexId { get; set; }
        public int TargetVertexId { get; set; }
        public string Label { get; set; }
        public int Weight { get; set; }
    }

    public class EdgeDtoEqualityComparer : IEqualityComparer<EdgeDto>
    {

        public bool Equals(EdgeDto x, EdgeDto y)
        {
            if (null == x) return false;
            if (null == y) return false;
            if (null != x && null != y)
                return x.SourceVertexId == y.SourceVertexId && x.TargetVertexId == y.TargetVertexId;
            else
                return false;
        }

        public int GetHashCode(EdgeDto obj)
        {
            return base.GetHashCode();
        }
    }

    public class VertexDto
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Label { get; set; }
        public string Group { get; set; }
    }

    public class VertexDtoEqualityComparer : IEqualityComparer<VertexDto>
    {

        public bool Equals(VertexDto x, VertexDto y)
        {
            return x.GetHashCode().Equals(y.GetHashCode());            
        }

        public int GetHashCode(VertexDto obj)
        {
            if (null == obj) return base.GetHashCode();
            return obj.Id.GetHashCode();
        }
    }
}
