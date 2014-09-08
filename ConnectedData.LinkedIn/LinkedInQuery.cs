using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.LinkedIn
{
    public class LinkedInQuery<TResponseData> : ShortBus.IRequest<TResponseData>, ShortBus.IAsyncRequest<TResponseData>
    {
        public readonly string AccessToken;
        public readonly string UserId;

        public LinkedInQuery(string accessToken, string userId)
        {
            AccessToken = accessToken;
            UserId = userId;
        }
    }
}
