using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Route
    {
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        [JsonProperty(PropertyName = "domain_guid")]
        public Guid DomainId { get; set; }

        [JsonProperty(PropertyName = "space_guid")]
        public Guid SpaceId { get; set; }

        [JsonProperty(PropertyName = "domain_url")]
        public string DomainUrl { get; set; }

        [JsonProperty(PropertyName = "space_url")]
        public string SpaceUrl { get; set; }

        [JsonProperty(PropertyName = "apps_url")]
        public string ApplicationUrl { get; set; }
    }
}