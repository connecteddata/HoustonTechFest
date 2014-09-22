using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConnectedData.Web.Controllers
{

    public class ApiBaseController : ApiController
    {

        private string _linkedInAccessToken = String.Empty;
        private string _linkedInUserId = String.Empty;

        protected readonly ShortBus.IMediator _mediator;
        public ApiBaseController(ShortBus.IMediator mediator)
            : this()
        {
            _mediator = mediator;
        }

        protected String LinkedInAccesToken
        {
            get
            {
                return _linkedInAccessToken;
            }
        }

        protected String LinkedInUserId
        {
            get
            {
                return _linkedInUserId;
            }
        }

        private ApiBaseController()
        {
            var claimsPrincipal = System.Threading.Thread.CurrentPrincipal as System.Security.Claims.ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var accessTokenClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:accesstoken");
                _linkedInAccessToken = null != accessTokenClaim ? accessTokenClaim.Value : string.Empty;

                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "urn:connectedData:LinkedInUserId");
                _linkedInUserId = null != userIdClaim ? userIdClaim.Value : string.Empty;
            }
        }
    }
}
