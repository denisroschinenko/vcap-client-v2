using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "default_space_guid")]
        public Guid? DefaultSpaceId { get; set; }

        [JsonProperty(PropertyName = "spaces_url")]
        public string SpacesUrl { get; set; }

        [JsonProperty(PropertyName = "organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty(PropertyName = "managed_organizations_url")]
        public string ManagedOrganizationsUrl { get; set; }

        [JsonProperty(PropertyName = "billing_managed_organizations_url")]
        public string BillingManagedOrganizationsUrl { get; set; }

        [JsonProperty(PropertyName = "audited_organizations_url")]
        public string AuditedOrganizationsUrl { get; set; }

        [JsonProperty(PropertyName = "managed_spaces_url")]
        public string ManagedSpacesUrl { get; set; }

        [JsonProperty(PropertyName = "audited_spaces_url")]
        public string AuditedSpacesUrl { get; set; }
    }
}