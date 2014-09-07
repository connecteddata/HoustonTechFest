using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConnectedData.LinkedIn.Service
{
    internal class Connections
    {
        public List<LinkedInPersonDto> People { get; set; }
    }

    public class LinkedInPersonDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Headline { get; set; }
        public Uri PictureUrl { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string CountryCode { get; set; }

    }

    public interface IMapper<in T, out V>
    {
        V Map(T t);
    }

    //public class LinkedInPersonXmlToPersonDtoMapper : IMapper<XmlNode,LinkedInPersonDto>
    //{
    //    LinkedInPersonDto Map(XmlNode node)
    //    {

    //    }
    //}
    public class ObtainUserConnectionsFromLinkedin
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
    }

    public class ObtainConnectionsCommandHandler //: ICommandHandler<ObtainUserConnections>
    {
        private string _accessToken;
        public ObtainConnectionsCommandHandler()
        {
        }

        public IEnumerable<LinkedInPersonDto> Handle(ObtainUserConnectionsFromLinkedin @command)
        {
            var dtos = new List<LinkedInPersonDto>();

            RestSharp.RestClient client = new RestSharp.RestClient();
            //client.AddHandler("application/xml", new ConnectionsDeserializer());
            RestSharp.IRestRequest request = new RestSharp.RestRequest("https://api.linkedin.com/v1/people/~/connections", RestSharp.Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", @command.AccessToken));
            var response = client.Execute(request);
            var mapper = new ConnectedData.LinkedIn.Service.ConnectionsDeserializer();
            var results = mapper.Map(response.Content);
            dtos.AddRange(results);
            return dtos;
        }


    }


}
