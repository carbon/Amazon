using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter<MacAlgorithm>))]
public enum MacAlgorithm
{
    HMAC_SHA_224 = 1,
    HMAC_SHA_256 = 2,
    HMAC_SHA_384 = 3,
    HMAC_SHA_512 = 4
}
