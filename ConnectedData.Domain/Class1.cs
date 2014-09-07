using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class DomainObject : IEquatable<DomainObject> { 
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as DomainObject);
        }

        public bool Equals(DomainObject other)
        {
            if (null == other) return false;
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (null == this.Id) return base.GetHashCode();
            return this.Id.GetHashCode();
        }
    }

    public class Person : DomainObject
    {
        public string Name { get; set; }
    }

    public class Concept : DomainObject
    {
        public string Name { get; set; }
    }

    public enum RelationshipDirection
    {
        LeftToRight,
        RightToLeft,
        Undirected
    }




}
