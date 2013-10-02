using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Stats
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }

        [JsonProperty(PropertyName = "uptime")]
        public double Uptime { get; set; }

        [JsonProperty(PropertyName = "uris")]
        public string[] Uris { get; set; }

        [JsonProperty(PropertyName = "mem_quota")]
        public long MemoryQuota { get; set; }

        [JsonProperty(PropertyName = "disk_quota")]
        public long DiskQuota { get; set; }

        [JsonProperty(PropertyName = "fds_quota")]
        public long FdsQuota { get; set; }

        [JsonProperty(PropertyName = "usage")]
        public Usage Usage { get; set; }
    }
}
