using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter<KeyState>))]
public enum KeyState
{
    Creating,
    Enabled,
    Disabled,
    PendingDeletion,
    PendingImport,
    PendingReplicaDeletion,
    Unavailable,
    Updating
}
