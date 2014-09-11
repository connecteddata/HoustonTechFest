using ConnectedData.DataTransfer;
using ConnectedData.Domain;
using ConnectedData.Messaging.Queries;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB.Queries
{
    public class ObtainUserSummaryHandler : GraphQueryHandler<ObtainUserSummary,UserSummaryDto>
    {
        public ObtainUserSummaryHandler(IGraphClient graphClient) : base(graphClient) { }
        protected override ICypherFluentQuery<UserSummaryDto> CypherQuery(ObtainUserSummary query)
        {
            throw new NotImplementedException("Going a different route for this handler");
        }

        protected override IEnumerable<UserSummaryDto> HandleQuery(ObtainUserSummary query)
        {
            return
                _graphClient
                .Cypher
                .OptionalMatch("(person:Person)-[:LINKED_IN_CONNECTED]-(connection:Person)-[:WORKS_IN]->(industry:Industry)")
                .Where("(person.Id = {personId})")
                .WithParam("personId", query.UserId)
                .Return((person, connection, industry) => new {
                    ConnectionNodes = connection.CollectAs<Person>(),
                    IndustryNodes = industry.CollectAsDistinct<Industry>()
                })
                .Results
                .Select(_ => new UserSummaryDto() {
                    NumberOfConnections = _.ConnectionNodes.Count(),
                    NumberOfIndustries = _.IndustryNodes.Count()
                });
            
        }
    }
}
