using System.Text.Json.Serialization;

using Amazon.Kms.Exceptions;

namespace Amazon.Kms.Serialization;

[JsonSerializable(typeof(CreateAliasRequest))]
[JsonSerializable(typeof(CreateGrantRequest))]
[JsonSerializable(typeof(DecryptRequest))]
[JsonSerializable(typeof(DecryptResult))]
[JsonSerializable(typeof(EncryptRequest))]
[JsonSerializable(typeof(EncryptResult))]
[JsonSerializable(typeof(GenerateDataKeyRequest))]
[JsonSerializable(typeof(GenerateDataKeyResult))]
[JsonSerializable(typeof(ListGrantsRequest))]
[JsonSerializable(typeof(ListGrantsResult))]
[JsonSerializable(typeof(KmsError))]
public partial class KmsSerializerContext : JsonSerializerContext
{
}