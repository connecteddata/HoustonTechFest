using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class UserInterestsQuery : ShortBus.IRequest<IEnumerable<Interest>>
    {
        public readonly string UserId;

        public UserInterestsQuery(string userId)
        {
            UserId = userId;
        }

    }
}
