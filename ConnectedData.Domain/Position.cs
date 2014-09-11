using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public class Position : DomainObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public Company Company { get; set; }
    }
}
