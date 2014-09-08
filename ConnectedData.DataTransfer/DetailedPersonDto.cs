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

    public class SkillDto
    {
        public string Name { get; set; }
    }

    public class EducationDto
    {
        public string Id { get; set; }
        public string SchoolName { get; set; }

        public string FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Degree { get; set; }
    }

    public class PositionDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string  Summary { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public CompanyDto Company { get; set; }
    }

    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
