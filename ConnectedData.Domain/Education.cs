using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class Education
    {

            public string Id { get; set; }
            public string SchoolName { get; set; }

            public string FieldOfStudy { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public string Degree { get; set; }
        
    }
}
