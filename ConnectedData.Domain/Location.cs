using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class Location
    {
        public string Name { get; set; }
        public Country Country { get; set; }
    }

    public class Country
    {
        public string CountryCode { get; set; }
    }

    public class Skill
    {
        public string Name { get; set; }
    }
}
