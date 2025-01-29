using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class BedrockRerankingConfiguration
{
    [JsonPropertyName("modelConfiguration")]
    public required BedrockRerankingModelConfiguration ModelConfiguration { get; set; }

    [JsonPropertyName("numberOfResults")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumberOfResults { get; set; }

}
