using ConnectedData.DataTransfer;
using ConnectedData.Domain;
using ConnectedData.Messaging;
using Neo4jClient;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB.Tests
{
    [TestFixture]
    public class ObtainedConnectionNotificationHandlerTests
    {
        private IGraphClient _graphClient;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var localUrl = "http://localhost:7474/db/data";
            var connectedGraphClient = new GraphClient(new Uri(localUrl));
            connectedGraphClient.Connect();

            _graphClient = connectedGraphClient;

            
        }

        [Test]
        public void Can_Persist_New_Connections()
        {
            
            //Arrange
            //create temp person
            _graphClient
                .Cypher
                .Create("(person:Person {person})")
                .WithParam("person", new Person() { Id = "randomUserId", FirstName = "kevin" })
                .ExecuteWithoutResults();

            var handler = new ObtainedConnectionsNotificationHandler(_graphClient);
            var peopleDtos = new List<PersonDto>();
            peopleDtos.Add(new PersonDto() { Id = "foo", CountryCode = "us", Industry = "late night coding", Location = "houston"});
            
            
            
            var message = new ObtainedUserConnectionsNotification("randomUserId",peopleDtos);
            //Act
            Assert.DoesNotThrow(() => handler.Handle(message));
            
            
            //Assert
        }

        [Test]
        
        public void ClearDBTest()
        {
            ClearDB();
        }

        [Test]
        public void Can_Handle_Profile()
        {
            //Arrange
            var profileJsonFileLocation = ConfigurationManager.AppSettings["DummyProfileFileJson"];
            var profileFileContents = File.ReadAllText(profileJsonFileLocation);
            var detailedPersonDto =
                JsonConvert.DeserializeObject<DetailedPersonDto>(profileFileContents);

            var handler = new Notifications.ObtainedProfileNotificationHandler(_graphClient);
            var message = new Messaging.Notifications.ObtainedUserProfileNotification(detailedPersonDto.Id, detailedPersonDto);

            //Act
            var start = DateTime.Now.Ticks;
            Assert.DoesNotThrow(() => handler.Handle(message));
            var end = DateTime.Now.Ticks;
            Console.WriteLine("Before Delete Took '{0}' seconds", (new TimeSpan(end - start)).Seconds);
        }

        [Test]
        public void Can_Handle_Bulk_Connections()
        {
            //Arrange
            var profileJsonFileLocation = ConfigurationManager.AppSettings["DummyProfileFileJson"];
            var profileFileContents = File.ReadAllText(profileJsonFileLocation);
            var detailedPersonDto =
                JsonConvert.DeserializeObject<DetailedPersonDto>(profileFileContents);

            var person = new Person()
            {
                Id = detailedPersonDto.Id,
                FirstName = detailedPersonDto.FirstName,
                LastName = detailedPersonDto.LastName,
                Headline = detailedPersonDto.Headline,
                Industry = new Industry() { Name = detailedPersonDto.Industry },
                Location = new Location() { Name = detailedPersonDto.Location, Country = new Country() { CountryCode = detailedPersonDto.CountryCode } },

            };

            _graphClient
                .Cypher
                .Create("(person:Person {newPerson})")
                .WithParam("newPerson", new { Id = person.Id, FirstName = person.FirstName, LastName = person.LastName, HeadLine = person.Headline })
                .ExecuteWithoutResults();


            var connectionsFileLocation = ConfigurationManager.AppSettings["DummyConnectionsFileJson"];
            var connectionsFileContents = File.ReadAllText(connectionsFileLocation);
            var connections = JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(connectionsFileContents);


            var handler = new ObtainedConnectionsNotificationHandler(_graphClient);
            var message = new ObtainedUserConnectionsNotification(person.Id, connections);
            //Act
            var start = DateTime.Now.Ticks;
            Assert.DoesNotThrow(() => handler.Handle(message));
            var end = DateTime.Now.Ticks;
            Console.WriteLine("Before Delete Took '{0}' seconds", (new TimeSpan(end - start)).Seconds);
        }

        [TearDown]
        public void TearDown()
        {
            ClearDB();
        }

        public void ClearDB()
        {
            ClearDBOfRelationships();
            ClearDBOfNodes();
            
        }

        private void ClearDBOfRelationships()
        {
        
            _graphClient
                .Cypher
                .Match("(a)-[r]-(b)")
                .Delete("r")
                .ExecuteWithoutResults();
        
        }

        private void ClearDBOfNodes()
        {
            _graphClient
                .Cypher
                .Match("(a)")
                .Delete("a")
                .ExecuteWithoutResults();
        }
    }
}
