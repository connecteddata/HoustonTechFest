using ConnectedData.DataTransfer;
using ConnectedData.Messaging.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.AzureSQLServer
{
    public class ImmediateGraphForUserHandler : ShortBus.IAsyncRequestHandler<ImmediateGraphForUser,GraphDto>
    {
        public Task<GraphDto> HandleAsync(ImmediateGraphForUser request)
        {
            var dto = new GraphDto();
            using (var db = new DBEntities())
            {
                var userLinkedInData =
                    db.LinkedInApiDatas
                    .Single(d => d.LinkedInUserId == request.UserId);
                    
                var profile = JsonConvert.DeserializeObject<DetailedPersonDto>(userLinkedInData.Profile);
                
                var connections = JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(userLinkedInData.Connections);

                var vertices = new List<VertexDto>();
                vertices.Add(new VertexDto() { 
                    Group = "Person", 
                    Id = profile.Id, 
                    Index = 0, 
                    Label = String.Format("{0} {1}", String.IsNullOrEmpty(profile.FirstName) ? string.Empty : profile.FirstName, string.IsNullOrEmpty(profile.LastName) ? string.Empty, profile.LastName) 
                });
                
                vertices.AddRange(
                    connections.Select((c,index) => new VertexDto() { 
                        Id = c.Id, 
                        Index = index + 1, 
                        Group = "Person" 
                    })
                );

                var industryVertices =
                    connections
                    .Select(c => c.Industry)
                    .Distinct()
                    .Select((industry, index) => new VertexDto() {
                        Group = industry,
                        Id =  industry,
                        Index = index + vertices.Count(),
                        Label = industry
                    })
                    .ToList();

                if (null == industryVertices.FirstOrDefault(i => i.Label == profile.Industry))
                    industryVertices.Add(new VertexDto() {  
                        Group = profile.Industry,
                        Id = profile.Industry,
                        Index = industryVertices.Count() + 1,
                        Label = profile.Industry
                    });


                vertices.AddRange(industryVertices);
                

                var edges = new List<EdgeDto>();
                
                edges.AddRange(
                    connections.Select(c => new EdgeDto() { 
                        SourceVertexId = 0,
                        TargetVertexId = vertices.Single(v => v.Id == c.Id).Index 
                    })
                );

                edges.AddRange(
                    connections.Select(c => new EdgeDto() { 
                        SourceVertexId = vertices.Single(v => v.Id == c.Industry).Index,
                        TargetVertexId = vertices.Single(v => v.Id == c.Id).Index
                    })
                );

                dto.Vertices = new HashSet<VertexDto>(vertices,new VertexDtoEqualityComparer());
                dto.Edges = new HashSet<EdgeDto>(edges, new EdgeDtoEqualityComparer());
                
            }
            return Task.FromResult<GraphDto>(dto);
        }
    }
}
