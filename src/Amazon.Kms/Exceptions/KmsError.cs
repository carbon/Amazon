#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kms
{
    public sealed class KmsError
    {
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}