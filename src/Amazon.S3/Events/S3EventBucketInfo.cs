#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3EventBucketInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("ownerIdentity")]
        public S3UserIdentity OwnerIdentity { get; init; }

        [JsonPropertyName("arn")]
        public string Arn { get; init; }
    }
}