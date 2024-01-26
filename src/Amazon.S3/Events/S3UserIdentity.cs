using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3UserIdentity
{
    [JsonPropertyName("principalId")]
    public required string PrincipalId { get; init; }
}