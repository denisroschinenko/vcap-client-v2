using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ServiceInstance
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "service_plan_guid")]
        public Guid ServicePlanId { get; set; }

        [JsonProperty(PropertyName = "space_guid")]
        public Guid SpaceId { get; set; }

        [JsonProperty(PropertyName = "dashboard_url")]
        public string DashboardUrl { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string ServiceInstanceType { get; set; }

        [JsonProperty(PropertyName = "space_url")]
        public string SpaceUrl { get; set; }

        [JsonProperty(PropertyName = "service_plan_url")]
        public string ServicePlanUrl { get; set; }

        [JsonProperty(PropertyName = "service_bindings_url")]
        public string ServiceBindingsUrl { get; set; }

        [JsonProperty(PropertyName = "credentials")]
        public Credentials CredentialsInfo { get; set; }

        #region Nested classes

        public class Credentials
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        public class GatewayData
        {
            [JsonProperty(PropertyName = "plan")]
            public string PlanName { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "options")]
            public Options OptionsInfo { get; set; }
        }

        public class Options
        {
        }

        #endregion
    }
}