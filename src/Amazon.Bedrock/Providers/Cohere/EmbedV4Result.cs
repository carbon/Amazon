using System.Text.Json.Serialization;

namespace Cohere;

public sealed class EmbedV4Result
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("embeddings")]
    public required EmbeddingsByType Embeddings { get; init; }

    [JsonPropertyName("texts")]
    public required string[] Texts { get; init; }

    // embeddings_floats
    // embeddings_by_type
    [JsonPropertyName("response_type")]
    public required string ResponseType { get; init; }
}

public class EmbeddingsByType
{
    [JsonPropertyName("float")]
    public List<float[]> Float { get; init; }

    [JsonPropertyName("int8")]
    public List<sbyte[]> Int8 { get; init; }

    [JsonPropertyName("uint8")]
    public List<byte[]> UInt8 { get; init; }

    /*
    [JsonPropertyName("binary")]
    public List<byte[]> Binary { get; init; }

    [JsonPropertyName("ubinary")]
    public List<byte[]> UBinary { get; init; }
    */
}


// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-embed-v4.html