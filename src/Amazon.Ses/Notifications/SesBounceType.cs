using System.Text.Json.Serialization;

namespace Amazon.Ses;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SesBounceType
{
    Undetermined = 1,
    Permanent    = 2,
    Transient    = 3,
}