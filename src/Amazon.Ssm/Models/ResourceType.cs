using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResourceType
{
    ManagedInstance,
    Document,
    EC2Instance
}
