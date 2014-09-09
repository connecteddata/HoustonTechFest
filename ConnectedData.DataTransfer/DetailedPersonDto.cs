using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.DataTransfer
{
    public class DetailedPersonDto : PersonDto
    {
        [LinkedInField("first-name")]
        public string FirstName { get; set; }
        [LinkedInField("last-name")]
        public string LastName { get; set; }
        [LinkedInField("headline")]
        public string Headline { get; set; }
        
        [LinkedInField("skills")]
        public IEnumerable<string> Skills { get; set; }
        [LinkedInField("educations")]
        public IEnumerable<EducationDto> Educations { get; set; }

        [LinkedInField("positions")]
        public IEnumerable<PositionDto> Positions { get; set; }

    }

}
