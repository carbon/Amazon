using System.Text.Json.Serialization;

namespace Cohere;

[JsonConverter(typeof(JsonStringEnumConverter<EmbeddingType>))]
public enum EmbeddingType
{
    [JsonStringEnumMemberName("float")]
    Float = 1,

    [JsonStringEnumMemberName("int8")]
    Int8 = 2,

    [JsonStringEnumMemberName("uint8")]
    Uint8 = 3,

    [JsonStringEnumMemberName("binary")]
    Binary = 4,

    [JsonStringEnumMemberName("ubinary")]
    UBinary = 5
}