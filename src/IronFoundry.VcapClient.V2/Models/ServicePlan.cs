using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ServicePlan
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "free")]
        public bool IsFree { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "service_guid")]
        public Guid ServiceId { get; set; }

        [JsonProperty(PropertyName = "extra")]
        public string ExtraInfo { get; set; }

        [JsonProperty(PropertyName = "unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty(PropertyName = "service_url")]
        public string ServiceUrl { get; set; }

        [JsonProperty(PropertyName = "service_instances_url")]
        public string ServiceInstancesUrl { get; set; }
    }
}