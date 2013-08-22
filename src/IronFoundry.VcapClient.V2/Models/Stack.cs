using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Stack
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}