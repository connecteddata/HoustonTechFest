using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConnectedData.LinkedIn.Connections
{
  public class ObtainConnectionsQueryHandler : LinkedInQueryHandler<ObtainConnectionsQuery,IEnumerable<PersonDto>>
  {

      protected override IEnumerable<PersonDto> HandleQuery(ObtainConnectionsQuery query)
      {
          var api = new ConnectionsAPI(query.AccessToken);

          return api.Result();
      }
  }
}
