using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.LinkedIn
{
    public abstract class LinkedInQueryHandler<TQuery, TResponseData> : ShortBus.IRequestHandler<TQuery,TResponseData>, ShortBus.IAsyncRequestHandler<TQuery,TResponseData>
        where TQuery : LinkedInQuery<TResponseData>
    {

        public TResponseData Handle(TQuery query)
        {
            return HandleQuery(query);
        }

        protected abstract TResponseData HandleQuery(TQuery query);

        public System.Threading.Tasks.Task<TResponseData> HandleAsync(TQuery query)
        {
            return System.Threading.Tasks.Task<TResponseData>.FromResult(HandleQuery(query));
        }
    }
}
