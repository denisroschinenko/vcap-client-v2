using System.Collections.Generic;
using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ResponseData<T>
    {
        [JsonProperty(PropertyName = "total_results")]
        public int TotalResult { get; set; }

        [JsonProperty(PropertyName = "total_pages")]
        public int TotalPage { get; set; }

        [JsonProperty(PropertyName = "prev_url")]
        public string PreviousUrl { get; set; }

        [JsonProperty(PropertyName = "next_url")]
        public string NextUrl { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public IEnumerable<Resource<T>> Resources { get; set; }
    }
}