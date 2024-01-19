using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class SignRequest : KmsRequest
{ 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; init; }

    public required byte[] Message { get; init; }

    public required SigningAlgorithm SigningAlgorithm { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KeyId { get; init; }

    // RAW | DIGEST
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MessageType { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DryRun { get; init; }
}