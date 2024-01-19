using System.Text.Json.Serialization;

using Amazon.Kms.Exceptions;

namespace Amazon.Kms.Serialization;

[JsonSerializable(typeof(CreateAliasRequest))]
[JsonSerializable(typeof(CreateAliasResult))]
[JsonSerializable(typeof(CreateGrantRequest))]
[JsonSerializable(typeof(CreateGrantResult))]
[JsonSerializable(typeof(DecryptRequest))]
[JsonSerializable(typeof(DecryptResult))]
[JsonSerializable(typeof(EncryptRequest))]
[JsonSerializable(typeof(EncryptResult))]
[JsonSerializable(typeof(GenerateDataKeyRequest))]
[JsonSerializable(typeof(GenerateDataKeyResult))]
[JsonSerializable(typeof(ListGrantsRequest))]
[JsonSerializable(typeof(ListGrantsResult))]
[JsonSerializable(typeof(KmsError))]
[JsonSerializable(typeof(RetireGrantRequest))]
[JsonSerializable(typeof(SignRequest))]
[JsonSerializable(typeof(SignResult))]
[JsonSerializable(typeof(VerifyRequest))]
[JsonSerializable(typeof(VerifyResult))]

public partial class KmsSerializerContext : JsonSerializerContext
{
}