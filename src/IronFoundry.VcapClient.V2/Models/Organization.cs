using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Organization
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "billing_enabled")]
        public bool IsEnabledBilling { get; set; }

        [JsonProperty(PropertyName = "quota_definition_guid")]
        public Guid QuotaDefinitionId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "quota_definition_url")]
        public string QuotaDefinitionUrl { get; set; }

        [JsonProperty(PropertyName = "spaces_url")]
        public string SpacesUrl { get; set; }

        [JsonProperty(PropertyName = "domains_url")]
        public string DomainsUrl { get; set; }

        [JsonProperty(PropertyName = "users_url")]
        public string UsersUrl { get; set; }

        [JsonProperty(PropertyName = "managers_url")]
        public string ManagersUrl { get; set; }

        [JsonProperty(PropertyName = "billing_managers_url")]
        public string BillingManagersUrl { get; set; }

        [JsonProperty(PropertyName = "auditors_url")]
        public string AuditorsUrl { get; set; }

        [JsonProperty(PropertyName = "app_events_url")]
        public string AppEventssUrl { get; set; }
    }

    public class OrganizationManifest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}