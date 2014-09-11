using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Messaging.Notifications
{
    public class ObtainedUserProfileNotification : Notification
    {
        public readonly string UserId;
        public readonly DetailedPersonDto DetailedPersonDto;

        public ObtainedUserProfileNotification(string userId, DetailedPersonDto detailedPersonDto)
        {
            DetailedPersonDto = detailedPersonDto;
            UserId = userId;
        }
    }
}
