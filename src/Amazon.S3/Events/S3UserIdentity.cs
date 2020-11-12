#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3UserIdentity
    {
        [JsonPropertyName("principalId")]
        public string PrincipalId { get; set; }
    }
}