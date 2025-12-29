using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingType>))]
public enum NovaEmbeddingType
{
    [JsonStringEnumMemberName("TEXT")]
    Text = 1,

    [JsonStringEnumMemberName("IMAGE")]
    Image = 2,

    [JsonStringEnumMemberName("VIDEO")]
    Video = 3,

    [JsonStringEnumMemberName("AUDIO")]
    Audio = 4,

    [JsonStringEnumMemberName("AUDIO_VIDEO_COMBINED")]
    AudioVideoCombined = 5,
}
