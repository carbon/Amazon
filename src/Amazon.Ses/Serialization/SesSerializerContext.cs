using System.Text.Json.Serialization;

namespace Amazon.Ses.Serialization;

[JsonSerializable(typeof(SesNotification))]
public partial class SesSerializerContext : JsonSerializerContext
{
}