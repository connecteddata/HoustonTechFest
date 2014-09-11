using ConnectedData.Domain;
using ConnectedData.Messaging;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedData.Utils;
using ConnectedData.GraphDB.Notifications;

namespace ConnectedData.GraphDB
{

    public class ObtainedConnectionsNotificationHandler : GraphNotificationHandler<ObtainedUserConnectionsNotification>
    {
        public ObtainedConnectionsNotificationHandler(IGraphClient graphClient) : base(graphClient) { }

        protected override void HandleMessage(ObtainedUserConnectionsNotification message)
        {
            //get person who has the connections
            var person = GetPersonFromUserId(message.UserId);

            if (null == person) throw new ArgumentException(string.Format("Could not find the Person with id '{0}' in the database.  Cannot associate connections", message.UserId));

            MergePeople(message.Connections.Select(c => new Person() { Id = c.Id }));

            MergeIndustries(message.Connections.Select(c => new Industry() { Name = c.Industry }).Distinct());

            MergeLocations(message.Connections.Select(c => new Location() { Name = c.Location }).Distinct());

            MergeCountries(message.Connections.Select(c => new Country() { CountryCode = c.CountryCode }).Distinct());

            AssociatePeopleWithTheirIndustries(
                message.Connections.Select(c => new Pair<string, string>() { First = c.Id, Second = c.Industry })
                .Where(p => !string.IsNullOrEmpty(p.First) && !string.IsNullOrEmpty(p.Second))
            );

            AssociatePeopleWithTheirLocations(
                message.Connections.Select(c => new Pair<string, string>() { First = c.Id, Second = c.Location })
                .Where(p => !string.IsNullOrEmpty(p.First) && !string.IsNullOrEmpty(p.Second))
            );

            AssociateLocationsWithTheirCountry(
                message.Connections.Select(c => new Pair<string, string>() { First = c.Location, Second = c.CountryCode })
                .Where(p => !string.IsNullOrEmpty(p.First) && !string.IsNullOrEmpty(p.Second))
            );

            AssociatePersonWithConnections(
                message.UserId,
                message.Connections.Select(c => c.Id).Distinct()
                );
        }


        private void AssociatePersonWithConnections(string userId, IEnumerable<string> connectionUserIds)
        {
            _graphClient.Cypher
                .Match("(user:Person)", "(connection:Person)")
                .Where((Person user) => user.Id == userId)
                .AndWhere("connection.Id IN {connectionIds}")
                .WithParam("connectionIds", connectionUserIds)
                .CreateUnique("(user)-[:LINKED_IN_CONNECTED]-(connection)")
                .ExecuteWithoutResults();
        }

        

        

        


        

    }
}
