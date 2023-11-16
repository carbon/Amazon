using System.Text.Json.Serialization;

namespace Amazon.Metadata.Serialization;

[JsonSerializable(typeof(IamSecurityCredentials))]
[JsonSerializable(typeof(InstanceAction))]
[JsonSerializable(typeof(InstanceIdentity))]
internal partial class MetadataSerializerContext : JsonSerializerContext
{
}