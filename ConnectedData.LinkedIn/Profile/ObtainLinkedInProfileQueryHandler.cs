using ConnectedData.DataTransfer;
using ConnectedData.LinkedIn.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Profile
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
