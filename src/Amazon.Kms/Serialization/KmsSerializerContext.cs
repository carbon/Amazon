using System.Text.Json.Serialization;

using Amazon.Kms.Exceptions;

namespace Amazon.Kms.Serialization;

[JsonSerializable(typeof(CreateGrantRequest))]
[JsonSerializable(typeof(EncryptRequest))]
[JsonSerializable(typeof(GenerateDataKeyResponse))]
[JsonSerializable(typeof(ListGrantsRequest))]
[JsonSerializable(typeof(ListGrantsResponse))]
[JsonSerializable(typeof(KmsError))]
public partial class KmsSerializerContext : JsonSerializerContext
{
}