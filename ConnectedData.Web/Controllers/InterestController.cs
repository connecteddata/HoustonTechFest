using ConnectedData.Domain;
using ConnectedData.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ConnectedData.Web.Controllers
{
    
    public class InterestController : AuthenticatedControllerBase
    {
        public InterestController(ShortBus.IMediator mediator) : base (mediator)
        {
        }

        // GET: Interest
        public ActionResult Index()
        {
            var response = _mediator.Request<IEnumerable<Interest>>(new UserInterestsQuery("3"));
            if (null == response) throw new Exception("unable to obtain a valid response from the mediator.  Please check its configurations.");
            if (response.HasException()) throw response.Exception;

            var interests = response.Data;            
            
            ViewBag.Interests = interests.Select(i => new InterestViewModel() { Name = i.Name });
            return View(model: new InterestViewModel());
        }

        // GET: Interest/Create
        [HttpPost]
        public ActionResult Add(InterestViewModel interest)
        {
            

            //var response = _mediator.Send(new AddInterestToUser("3", interest));
            return RedirectToActionPermanent("Index");
        }

        // POST: Interest/Create
        [HttpPost]
        public ActionResult Remove(InterestViewModel collection)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
