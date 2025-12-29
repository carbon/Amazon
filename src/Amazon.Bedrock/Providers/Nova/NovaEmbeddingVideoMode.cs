using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingVideoMode>))]
public enum NovaEmbeddingVideoMode
{
    [JsonStringEnumMemberName("AUDIO_VIDEO_COMBINED")]
    AudioVideoCombined = 1,

    [JsonStringEnumMemberName("AUDIO_VIDEO_SEPARATE")]
    AudioVideoSeperate = 2
}
