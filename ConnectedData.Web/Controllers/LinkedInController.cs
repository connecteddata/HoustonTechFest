using ConnectedData.DataTransfer;
using ConnectedData.Messaging.Notifications;
using ConnectedData.Messaging.Queries;
using ConnectedData.Web.Models;
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
            // short circuit if data has already been retrieved or even make that part of a user or context object

            //mediate a call to linked in to get data
            var profileResponse = await _mediator.RequestAsync<DetailedPersonDto>(new LinkedIn.Profiles.ObtainProfileQuery(LinkedInAccesToken));

            if (profileResponse.HasException())
                throw profileResponse.Exception;

            //notify all subscribers (mainly just graphDB, who will persist any updates)
            var persist = await _mediator.NotifyAsync(new ObtainedUserProfileNotification(profileResponse.Data));

            if (persist.HasException())
                throw persist.Exception;

            var connectionsResponse = await _mediator.RequestAsync<IEnumerable<PersonDto>>(new LinkedIn.Connections.ObtainConnectionsQuery(this.LinkedInAccesToken));

            if (connectionsResponse.HasException())
                throw connectionsResponse.Exception;

            //notify all subscribers (mainly just graphDB, who will persist any updates)
            persist = await _mediator.NotifyAsync(new Messaging.ObtainedUserConnectionsNotification(this.LinkedInUserId, connectionsResponse.Data));

            if (persist.HasException())
                throw persist.Exception;

            return RedirectToAction("Summary");
        }

        public async Task<ActionResult> Summary()
        {
            var response = await _mediator.RequestAsync<IEnumerable<UserSummaryDto>>(new ObtainUserSummary(this.LinkedInUserId));

            if (response.HasException())
                throw response.Exception;
            var result = response.Data.FirstOrDefault(); 
            if (null == result) throw new InvalidOperationException(string.Format("Unable to gather the LinkedIn Summary Data for user '{0}'", this.LinkedInUserId));
            
            
            return View(model: new LinkedInSummaryViewModel() { NumberOfConnections = result.NumberOfConnections, NumberOfIndustries = result.NumberOfIndustries });
        }

        public JsonResult GraphSummary()
        {
            //mediate a call to graph db to get all linked in info.  would be terrific to graph represent that.
            //look at http://bl.ocks.org/mbostock/4062045 for inspiration
            throw new NotImplementedException();
        }
    }
}