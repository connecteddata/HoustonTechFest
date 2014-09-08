using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Service.API
{
    public abstract class APIBase<TAPIReturnData, TOut>
    {
        protected RestSharp.RestClient _client;

        private readonly string _accessToken;

        private readonly string _apiUrl;

        protected readonly IMapper<TAPIReturnData, TOut> _mapper;

        public APIBase(string accessToken, string apiUrl, IMapper<TAPIReturnData,TOut> mapper)
        {
            _accessToken = accessToken;
            _apiUrl = apiUrl;
            _mapper = mapper;
            _client = new RestSharp.RestClient();
            
        }

        protected virtual RestSharp.IRestRequest AddAuthorizationHeader(RestSharp.IRestRequest request)
        {
            request.AddHeader("Authorization", string.Format("Bearer {0}", _accessToken));
            return request;
        }

        protected virtual RestSharp.IRestRequest Request()
        {
            RestSharp.IRestRequest request = new RestSharp.RestRequest(_apiUrl, RestSharp.Method.GET);
            request = AddAuthorizationHeader(request);
            return request;
        }

        public abstract TOut Result();
    }

    public abstract class APIMappingStringContentReponse<TOut> 
        : APIBase<string,TOut>
    {            
        public APIMappingStringContentReponse(string accessToken, string apiUrl, IMapper<string,TOut> mapper)
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
