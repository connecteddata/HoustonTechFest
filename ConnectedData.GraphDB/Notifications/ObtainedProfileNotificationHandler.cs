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
            MergePeople(people);

            var industries = new List<Industry>() { new Industry() { Name = message.DetailedPersonDto.Industry } };
            MergeIndustries(industries);


            var locations = new List<Location>() { new Location() { Name = message.DetailedPersonDto.Location } };
            MergeLocations(locations);

            var countries = new List<Country>() { new Country() { CountryCode = message.DetailedPersonDto.CountryCode } };

            MergeCountries(countries); ;

            AssociatePeopleWithTheirIndustries(
                new List<Pair<string,string>>() {
                    new Pair<string,string>() {
                        First = message.DetailedPersonDto.Id,
                        Second = message.DetailedPersonDto.Industry
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
        }
    }
}
