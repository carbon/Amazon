using System.Text.Json.Serialization;

namespace Cohere;

public sealed class EmbedV3Result
{
    [JsonPropertyName("embeddings")]
    public required List<float[]> Embeddings { get; init; }

    [JsonPropertyName("texts")]
    public required string[] Texts { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }
}


// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-embed.html

/*
{
    "embeddings": [
        [float //*1024)]
    ],
    "id": "string",
    "texts": ["string"]
}
*/