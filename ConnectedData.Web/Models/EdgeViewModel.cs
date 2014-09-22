using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.Web.Models
{
    public class EdgeViewModel
    {
        [JsonProperty("source")]
        public int Source{ get; set; }
        [JsonProperty("target")]
        public int Target { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
