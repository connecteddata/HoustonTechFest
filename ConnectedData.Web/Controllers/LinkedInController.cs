using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConnectedData.Web.Controllers
{
    public class LinkedInController : Controller

    {
        private readonly ShortBus.IMediator _mediator;

        public LinkedInController(ShortBus.IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: LinkedInSync
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RetrieveData()
        {
            //mediate a call to linked in to get data
            //var linkedInDataResponse = _mediator.Request(LinkedInDataForUser())
            //mediate a call to graph db to persist
            //redirect to linked in summary 
            return RedirectToAction("Summary");
        }

        public ActionResult Summary()
        {
            //populate the view necessary, data will be filled in via graph summary
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