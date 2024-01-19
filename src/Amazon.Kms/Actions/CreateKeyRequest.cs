using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class CreateKeyRequest : KmsRequest 
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? BypassPolicyLockoutSafetyCheck { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CustomKeyStoreId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public KeySpec? KeySpec { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KeyUsage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool MultiRegion { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public KeyOrigin? Origin { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Policy { get; init; }

    // Tags

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? XksKeyId { get; init; }
}