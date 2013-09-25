using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Space
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "organization_guid")]
        public Guid OrganizationId { get; set; }

        [JsonProperty(PropertyName = "organization_url")]
        public string OrganizationUrl { get; set; }

        [JsonProperty(PropertyName = "developers_url")]
        public string DevelopersUrl { get; set; }

        [JsonProperty(PropertyName = "managers_url")]
        public string ManagersUrl { get; set; }

        [JsonProperty(PropertyName = "auditors_url")]
        public string AuditorsUrl { get; set; }

        [JsonProperty(PropertyName = "apps_url")]
        public string AppsUrl { get; set; }

        [JsonProperty(PropertyName = "domains_url")]
        public string DomainsUrl { get; set; }

        [JsonProperty(PropertyName = "service_instances_url")]
        public string ServiceInstancesUrl { get; set; }

        [JsonProperty(PropertyName = "app_events_url")]
        public string AppEventsUrl { get; set; }
    }


    public class SpaceManifest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "organization_guid")]
        public Guid OrganizationId { get; set; }

    }
}