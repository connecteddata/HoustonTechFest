using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectedData.Utils;
using ConnectedData.DataTransfer;
using Newtonsoft.Json;

namespace ConnectedData.SqlToGraphDataTransfer.ConsoleApp
{
    class Program
    {
        public static void Main()
        {
            List<string> _profilesCollected = new List<string>();
            List<string> _connectionCollected = new List<string>();

            var profileMapper = new ConnectedData.LinkedIn.Profiles.ProfileDeserializer();
            var connectionsMapper = new ConnectedData.LinkedIn.Connections.ConnectionsDeserializer();

            var localUrl = "http://localhost:7474/db/data";
            var connectedGraphClient = new Neo4jClient.GraphClient(new Uri(localUrl));
            connectedGraphClient.Connect();


            var connectionsGraphHandler = new ConnectedData.GraphDB.ObtainedConnectionsNotificationHandler(connectedGraphClient);
            var profileGraphHandler = new ConnectedData.GraphDB.Notifications.ObtainedProfileNotificationHandler(connectedGraphClient);

            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            Console.Clear();

            // Establish an event handler to process key press events.
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
            
            Console.WriteLine("CTRL+C to interrupt the read operation:");

            DateTime then = DateTime.MinValue;
            DateTime now = DateTime.UtcNow;

            while (true)
            {
                now = DateTime.UtcNow;
                
                
                Console.WriteLine(string.Format("Checking for new records between '{0}' and '{1}'",then.ToString("s"),now.ToString("s")));

                
                using(var db = new ConnectedData.AzureSQLServer.DBEntities())
                {
                    var newProfiles =
                        db.LinkedInApiDatas
                        .Where(d =>
                            !_profilesCollected.Contains(d.LinkedInUserId) 
                            && !_connectionCollected.Contains(d.LinkedInUserId)
                            && d.LastModified.HasValue
                            && d.LastModified.Value > then
                            && d.LastModified.Value <= now
                        )
                        .Select(p => new { UserId = p.LinkedInUserId, ProfileDtoString = p.Profile, Connections = p.Connections });                   
                    
                    
                    Console.WriteLine(string.Format("Found '{0}' new profiles",newProfiles.Count()));

                    foreach(var profile in newProfiles)
                    {
                       profileGraphHandler.Handle(new Messaging.Notifications.ObtainedUserProfileNotification(profile.UserId, JsonConvert.DeserializeObject<DetailedPersonDto>(profile.ProfileDtoString)));
                       _profilesCollected.Add(profile.UserId);

                       connectionsGraphHandler.Handle(new Messaging.ObtainedUserConnectionsNotification(profile.UserId, JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(profile.Connections)));
                       _connectionCollected.Add(profile.UserId);
                    }
                }

                then = now;
                

                // Exit if the user pressed the 'X' key. 
                if (cki.Key == ConsoleKey.X) break;
                Thread.Sleep(30*1000);
            }
        }

        protected static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("\nThe read operation has been interrupted.");
            Console.WriteLine("shutting down in 2 seconds");
            Thread.Sleep(2000);

        }
    }
}
