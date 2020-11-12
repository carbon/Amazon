#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3EventDetails
    {
        [JsonPropertyName("s3SchemaVersion")]
        public string S3SchemaVersion { get; set; }

        [JsonPropertyName("configurationId")]
        public string ConfigurationId { get; set; }

        [JsonPropertyName("bucket")]
        public S3EventBucketInfo Bucket { get; set; }

        [JsonPropertyName("object")]
        public S3EventObjectInfo Object { get; set; } 
    }
}
