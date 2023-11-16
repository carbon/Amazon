#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[method: JsonConstructor]
public readonly struct DeleteMessageBatchResultEntry(string id)
{
    [JsonPropertyName("Id")]
    public string Id { get; } = id;
}