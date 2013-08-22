using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Info
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "build")]
        public ushort BuildNumber { get; set; }

        [JsonProperty(PropertyName = "support")]
        public Uri SupportUrl { get; set; }

        [JsonProperty(PropertyName = "version")]
        public ushort Version { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "authorization_endpoint")]
        public Uri AuthorizationUrl { get; set; }

        [JsonProperty(PropertyName = "api_version")]
        public string ApiVersion { get; set; }

        [JsonProperty(PropertyName = "user")]
        public Guid UserId { get; set; }
    }
}