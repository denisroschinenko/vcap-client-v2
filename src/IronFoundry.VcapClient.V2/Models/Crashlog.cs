using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Crashlog 
    {
        [JsonProperty(PropertyName = "instance")]
        public string Instance { get; private set; }

        [JsonProperty(PropertyName = "since")]
        public int Since { get; private set; }
    }
}
