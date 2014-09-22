using ConnectedData.Messaging;
using ConnectedData.Messaging.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.AzureSQLServer
{

    public class RetrievedLinkedInConnectionsHandler : NotificationHandler<ObtainedUserConnectionsNotification>
    {
        protected override void HandleProxy(ObtainedUserConnectionsNotification notification)
        {
            var connections = JsonConvert.SerializeObject(notification.Connections);
            using (var db = new DBEntities())
            {
                var userLinkedInData = db.LinkedInApiDatas.Where(d => d.LinkedInUserId == notification.UserId).SingleOrDefault();
                if (null == userLinkedInData)
                    db.LinkedInApiDatas.Add(new LinkedInApiData() { Id = Guid.NewGuid(), Connections = connections, LinkedInUserId = notification.UserId, LastModified = DateTime.Now });
                else
                {
                    userLinkedInData.Connections = connections;
                    userLinkedInData.LastModified = DateTime.Now;
                }
                db.SaveChanges();
            }
        }
    }
}
