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
    public class CreateDomainObjectsHandler<TDomainObjects> :
        DomainObjectsCommandHandler<CreateDomainObjects<TDomainObjects>,TDomainObjects>, ICommandHandler<ICommand>
        where TDomainObjects : IEnumerable<DomainObject>
    {

        public CreateDomainObjectsHandler(IGraphClient client) : base(client) { }

        protected override ICypherFluentQuery Query(TDomainObjects domainObjects)
        {
            return
                _graphClient
                .Cypher
                .Create(domainObjects.CreateString())
               .WithParam(domainObjects.PuralizedEntityString(), domainObjects);
        }
    }
}
