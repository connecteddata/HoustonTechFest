using ConnectedData.Messaging;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{

    public abstract class GraphNotificationHandler<TMessage> : NotificationHandler<TMessage>
        where TMessage : IMessage
    {

        protected readonly IGraphClient _graphClient;

        public GraphNotificationHandler(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        protected abstract ICypherFluentQuery CypherQuery(TMessage message);

        protected override void HandleMessage(TMessage message)
        {
            ExecuteWithoutResults(cypherQuery: CypherQuery(message));
        }


        protected virtual void ExecuteWithoutResults(ICypherFluentQuery cypherQuery)
        {
            Console.WriteLine(cypherQuery.Query.DebugQueryText);
            cypherQuery.ExecuteWithoutResults();
        }
    }
}
