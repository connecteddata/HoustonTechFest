using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Profiles
{
    public class ObtainProfileQuery : LinkedInQuery<DetailedPersonDto>
    {
        public ObtainProfileQuery(string accessToken) : base(accessToken) { }
    }
}
