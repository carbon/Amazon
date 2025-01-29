using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankingConfiguration
{
    [JsonPropertyName("bedrockRerankingConfiguration")]
    public required BedrockRerankingConfiguration BedrockRerankingConfiguration { get; init; }

    // BEDROCK_RERANKING_MODEL
    [JsonPropertyName("type")]
    public required string Type { get; init; }


    public static implicit operator RerankingConfiguration(BedrockRerankingConfiguration configuration)
    {
        return new RerankingConfiguration {
            BedrockRerankingConfiguration = configuration, 
            Type = "BEDROCK_RERANKING_MODEL"
        };
    }
}
