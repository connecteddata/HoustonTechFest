using ConnectedData.Domain;
using ConnectedData.Messaging.Notifications;
using ConnectedData.Utils;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB.Notifications
{
    public class ObtainedProfileNotificationHandler : GraphNotificationHandler<ObtainedUserProfileNotification>
    {
        public ObtainedProfileNotificationHandler(IGraphClient client) : base(client) {}
        protected override void HandleProxy(ObtainedUserProfileNotification message)
        {
            var people = new List<Person>()
            {
                new Person(){
                    Id = message.DetailedPersonDto.Id,
                    FirstName = message.DetailedPersonDto.FirstName,
                    LastName = message.DetailedPersonDto.LastName,
                    Headline = message.DetailedPersonDto.Headline
                }
            };
            MergePerson(people.First());

            var skills = message.DetailedPersonDto.Skills;
            MergeSkills(skills);

            var industries = new List<Industry>() { new Industry() { Name = message.DetailedPersonDto.Industry } };
            MergeIndustries(industries);


            var locations = new List<Location>() { new Location() { Name = message.DetailedPersonDto.Location } };
            MergeLocations(locations);

            var countries = new List<Country>() { new Country() { CountryCode = message.DetailedPersonDto.CountryCode } };

            MergeCountries(countries); ;

            var schools = message.DetailedPersonDto.Educations.Select(e => e.SchoolName );
            MergeSchools(schools);

            var companies = message.DetailedPersonDto.Positions.Select(p => p.Company).Distinct(new DataTransfer.CompanyDtoIdComparer());
            MergeCompanies(companies);

            AssociatePeopleWithTheirIndustries(
                new List<Pair<string,string>>() {
                    new Pair<string,string>() {
                        First = message.DetailedPersonDto.Id,
                        Second = message.DetailedPersonDto.Industry
                    }
                }
            );

            AssociatePeopleWithTheirSkills(
                new List<Pair<string, IEnumerable<string>>>()
                {
                    new Pair<string,IEnumerable<string>>() {
                        First = message.DetailedPersonDto.Id,
                        Second = message.DetailedPersonDto.Skills
                    }
                }
            );

            AssociatePeopleWithTheirLocations(
                new List<Pair<string,string>>() {
                    new Pair<string,string>() {
                        First = message.DetailedPersonDto.Id,
                        Second = message.DetailedPersonDto.Location
                    }
                }
            );

            AssociateLocationsWithTheirCountry(
                new List<Pair<string, string>>() {
                    new Pair<string,string>() {
                        First = message.DetailedPersonDto.Location,
                        Second = message.DetailedPersonDto.CountryCode
                    }
                }
            );

            AssociatePersonWithThierSchools(
                new Pair<string,IEnumerable<string>>() { First = message.DetailedPersonDto.Id, Second = message.DetailedPersonDto.Educations.Select(e => e.SchoolName ).Distinct() }
            );

            AssociatePersonWithCompany
            (
                new Pair<string, IEnumerable<string>>() { First = message.DetailedPersonDto.Id, Second = message.DetailedPersonDto.Positions.Select(p => p.Company.Id).Distinct()}
            );
        }

        private void AssociatePersonWithThierSchools(Pair<string, IEnumerable<string>> pair)
        {
            _graphClient.Cypher
                .Match("(person:Person)", "(school:School)")
                .Where("person.Id = {id}")
                .AndWhere("school.Name IN {names}")
                .WithParam("id", pair.First)
                .WithParam("names", pair.Second)
                .CreateUnique("(person)-[:EDUCATED_AT]-(school)")
                .ExecuteWithoutResults();
        }

        private void AssociatePersonWithCompany(Pair<string, IEnumerable<string>> pair)
        {
            _graphClient.Cypher
                .Match("(person:Person)", "(company:Company)")
                .Where("person.Id = {id}")
                .AndWhere("company.Id IN {ids}")
                .WithParam("id", pair.First)
                .WithParam("ids", pair.Second)
                .CreateUnique("(person)-[:WORKED_FOR]->(company)")
                .ExecuteWithoutResults();
        }

        private void MergeCompanies(IEnumerable<DataTransfer.CompanyDto> companies)
        {
            var companiesInDB =
            _graphClient.Cypher
                .Match("(company:Company)")
                .Where("company.Id IN {ids}")
                .WithParam("ids", companies.Select(c => c.Id).Distinct())
                .Return(company => company.As<ConnectedData.DataTransfer.CompanyDto>())
                .Results;


            var companiesToCreate =
                companies.Except(companiesInDB, new DataTransfer.CompanyDtoIdComparer())
                .Distinct(new DataTransfer.CompanyDtoIdComparer());

            _graphClient.Cypher
                .Create("(company:Company {companies})")
                .WithParam("companies", companiesToCreate)
                .ExecuteWithoutResults();
        }

        private void MergeSchools(IEnumerable<string> schools)
        {
            var schoolsInDb = 
            _graphClient.Cypher
                .Match("(school:School)")
                .Where("school.Name IN {schoolNames}")
                .WithParam("schoolNames",schools.Distinct())
                .Return(school => school.As<string>())
                .Results;

            var schoolsToCreate = schools.Distinct().Except(schoolsInDb);

            _graphClient.Cypher
                .Create("(school:School {schools})")
                .WithParam("schools", schoolsToCreate.Select(s => new { Name = s}))
                .ExecuteWithoutResults();
        }

        private void AssociatePeopleWithTheirSkills(List<Pair<string, IEnumerable<string>>> list)
        {
            if (list.IsNullOrEmpty()) return;

            foreach (var pair in list)
            {
//                match (u:User),(p:Product)
//where rand() < 0.1
//with u,p
//limit 50000
//merge (u)-[:OWN]->(p);

                var cypher = _graphClient
                    .Cypher
                    .Match("(p:Person)","(s:Concept)")
                    .Where((Person p) => p.Id == pair.First)
                    .AndWhere("(s.Name IN {skills})")
                    .CreateUnique("(p)-[:HAS_SKILL]-(s)")
                    .WithParam("skills", pair.Second);
                
                Console.WriteLine(cypher.Query.DebugQueryText);
                Console.WriteLine(cypher.Query.QueryText);

                    cypher.ExecuteWithoutResults();
            }
        }

        protected void MergeSkills(IEnumerable<string> skills)
        {
            if (skills.IsNullOrEmpty()) return;

            var existingSkills = _graphClient
                .Cypher
                .Match("(skill:Concept)")
                .Where("(skill.Name IN {skills})")
                .WithParam("skills", skills)
                .Return(skill => skill.As<string>())
                .Results;

            var newSkills = skills.Except(existingSkills);

            CreateSkills(newSkills);
        }

        private void CreateSkills(IEnumerable<string> skills)
        {
            if (skills.IsNullOrEmpty()) return;
            foreach (var skill in skills)
                _graphClient.Cypher
                    .Merge("(skill:Concept { Name: {name}})")
                    .OnCreate()
                    .Set("skill = {newSkill}")
                    .WithParams(new
                    {
                        name = skill,
                        newSkill = new { Name = skill }
                    })
                    .ExecuteWithoutResults();

        }

        protected void MergePerson(Person personToMerge)
        {
            _graphClient.Cypher
                .Merge("(person:Person { Id : {id} })")
                .OnCreate()
                .Set("person = {newPerson}")
                .OnMatch()
                .Set("person = {updatedPerson}")
                .WithParam("id",personToMerge.Id)
                .WithParam("newPerson", personToMerge)
                .WithParam("updatedPerson",personToMerge)
                .ExecuteWithoutResults();
        }
    }
}
