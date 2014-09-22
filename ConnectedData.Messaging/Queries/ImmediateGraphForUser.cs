using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedData.DataTransfer;

namespace ConnectedData.Messaging.Queries
{
    public class ImmediateGraphForUser : Query<GraphDto>
    {
        public readonly string UserId;

        public ImmediateGraphForUser(string userId)
        {
            UserId = userId;
        }
    }
}
