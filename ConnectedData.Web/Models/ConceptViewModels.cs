using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConnectedData.Web.Models
{
    public class ConceptViewModel
    {
        public string Name { get; set; }
    }

    public class InterestViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}