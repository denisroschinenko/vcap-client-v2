using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class StatInfo 
    {
        [JsonIgnore]
        public int StatInfoId { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "stats")]
        public Stats Stats { get; set; }
    }
}
