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
    
    public class ObtainedConnectionsNotificationHandler : GraphNotificationHandler<ObtainedUserConnectionsNotification>
    {
        public ObtainedConnectionsNotificationHandler(IGraphClient graphClient) : base(graphClient) { }
        protected override ICypherFluentQuery CypherQuery(ObtainedUserConnectionsNotification message)
        {
            message.Connections;
            //look at current persons in DB, filter any in list that are "detailed"
            //persist others
            throw new NotImplementedException();
        }

        
    }
}
