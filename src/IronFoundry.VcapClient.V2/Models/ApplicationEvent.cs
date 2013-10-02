using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ApplicationEvent
    {
        [JsonProperty(PropertyName = "app_guid")]
        public Guid AppId { get; set; }

        [JsonProperty(PropertyName = "instance_guid")]
        public string InstanceId { get; set; }

        [JsonProperty(PropertyName = "instance_index")]
        public int InstanceIndex { get; set; }

        [JsonProperty(PropertyName = "exit_status")]
        public int ExitStatus { get; set; }

        [JsonProperty(PropertyName = "exit_description")]
        public string ExitDescription { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTimeOffset EventTime { get; set; }

        [JsonProperty(PropertyName = "app_url")]
        public string AppUrl { get; set; }
    }
}
