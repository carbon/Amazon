using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class CreateGrantResult : KmsResult
{
    [JsonPropertyName("GrantId")]
    public required string GrantId { get; init; }

    [JsonPropertyName("GrantToken")]
    public required string GrantToken { get; init; }
}