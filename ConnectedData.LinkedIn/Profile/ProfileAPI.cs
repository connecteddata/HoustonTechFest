using ConnectedData.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Service.API
{
    class ProfileAPI : APIMappingStringContentReponse<DetailedPersonDto>
    {
        public ProfileAPI(string acccessToken)
            : base(acccessToken, APIUrlWithFieldSelectors(), new ProfileDeserializer())
        {
        }

        private static string APIUrlWithFieldSelectors()
        {
            var builer = new StringBuilder("https://api.linkedin.com/v1/people/~");
            List<string> names = new List<string>();
            foreach(var propertyInfo in typeof(DetailedPersonDto).GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttributes(true).FirstOrDefault(a => (a as LinkedInField) != null);
                if (null != attribute)
                {
                    names.Add((attribute as LinkedInField).APIField);
                }
            }
            if (0 < names.Count())
            {
                builer.Append(":(");
                builer.Append(string.Join(",", names.Distinct()));
                builer.Append(")");
            }
                            
            return builer.ToString();
        }
    }
}
