using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter<KeyManager>))]
public enum KeyManager
{
    AWS = 1,
    CUSTOMER = 2
}