using ConnectedData.Domain;
using ConnectedData.GraphDB;
using Moq;
using Neo4jClient;
using Neo4jClient.Cypher;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectedData.Tests
{
    [TestFixture]
    public class Neo4jPersistanceTests
    {
        private IGraphClient _graphClient;
        private IPersistance _persistance;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var localUrl = "http://localhost:7474/db/data";
            var connectedGraphClient = new GraphClient(new Uri(localUrl));
            connectedGraphClient.Connect();

            _graphClient = connectedGraphClient;

            _persistance =
                new GraphPersistance(
                    _graphClient,
                    new List<ICommandHandler<ICommand>>()
                    {
                        new CreateDomainObjectsHandler<IEnumerable<Person>>(_graphClient),
                        new CreateDomainObjectsHandler<IEnumerable<Concept>>(_graphClient),
                        new UpdateDomainObjectHandler<Person>(_graphClient),
                        new UpdateDomainObjectHandler<Concept>(_graphClient),
                        new DeleteDomainObjectHandler<Concept>(_graphClient),
                        new DeleteDomainObjectHandler<Person>(_graphClient),
                        new AssociateSkillWithPersonCommandHandler(_graphClient),
                        new DisaassociateSkillWithPersonCommandHandler(_graphClient)
                        
                    }
                );

            ClearDB();
        }

        [Test]
        public void LocalNeo4jConnectionTest()
        {

            var localUrl = "http://localhost:7474/db/data";
            var neo4jClient = new Neo4jClient.GraphClient(new Uri(localUrl));
            Assert.DoesNotThrow(() => neo4jClient.Connect());
        }

        [Test]
        public void GrapheneDBRemoteNeo4jConnectionTest()
        {
            
            var GRAPHENEDB_URL = "http://app28796168:KdzDgaRdrnXTRkYf1eRk@app28796168.sb02.stations.graphenedb.com:24789/db/data";
            var remoteNeo4jClient = new Neo4jClient.GraphClient(new Uri(GRAPHENEDB_URL));
            Assert.DoesNotThrow(() => remoteNeo4jClient.Connect());   
        }

        [Test]
        public void Can_Create_A_Person_Node_In_Neo4j()
        {
            //Arrange
            ClearDBOfPeople();

            var person = new Person(){ Name = "kevin", Id = Guid.NewGuid() };
            
            _persistance.Create<IEnumerable<Person>>(new List<Person>() { person });

            var peopleInDB = GetPeopleInDB();

            Assert.IsNotNull(peopleInDB.FirstOrDefault(p => p.Id == person.Id));


            ClearDBOfPeople();

            peopleInDB = GetPeopleInDB();

            CollectionAssert.IsEmpty(peopleInDB);
        }

        [Test]
        public void Can_Create_A_Concept_In_Neo4j()
        {
            var concept = new Concept() { Name = "neo4j", Id = Guid.NewGuid() };

            _persistance.Create<IEnumerable<Concept>>(new List<Concept>() { concept });

            var conceptsInDB = GetConceptsInDB();

            Assert.IsNotNull(conceptsInDB.FirstOrDefault(p => p.Id == concept.Id));
        }

        [Test]
        public void Can_Update_A_Concept_In_Neo4j()
        {
            var oldGuid = Guid.NewGuid();
            var oldName = "neo4j";
            var concept = new Concept() { Name = oldName, Id = oldGuid };

            _persistance.Create<IEnumerable<Concept>>(new List<Concept>() { concept });

            var conceptsInDB = GetConceptsInDB();

            Assert.IsNotNull(conceptsInDB.FirstOrDefault(c => c.Id == oldGuid && c.Name == oldName ));

            var updatedName = "updatedNeo4j";
            concept.Name = updatedName;
            _persistance.Update<Concept>(concept);

            conceptsInDB = GetConceptsInDB();

            Assert.IsNotNull(conceptsInDB.FirstOrDefault(c => c.Id == oldGuid && c.Name == updatedName));
            Assert.IsNull(conceptsInDB.FirstOrDefault(c => c.Id == oldGuid && c.Name == oldName));
        }

        private IEnumerable<Concept> GetConceptsInDB()
        {
            return
                _graphClient
                .Cypher
                .Match("(queriedConcept:Concept)")
                .Return(queriedConcept => queriedConcept.As<Concept>())
                .Results;
        }

        [Test]
        public void Can_Create_A_Person_To_Skill_Relationship()
        {
            var testPerson = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "bob"
            };

            var testSkill = new Concept()
            {
                Id = Guid.NewGuid(),
                Name = "late night coding"
            };


            _persistance.Create<IEnumerable<Person>>(new List<Person>() { testPerson});
            _persistance.Create<IEnumerable<Concept>>(new List<Concept>() {testSkill});


            
            _persistance.Handle(
                new AssociateSkillWithPerson(testPerson,testSkill)
            );

            //TO DO need an assertion to get this right.


            var personWithSkills = GetPersonWithSkills(testPerson);

            Assert.AreEqual(testPerson, personWithSkills.Person);
            Assert.AreEqual(1, personWithSkills.Skills.Count());
            Assert.AreEqual(testSkill, personWithSkills.Skills.First());
        }

        

        


        [Test]
        public void Can_Create_An_Enumeration_Of_Entities()
        {
            var people = new List<Person>() { new Person() { Id = Guid.NewGuid(), Name = "kevin" }, new Person() { Id = Guid.NewGuid(), Name = "ginu" } };
            _persistance.Create<IEnumerable<Person>>(people);

            var peopleInDB = GetPeopleInDB();

        }

        private void ClearDBOfPeople()
        {
            var peopleInDB = GetPeopleInDB();

            foreach (var person in peopleInDB)
                _persistance.Delete<Person>(person);
        }

        private void ClearDBOfConcepts()
        {
            var conceptsInDB = GetConceptsInDB();

            foreach (var concept in conceptsInDB)
                _persistance.Delete<Concept>(concept);
        }

        private IEnumerable<Person> GetPeopleInDB()
        {
            return 
                _graphClient
                .Cypher
                .Match("(queriedPerson:Person)")
                .Return(queriedPerson => queriedPerson.As<Person>())
                .Results;
        }

        [Test]
        public void Can_Create_Multiple_Relationships()
        {
            var person = new Person { Name = "kevin", Id = Guid.NewGuid() };

            var skills = new List<Concept>() { new Concept() { Id = Guid.NewGuid(), Name = "C#"}, new Concept() { Id = Guid.NewGuid(), Name = "F#" }};

            _persistance.Create<IEnumerable<Person>>(new List<Person>() {person});
            _persistance.Create<IEnumerable<Concept>>(skills);

            foreach (var skill in skills)
                _persistance.Handle(new AssociateSkillWithPerson(person, skill));

            var personWithSkills = GetPersonWithSkills(person);

            Assert.AreEqual(person, personWithSkills.Person);
            Assert.AreEqual(2, personWithSkills.Skills.Count());
            CollectionAssert.AreEquivalent(skills,personWithSkills.Skills);
        }

        [TearDown]
        public void Teardown()
        {
            ClearDB();
            CollectionAssert.IsEmpty(GetPeopleInDB());
            CollectionAssert.IsEmpty(GetConceptsInDB());
        }

        public void ClearDB()
        {
            ClearDBOfRelationships();
            ClearDBOfPeople();
            ClearDBOfConcepts();
        }

        private void ClearDBOfRelationships()
        {
            _graphClient
                .Cypher
                .Match("(a)-[r]-(b)")
                .Delete("r")
                .ExecuteWithoutResults();
        }

        public PersonWithSkillsDto GetPersonWithSkills(Person personToFind)
        {
            return _graphClient
            .Cypher
            .Match("(foundPerson:Person)-[r:HAS_SKILL]->(foundSkills:Concept)")
            .Where((Person foundPerson) => foundPerson.Id == personToFind.Id)
            .Return((foundPerson, foundSkills) => new {
                Person = foundPerson.As<Person>(),
                Skills = foundSkills.CollectAs<Concept>()
            })
            .Results
            .Select(a => new PersonWithSkillsDto(a.Person, a.Skills.Select(n => n.Data)))
            .FirstOrDefault();
        }   
    }

    public class PersonWithSkillsDto
    {
        public readonly Person Person;
        public readonly IEnumerable<Concept> Skills;

        public PersonWithSkillsDto(Person person, IEnumerable<Concept> skills)
        {
            Person = person;
            Skills = skills;
        }
    }    
}
