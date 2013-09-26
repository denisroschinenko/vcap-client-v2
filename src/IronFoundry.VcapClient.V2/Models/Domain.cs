using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Domain
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "owning_organization_guid")]
        public Guid? OwningOrganizationId { get; set; }

        [JsonProperty(PropertyName = "wildcard")]
        public bool IsWilcardExist { get; set; }

        [JsonProperty(PropertyName = "owning_organization_url")]
        public string OwningOrganizationUrl { get; set; }

        [JsonProperty(PropertyName = "spaces_url")]
        public string SpacesUrl { get; set; }
    }

    internal class DomainManifest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "owning_organization_guid")]
        public Guid? OwningOrganizationId { get; set; }

        [JsonProperty(PropertyName = "wildcard")]
        public bool IsWilcardExist { get; set; }
    }
}