using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingImageDetail>))]
public enum NovaEmbeddingImageDetail
{
    [JsonStringEnumMemberName("STANDARD_IMAGE")]
    StandardImage = 1,

    [JsonStringEnumMemberName("DOCUMENT_IMAGE")]
    DocumentImage = 2
}
