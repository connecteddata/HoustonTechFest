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

        
        protected Person GetPersonFromUserId(string userId)
        {
            var cypher = _graphClient.Cypher
                .Match("(person:Person)")
                .Where((Person person) => person.Id == userId)
                .Return(person => person.As<Person>());
            return cypher
                .Results
                .FirstOrDefault();
        }


        protected virtual void MergePeople(IEnumerable<Person> people)
        {
            //people in db
            var peopleInDb =
                _graphClient.Cypher
                .Match("(person:Person)")
                .Where("(person.Id IN {ids})")
                .WithParam("ids", people.Select(p => p.Id).Distinct())
                .Return(person => person.As<Person>())
                .Results;


            var filteredPeople = people.Except(peopleInDb, new PersonIdComparer());
            CreatePeople(filteredPeople);
        }


        private void CreatePeople(IEnumerable<Person> filteredPeople)
        {
            if (filteredPeople.IsNullOrEmpty()) return;

            var cypher = _graphClient.Cypher
                .Create("(person:Person {people})")
                .WithParam("people", filteredPeople);
            cypher.ExecuteWithoutResults();
        }


        protected void MergeIndustries(IEnumerable<Industry> industries)
        {
            if (industries.IsNullOrEmpty()) return;

            var existingIndustries = _graphClient
                .Cypher
                .Match("(industry:Industry)")
                .Where("(industry.Name IN {industries})")
                .WithParam("industries", industries)
                .Return(industry => industry.As<Industry>())
                .Results;

            var newIndustries = industries.Except(existingIndustries);

            if (newIndustries.IsNullOrEmpty()) return;

            CreateIndustries(newIndustries);
        }

        private void CreateIndustries(IEnumerable<Industry> industries)
        {
            if (industries.IsNullOrEmpty()) return;
            foreach (var industry in industries)
                _graphClient.Cypher
                    .Merge("(industry:Industry { Name: {name}})")
                    .OnCreate()
                    .Set("industry = {newIndustry}")
                    .WithParams(new
                    {
                        name = industry.Name,
                        newIndustry = industry
                    })
                    .ExecuteWithoutResults();

        }


        protected void MergeLocations(IEnumerable<Location> enumerable)
        {
            var locationsInDb =
                _graphClient.Cypher
                .Match("(location:Location)")
                .Where("(location.Name IN {names})")
                .WithParam("names", enumerable.Select(e => e.Name))
                .Return(location => location.As<Location>())
                .Results;

            var filteredLocations = enumerable.Except(locationsInDb, new LocationNameComparer());
            CreateLocations(filteredLocations);
        }


        protected void MergeCountries(IEnumerable<Country> countries)
        {
            var countriesInDb =
                _graphClient.Cypher
                .Match("(country:Country)")
                .Where("(country.CountryCode IN {codes})")
                .WithParam("codes", countries.Select(e => e.CountryCode))
                .Return(country => country.As<Country>())
                .Results;

            var filteredCountries = countries.Except(countriesInDb, new CountryCodeComparer());
            CreateCountries(filteredCountries);
        }

        private void CreateCountries(IEnumerable<Country> filteredCountries)
        {
            _graphClient.Cypher
                .Create("(country:Country {countries})")
                .WithParam("countries", filteredCountries)
            .ExecuteWithoutResults();

        }

        private void CreateLocations(IEnumerable<Location> locations)
        {
            _graphClient
                .Cypher
                .Create("(location:Location {locations})")
                .WithParam("locations", locations.Select(l => new { Name = l.Name }))
                .ExecuteWithoutResults();
        }

        protected void AssociatePeopleWithTheirIndustries(IEnumerable<Pair<string, string>> personIdIndustryNamePairs)
        {
            if (personIdIndustryNamePairs.IsNullOrEmpty()) return;
            var groupedByIndustry =
                personIdIndustryNamePairs
                .Select(p => new { PersonId = p.First, IndustryName = p.Second })
                .GroupBy(key => key.IndustryName, element => element.PersonId, (key, element) => new { IndustryName = key, PersonIds = element.ToList() });
            foreach (var grouping in groupedByIndustry)
            {
                var industryName = grouping.IndustryName;
                var personIds = grouping.PersonIds;
                _graphClient.Cypher
                    .Match("(person:Person)", "(industry:Industry)")
                    .Where("(person.Id IN {personIds})")
                    .AndWhere((Industry industry) => industry.Name == industryName)
                    .WithParam("personIds", personIds)
                    .CreateUnique("(person)-[:WORKS_IN]->(industry)")
                    .ExecuteWithoutResults();
            }

        }

        protected void AssociateLocationsWithTheirCountry(IEnumerable<Pair<string, string>> locationCountryCodePairs)
        {
            if (locationCountryCodePairs.IsNullOrEmpty()) return;
            var groupedByCountry =
                locationCountryCodePairs
                .Select(p => new { LocationName = p.First, CountryCode = p.Second })
                .GroupBy(key => key.CountryCode, element => element.LocationName, (key, element) => new { CountryCode = key, Locations = element.ToList() });

            foreach (var pair in groupedByCountry)
            {
                var countryCode = pair.CountryCode;
                var locations = pair.Locations;
                _graphClient
                    .Cypher
                    .Match("(location:Location)", "(country:Country)")
                    .Where("(location.Name IN {locations})")
                    .AndWhere((Country country) => country.CountryCode == countryCode)
                    .WithParam("locations", locations)
                    .CreateUnique("(location)-[:IS_WITHIN]->(country)")
                    .ExecuteWithoutResults();
            }

        }

        protected void AssociatePeopleWithTheirLocations(IEnumerable<Pair<string, string>> userIdLocationNamePairs)
        {
            if (userIdLocationNamePairs.IsNullOrEmpty()) return;
            foreach (var pair in userIdLocationNamePairs.Select(p => new { PersonId = p.First, LocationName = p.Second }))
            {
                _graphClient.Cypher
                    .Match("(person:Person)", "(location:Location)")
                    .Where((Person person) => person.Id == pair.PersonId)
                    .AndWhere((Location location) => location.Name == pair.LocationName)
                    .CreateUnique("(person)-[:WORKS_AROUND]->(location)")
                    .ExecuteWithoutResults();
            }
        }


    }
}
