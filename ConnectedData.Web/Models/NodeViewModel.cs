using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.Web.Models
{
    public class NodeViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
    }
}
