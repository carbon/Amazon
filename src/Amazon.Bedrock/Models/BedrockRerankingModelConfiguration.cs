using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class BedrockRerankingModelConfiguration
{
    [JsonPropertyName("modelArn")]
    public required string ModelArn { get; set; }

    [JsonPropertyName("additionalModelRequestFields")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? AdditionalModelRequestFields { get; set; }
}