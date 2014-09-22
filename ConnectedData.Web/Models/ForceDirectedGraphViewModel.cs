using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConnectedData.Web.Models
{
    public class ForceDirectedViewModel
    {
        [JsonProperty("nodes")]
        public IEnumerable<NodeViewModel> Nodes {get;set;}
        [JsonProperty("links")]
        public IEnumerable<EdgeViewModel> Edges { get; set; }
    }
}