using System.Text.Json.Serialization;

namespace Cohere;

public sealed class EmbedV4Request
{
    [JsonPropertyName("input_type")]
    public required EmbedV4InputType InputType { get; init; }

    /// <summary>
    /// Maximum number of texts per call is 96.
    /// For optimal performance, we recommend reducing the length of each text to less than 512 tokens. 1 token is about 4 characters.
    /// </summary>
    [JsonPropertyName("texts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Texts { get; init; }

    /// <summary>
    /// An array of image data URIs for the model to embed. 
    /// Maximum number of images per call is 1.
    /// </summary>
    [JsonPropertyName("images")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Images { get; init; }

    [JsonPropertyName("inputs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CohereInput[]? Inputs { get; init; }

    [JsonPropertyName("truncate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Cohere4Truncate Truncate { get; init; }

    [JsonPropertyName("embedding_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmbeddingType[]? EmbeddingTypes { get; set; }

    [JsonPropertyName("max_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("output_dimension")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? OutputDimension { get; set; }
}
