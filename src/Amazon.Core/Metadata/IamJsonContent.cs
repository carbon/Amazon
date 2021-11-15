using System.Text.Json.Serialization;

namespace Amazon.Metadata;

[JsonSerializable(typeof(IamSecurityCredentials))]
internal partial class IamJsonContext : JsonSerializerContext
{
}