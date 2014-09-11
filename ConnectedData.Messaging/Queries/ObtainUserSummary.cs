using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Messaging.Queries
{
    public class ObtainUserSummary : Query<IEnumerable<UserSummaryDto>>
    {
        public readonly string UserId;

        public ObtainUserSummary(string userId)
        {
            UserId = userId;
        }
    }
}
