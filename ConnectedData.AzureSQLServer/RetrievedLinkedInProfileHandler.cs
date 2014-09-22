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
    public class RetrievedLinkedInProfileHandler : NotificationHandler<ObtainedUserProfileNotification>
    {
        protected override void HandleProxy(ObtainedUserProfileNotification notification)
        {
            var profile = JsonConvert.SerializeObject(notification.DetailedPersonDto);
            using (var db = new DBEntities())
            {
                var userLinkedInData = db.LinkedInApiDatas.Where(d => d.LinkedInUserId == notification.UserId).SingleOrDefault();
                if (null == userLinkedInData)
                    db.LinkedInApiDatas.Add(new LinkedInApiData() { Id = Guid.NewGuid(), Profile = profile, LinkedInUserId = notification.UserId, LastModified = DateTime.Now });
                else
                {
                    userLinkedInData.Profile = profile;
                    userLinkedInData.LastModified = DateTime.Now;
                }
                db.SaveChanges();
            }
        }
    }

}
