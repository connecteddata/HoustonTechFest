using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Profiles
{
    public class ObtainLinkedInProfileQueryHandler : LinkedInQueryHandler<ObtainProfileQuery, DetailedPersonDto>
    {
        protected override DetailedPersonDto HandleQuery(ObtainProfileQuery query)
        {
            var api = new ProfileAPI(query.AccessToken);

            return api.Result();
        }
    }
}
