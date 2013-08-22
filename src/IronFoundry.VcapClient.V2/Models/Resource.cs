using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class Resource<T>
    {
        [JsonProperty(PropertyName = "metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty(PropertyName = "entity")]
        public T Entity { get; set; }
    }
}