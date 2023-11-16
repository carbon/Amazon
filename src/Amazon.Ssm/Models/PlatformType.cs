using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter<PlatformType>))]
public enum PlatformType
{
    Windows,
    Linux
}
