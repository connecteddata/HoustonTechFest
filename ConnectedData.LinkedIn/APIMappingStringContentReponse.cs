using ConnectedData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn
{
    public abstract class APIMappingStringContentReponse<TOut>
        : APIBase<string, TOut>
    {
        public APIMappingStringContentReponse(string accessToken, string apiUrl, IMapper<string, TOut> mapper)
            : base(accessToken, apiUrl, mapper)
        {

        }

        public override TOut Result()
        {
            var response = _client.Execute(Request());
            return _mapper.Map(response.Content);
        }

    }

}
