using ConnectedData.DataTransfer;
using ConnectedData.LinkedIn.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Service.Profile
{
    public class ObtainLinkedInProfileQueryHandler : LinkedInQueryHandler<ObtainConnectionsQuery, IEnumerable<PersonDto>>
    {
        public DetailedPersonDto Handle(ObtainProfileQuery request)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<PersonDto> HandleQuery(ObtainConnectionsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
