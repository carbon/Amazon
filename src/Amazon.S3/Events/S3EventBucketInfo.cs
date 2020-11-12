#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3EventBucketInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ownerIdentity")]
        public S3UserIdentity OwnerIdentity { get; set; }

        [JsonPropertyName("arn")]
        public string Arn { get; set; }
    }
}