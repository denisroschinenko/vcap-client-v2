using System;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Application
    {
        internal static class ApplicationStates
        {
            public const string Stopped = "STOPPED";
            public const string Started = "STARTED";
            public const string Restarted = "RESTARTED";
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "production")]
        public bool Production { get; set; }

        [JsonProperty(PropertyName = "space_guid")]
        public Guid SpaceGuid { get; set; }

        [JsonProperty(PropertyName = "stack_guid")]
        public Guid StackGuid { get; set; }

        [JsonProperty(PropertyName = "buildpack")]
        public string Buildpack { get; set; }

        [JsonProperty(PropertyName = "detected_buildpack")]
        public string DetectedBuildpack { get; set; }

        [JsonProperty(PropertyName = "environment_json")]
        public object EnvironmentInfo { get; set; }

        [JsonProperty(PropertyName = "memory")]
        public long Memory { get; set; }

        [JsonProperty(PropertyName = "instances")]
        public int NumberInstance { get; set; }

        [JsonProperty(PropertyName = "disk_quota")]
        public long DiskQuota { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "version")]
        public Guid Version { get; set; }

        [JsonProperty(PropertyName = "command")]
        public string Command { get; set; }

        [JsonProperty(PropertyName = "console")]
        public bool IsConsole { get; set; }

        [JsonProperty(PropertyName = "debug")]
        public string Debug { get; set; }

        [JsonProperty(PropertyName = "staging_task_id")]
        public string StagingTaskId { get; set; }

        [JsonProperty(PropertyName = "space_url")]
        public string SpaceUrl { get; set; }

        [JsonProperty(PropertyName = "stack_url")]
        public string StackUrl { get; set; }

        [JsonProperty(PropertyName = "service_bindings_url")]
        public string ServiceBindingsUrl { get; set; }

        [JsonProperty(PropertyName = "routes_url")]
        public string RoutesUrl { get; set; }

        [JsonProperty(PropertyName = "events_url")]
        public string EventsUrl { get; set; }
    }

    internal class ApplicationManifest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "space_guid")]
        public Guid SpaceGuid { get; set; }

        [JsonProperty(PropertyName = "stack_guid")]
        public Guid StackGuid { get; set; }

        [JsonProperty(PropertyName = "memory")]
        public long Memory { get; set; }

        [JsonProperty(PropertyName = "instances")]
        public int NumberInstance { get; set; }
    }
}
