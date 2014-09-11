using ConnectedData.Domain;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public class UpdateDomainObjectHandler<TDomainObject> :
        DomainObjectCommandHandler<UpdateDomainObject<TDomainObject>, TDomainObject>, ICommandHandler<ICommand>
        where TDomainObject: DomainObject
    {
        public UpdateDomainObjectHandler(IGraphClient client) : base(client) { }
    
        protected override ICypherFluentQuery Query(TDomainObject t)
        {
            return _graphClient
                .Cypher
                .Match(string.Format("({0})", t.EntityPlusLabelCypher()))
                .Where(t.WhereIdClause())
                .WithParam("Id", t.Guid.ToString())
                .Set(string.Format("{0} = {{{1}}}", t.EntityString().ToLower(), t.UpdateEntityString()))
                .WithParam(t.UpdateEntityString(), t);
        }
    }

    public class DeleteDomainObjectHandler<TDomainObject> :
        DomainObjectCommandHandler<DeleteDomainObject<TDomainObject>, TDomainObject>, ICommandHandler<ICommand>
        where TDomainObject: DomainObject
    {
        public DeleteDomainObjectHandler(IGraphClient client) : base(client) { }

        protected override ICypherFluentQuery Query(TDomainObject t)
        {
            return _graphClient
                    .Cypher
                    .Match(string.Format("({0})", t.EntityPlusLabelCypher()))
                    .Where(t.WhereIdClause())
                    .WithParam("Id", t.Guid.ToString())
                    .Delete(t.EntityString().ToLower());
        }
    }
}
