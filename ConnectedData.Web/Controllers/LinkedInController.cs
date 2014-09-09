using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConnectedData.Web.Controllers
{
    public class LinkedInController : AuthenticatedControllerBase
    {

        public LinkedInController(ShortBus.IMediator mediator) : base(mediator)
        {
        }

        // GET: LinkedInSync
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> RetrieveData()
        {
            //mediate a call to linked in to get data
            var profileResponse = await _mediator.RequestAsync<DetailedPersonDto>(new LinkedIn.Profiles.ObtainProfileQuery(LinkedInAccesToken));

            if (profileResponse.HasException())
                throw profileResponse.Exception;

            //notify all subscribers (mainly just graphDB, who will persist any updates)
            var persist = await _mediator.NotifyAsync(profileResponse.Data);

            if (persist.HasException())
                throw persist.Exception;

            var connectionsResponse = await _mediator.RequestAsync<IEnumerable<PersonDto>>(new LinkedIn.Connections.ObtainConnectionsQuery(this.LinkedInAccesToken));

            if (connectionsResponse.HasException())
                throw connectionsResponse.Exception;

            //notify all subscribers (mainly just graphDB, who will persist any updates)
            persist = await _mediator.NotifyAsync(profileResponse.Data);

            if (persist.HasException())
                throw persist.Exception;

            return RedirectToAction("Summary");
        }

        public ActionResult Summary()
        {
            //var response = await _mediator.RequestAsync<LinkedInSummaryDto>(new ObtainLinkedInSummary(this.LinkedInUserId));
            
            //if (userLinkedInSummaryResponse.HasException())
                //throw userLinkedInSummaryResponse.Exception;
            //var model = AutoMapper.Map<LinkedInSummaryDto, LinkedInSummaryViewModel>(response.Data);
            //return View(model);
            return View();
        }

        public JsonResult GraphSummary()
        {
            //mediate a call to graph db to get all linked in info.  would be terrific to graph represent that.
            //look at http://bl.ocks.org/mbostock/4062045 for inspiration
            throw new NotImplementedException();
        }
    }
}