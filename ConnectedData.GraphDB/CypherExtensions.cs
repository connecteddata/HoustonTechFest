using ConnectedData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public static class CypherExtensions
    {

        public static string ToCypherRelationshipConvention(this string relationshipName)
        {
            return Regex.Replace(relationshipName, "(\\B[A-Z])", "_$1").ToUpper();
        }


        public static string EntityString(this DomainObject domainObject)
        {
            return domainObject.GetType().Name;
        }

        public static string CreateString(this DomainObject domainObject)
        {
            return string.Format("({0} {{{1}}})", domainObject.EntityPlusLabelCypher(), domainObject.NewEntityString());
        }

        public static string CreateString(this IEnumerable<DomainObject> domainObjects)
        {
            if (null == domainObjects) throw new ArgumentNullException("domainObjects");
            if (0 == domainObjects.Count()) throw new ArgumentException("Cannot build a cypher create string for an empty collection");
            return string.Format("({0} {{{1}}})", domainObjects.First().EntityPlusLabelCypher(), domainObjects.PuralizedEntityString());
        }

        public static string PuralizedEntityString(this IEnumerable<DomainObject> domainObjects)
        {
            if (null == domainObjects) throw new ArgumentNullException("domainObjects");
            if (0 == domainObjects.Count()) throw new ArgumentException("Cannot build a cypher create string for an empty collection");
            var @object = domainObjects.First();
            if (@object is Person1) return "people";
            if (@object is Concept) return "concepts";
            return "domainObjects";
        }

        public static string EntityPlusLabelCypher(this DomainObject domainObject)
        {
            return string.Format("{0}:{1}", domainObject.EntityString().ToLower(), domainObject.EntityString());
        }

        public static string NewEntityString(this DomainObject domainObject)
        {
            return string.Format("new{0}", domainObject.EntityString());
        }

        public static string UpdateEntityString(this DomainObject domainObject)
        {
            return string.Format("update{0}", domainObject.EntityString());
        }


        public static string WhereIdClause(this DomainObject domainObject)
        {
            return string.Format("{0}.Id = {{Id}}", domainObject.EntityString().ToLower());
        }

        public static string DirectionRelationshipString<TLeft,TRight>(this DomainRelationship<TLeft, TRight> relationship)
            where TRight : DomainObject
            where TLeft : DomainObject
        {
            return relationship.DirectionRelationshipString<TLeft, TRight>(String.Empty);
        }

        public static string DirectionRelationshipString<TLeft, TRight>(this DomainRelationship<TLeft, TRight> relationship, string identity)
            where TRight : DomainObject
            where TLeft : DomainObject
        {
            var cypherConventionName = relationship.Name.ToCypherRelationshipConvention();
            switch (relationship.Direction)
            {
                case Domain.RelationshipDirection.LeftToRight:
                    return string.Format("-[{0}:{1}]->", identity, cypherConventionName);
                case Domain.RelationshipDirection.RightToLeft:
                    return string.Format("<-[{0}:{1}]-", identity, cypherConventionName);
                case Domain.RelationshipDirection.Undirected:
                    return string.Format("-[{0}:{1}]-", identity, cypherConventionName);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
