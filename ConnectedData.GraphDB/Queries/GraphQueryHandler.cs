using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedData.Messaging;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace ConnectedData.GraphDB
{
    public abstract class GraphQueryHandler<TQuery,TResponseData> : QueryHandler<TQuery,IEnumerable<TResponseData>>
        where TQuery : Query<IEnumerable<TResponseData>>
    {
        protected readonly IGraphClient _graphClient;

        public GraphQueryHandler(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        protected abstract ICypherFluentQuery<TResponseData> CypherQuery(TQuery query);

        protected virtual IEnumerable<TResponseData> Execute(ICypherFluentQuery<TResponseData> cypherQuery)
        {
            return cypherQuery.Results;
        }

        protected override IEnumerable<TResponseData> HandleQuery(TQuery query)
        {
            var cypher = CypherQuery(query);
            return Execute(cypher);
        }
    }
}
