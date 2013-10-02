using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class InstanceDetail 
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "since")]
        public int Since { get; set; }

        [JsonProperty(PropertyName = "debug_port")]
        public string DebugPort { get; set; }

        [JsonProperty(PropertyName = "console_port")]
        public string ConsolePort { get; set; }

        [JsonProperty(PropertyName = "console_ip")]
        public string ConsoleIp { get; set; }

        [JsonProperty(PropertyName = "debug_ip")]
        public string DebugIp { get; set; }
    }
}
