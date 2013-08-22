using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Metadata
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid ObjectId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTimeOffset? UpdatedDateTime { get; set; }
    }
}