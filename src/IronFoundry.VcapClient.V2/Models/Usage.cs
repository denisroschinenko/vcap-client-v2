using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Usage
    {
        [JsonProperty(PropertyName = "time")]
        public DateTimeOffset CurrentTime { get; set; }

        [JsonProperty(PropertyName = "cpu")]
        public float CpuTime { get; set; }

        [JsonProperty(PropertyName = "mem")]
        public float MemoryUsage { get; set; }

        [JsonProperty(PropertyName = "disk")]
        public float DiskUsage { get; set; }
    }
}
