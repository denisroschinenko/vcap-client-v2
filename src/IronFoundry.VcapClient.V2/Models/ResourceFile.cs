using Newtonsoft.Json;

namespace IronFoundry.VcapClient.V2.Models
{
    public class ResourceFile
    {
        [JsonProperty(PropertyName="size")]
        public ulong Size { get; private set; }

        [JsonProperty(PropertyName="sha1")]
        public string SHA1 { get; private set; }

        [JsonProperty(PropertyName="fn")]
        public string FN { get; private set; }

        public ResourceFile(ulong argSize, string argSha1, string argFN)
        {
            Size = argSize;
            SHA1 = argSha1;
            FN   = argFN;
        }

        public bool Equals(ResourceFile other)
        {
            if (null == other)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ResourceFile);
        }

        public override int GetHashCode()
        {
            return SHA1.GetHashCode();
        }
    }
}
