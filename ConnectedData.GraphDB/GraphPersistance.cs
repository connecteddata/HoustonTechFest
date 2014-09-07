using ConnectedData.Domain;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public class GraphPersistance : PersistanceBase
    {
        protected readonly IGraphClient _graphClient;

        public GraphPersistance(IGraphClient graphClient, IEnumerable<ICommandHandler<ICommand>> commandHandlers)
            : base(commandHandlers)
        {
            _graphClient = graphClient;
        }
    }


}
