using ConnectedData.Messaging.Queries;
using ConnectedData.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConnectedData.Web.Controllers
{
    public class ForceDirectedGraphController : ApiBaseController
    {
        public ForceDirectedGraphController(ShortBus.IMediator mediator)
            : base(mediator) {}

        public async Task<ForceDirectedViewModel> Get()
        {

            var result = await _mediator.RequestAsync(new ImmediateGraphForUser(this.LinkedInUserId));

            if (result.HasException())
                throw result.Exception;

            var nodes = result.Data.Vertices.Select(v => new NodeViewModel() {
                        Label = v.Label,
                        Id = v.Index,
                        Group = v.Group
                    })
                    .OrderBy(nvm => nvm.Id)
                    .ToList();

            //to do update for uniqueness
            var edges = result.Data.Edges.Select(e => new EdgeViewModel() {
                        Source = nodes.IndexOf(nodes.First(n => n.Id == e.SourceVertexId)),
                        Target = nodes.IndexOf(nodes.First(n => n.Id == e.TargetVertexId)),
                        Value = 1
                    });
            
            var model =
                new Web.Models.ForceDirectedViewModel() {
                    Nodes = nodes,
                    Edges = edges
                };

            return model;
                
        }
    }
}
