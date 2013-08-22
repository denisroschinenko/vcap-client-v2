using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Event
    {
        [JsonProperty(PropertyName = "type")]
        public string TypeName { get; set; }

        [JsonProperty(PropertyName = "actor")]
        public Guid ActorId { get; set; }

        [JsonProperty(PropertyName = "actor_type")]
        public string ActorType { get; set; }

        [JsonProperty(PropertyName = "actee")]
        public Guid ActionId { get; set; }

        [JsonProperty(PropertyName = "actee_type")]
        public string ActionType { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTimeOffset TimeStamp { get; set; }

        [JsonProperty(PropertyName = "space_guid")]
        public Guid SpaceId { get; set; }

        [JsonProperty(PropertyName = "space_url")]
        public string SpaceUrl { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public EventMetadata MetadataEvent { get; set; }

        #region Nested classes
        public class EventMetadata
        {
            [JsonProperty(PropertyName = "changes")]
            public ChangedData ChangedData { get; set; }

            [JsonProperty(PropertyName = "footprints")]
            public FootPrint FootPrint { get; set; }

        }

        public class ChangedData
        {
            [JsonProperty(PropertyName = "environment_json")]
            public string EnvironmentInfo { get; set; }

            [JsonProperty(PropertyName = "instances")]
            public int NumberInstance { get; set; }

            [JsonProperty(PropertyName = "memory")]
            public long Memory { get; set; }

            [JsonProperty(PropertyName = "state")]
            public string State { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }


        public class FootPrint
        {
            [JsonProperty(PropertyName = "instances")]
            public int NumberInstance { get; set; }

            [JsonProperty(PropertyName = "memory")]
            public long Memory { get; set; }
        }
        #endregion
    }
}
