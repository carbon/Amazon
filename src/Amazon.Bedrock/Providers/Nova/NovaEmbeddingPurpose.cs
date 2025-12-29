using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingPurpose>))]
public enum NovaEmbeddingPurpose
{
    [JsonStringEnumMemberName("GENERIC_INDEX")]
    GenericIndex = 1,

    [JsonStringEnumMemberName("GENERIC_RETRIEVAL")]
    GenericRetrieval = 2,

    [JsonStringEnumMemberName("TEXT_RETRIEVAL")]
    TextRetrieval = 3,

    [JsonStringEnumMemberName("IMAGE_RETRIEVAL")]
    ImageRetrieval = 4,

    [JsonStringEnumMemberName("VIDEO_RETRIEVAL")]
    VideoRetrieval = 5,

    [JsonStringEnumMemberName("DOCUMENT_RETRIEVAL")]
    DocumentRetrieval = 6,

    [JsonStringEnumMemberName("AUDIO_RETRIEVAL")]
    AudioRetrieval = 7,

    [JsonStringEnumMemberName("CLASSIFICATION")]
    Classification = 8,

    [JsonStringEnumMemberName("CLUSTERING")]
    Clustering = 9
}