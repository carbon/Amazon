using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class ListKeysRequest : KmsRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Limit { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Marker { get; init; }
}