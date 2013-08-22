using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Service
    {
        [JsonProperty(PropertyName = "label")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "provider")]
        public string Provider { get; set; }

        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "info_url")]
        public string InfoUrl { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "bindable")]
        public bool IsBindable { get; set; }

        [JsonProperty(PropertyName = "unique_id")]
        public int UniqueId { get; set; }

        [JsonProperty(PropertyName = "extra")]
        public string ExtraInfo { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<string> Tags { get; set; }

        [JsonProperty(PropertyName = "service_plans_url")]
        public string ServicePlansUrl { get; set; }
    }
}