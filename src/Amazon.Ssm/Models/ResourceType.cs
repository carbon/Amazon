using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter<ResourceType>))]
public enum ResourceType
{
    ManagedInstance,
    Document,
    EC2Instance
}
