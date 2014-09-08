using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Service.API
{

    public class ConnectionsAPI : APIMappingStringContentReponse<IEnumerable<PersonDto>>
    {
        public ConnectionsAPI(string acccessToken)
            : base(acccessToken, "https://api.linkedin.com/v1/people/~/connections",new ConnectedData.LinkedIn.ConnectionsDeserializer())
        {
        }
    }
}
