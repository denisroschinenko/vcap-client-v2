using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ServiceBind
    {
        [JsonProperty(PropertyName = "app_guid")]
        public Guid ApplicationId { get; set; }

        [JsonProperty(PropertyName = "service_instance_guid")]
        public Guid ServiceInstanceId { get; set; }

        [JsonProperty(PropertyName = "credentials")]
        public Credentials CredentialsInfo { get; set; }

        [JsonProperty(PropertyName = "binding_options")]
        public BindingOptions BindingOptionsInfo { get; set; }

        [JsonProperty(PropertyName = "gateway_data")]
        public GatewayData GatewayInfo { get; set; }

        [JsonProperty(PropertyName = "gateway_name")]
        public string GatewayName { get; set; }

        [JsonProperty(PropertyName = "app_url")]
        public string ApplicationUrl { get; set; }

        [JsonProperty(PropertyName = "service_instance_url")]
        public string ServiceInstanceUrl { get; set; }

        #region Nested classes

        public class BindingOptions
        {
        }

        public class Credentials
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "jdbcUrl")]
            public string JdbcUrl { get; set; }

            [JsonProperty(PropertyName = "uri")]
            public Uri Url { get; set; }

            [JsonProperty(PropertyName = "hostname")]
            public string HostName { get; set; }

            [JsonProperty(PropertyName = "port")]
            public int Port { get; set; }

            [JsonProperty(PropertyName = "username")]
            public int Username { get; set; }

            [JsonProperty(PropertyName = "password")]
            public int Password { get; set; }
        }

        public class GatewayData
        {
            [JsonProperty(PropertyName = "data")]
            public GatewayDataObject DataObject { get; set; }
        }

        public class GatewayDataObject
        {
            [JsonProperty(PropertyName = "binding_options")]
            public BindingOptions BindingOptions { get; set; }
        }

        #endregion
    }

    public class ServiceBindManifest
    {
        [JsonProperty(PropertyName = "app_guid")]
        public Guid ApplicationId { get; set; }

        [JsonProperty(PropertyName = "service_instance_guid")]
        public Guid ServiceInstanceId { get; set; }
    }
}