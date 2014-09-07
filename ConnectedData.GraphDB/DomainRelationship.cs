using ConnectedData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public class DomainRelationship<TLeftSideDomainObject, TRightSideDomainObject>
        where TRightSideDomainObject : DomainObject
        where TLeftSideDomainObject : DomainObject
    {
        protected RelationshipDirection _direction;
        public RelationshipDirection Direction
        {
            get
            { return _direction; }
        }

        protected TLeftSideDomainObject _leftside;
        protected TRightSideDomainObject _rightside;
        protected string _name;



        public TLeftSideDomainObject LeftSideDomainObject { get { return _leftside; } }
        public TRightSideDomainObject RightSideDomainObject { get { return _rightside; } }

        public string Name { get { return _name; } }

        public DomainRelationship(TLeftSideDomainObject left, TRightSideDomainObject right, RelationshipDirection direction, string name)
        {
            _leftside = left;
            _rightside = right;
            _direction = direction;
            _name = name;
        }
    }
}
