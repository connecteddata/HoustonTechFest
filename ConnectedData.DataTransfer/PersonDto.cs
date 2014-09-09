using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.DataTransfer
{
    public class PersonDto
    {
        [LinkedInField("id")]
        public string Id { get; set; }
        [LinkedInField("industry")]
        public string Industry { get; set; }
        [LinkedInField("location:(name)")]
        public string Location { get; set; }
        [LinkedInField("location:(country:(code))")]
        public string CountryCode { get; set; }

    }
}
