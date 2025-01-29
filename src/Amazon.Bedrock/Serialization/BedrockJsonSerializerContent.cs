using System.Text.Json.Serialization;

using Amazon.Bedrock.Actions;

namespace Amazon.Bedrock.Serialization;

[JsonSerializable(typeof(ConverseRequest))]
[JsonSerializable(typeof(RerankRequest))]
public partial class BedrockJsonSerializerContent : JsonSerializerContext
{
}