#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3EventObjectInfo
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("eTag")]
        public string ETag { get; set; }

        [JsonPropertyName("versionId")]
        public string VersionId { get; set; }

        [JsonPropertyName("sequencer")]
        public string Sequencer { get; set; }
    }
}