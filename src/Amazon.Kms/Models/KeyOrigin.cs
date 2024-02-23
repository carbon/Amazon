using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter<KeyOrigin>))]
public enum KeyOrigin
{
    AWS_KMS,
    EXTERNAL,
    AWS_CLOUDHSM,
    EXTERNAL_KEY_STORE
}