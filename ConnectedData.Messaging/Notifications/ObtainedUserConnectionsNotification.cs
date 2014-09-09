using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Messaging
{
    public class ObtainedUserConnectionsNotification : IMessage
    {
        public readonly IEnumerable<PersonDto> Connections;
        public readonly string UserId;

        public ObtainedUserConnectionsNotification(string userId, IEnumerable<PersonDto> connections)
        {
            UserId = userId;
            Connections = connections;
        }
    }
}
