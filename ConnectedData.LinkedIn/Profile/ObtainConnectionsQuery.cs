using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Profile
{
    public class ObtainConnectionsQuery : LinkedInQuery<IEnumerable<PersonDto>>
    {
        public ObtainConnectionsQuery(string accessToken, string userId) : base(accessToken, userId) { }
    }
}
