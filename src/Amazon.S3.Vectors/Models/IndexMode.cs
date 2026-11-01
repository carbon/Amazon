using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

[JsonConverter(typeof(JsonStringEnumConverter<IndexMode>))]
public enum IndexMode
{
    [JsonStringEnumMemberName("CLASSIC")]
    Classic = 1,

    [JsonStringEnumMemberName("ENHANCED")]
    Enhanced = 2
}
