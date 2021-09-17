using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum KeySpec
{
    AES_256 = 1,
    AES_128 = 2
}