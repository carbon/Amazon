using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[method: JsonConstructor]
public sealed class ChangeMessageVisibilityBatchResultEntry(string id)
{
    [JsonPropertyName("Id")]
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));
}