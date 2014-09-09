using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class Person
    {
        public string Id { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string CountryCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Headline { get; set; }

        public ISet<string> Skills { get; set; }
        public ISet<Education> Educations { get; set; }

        public ISet<Position> Positions { get; set; }
    }
}
