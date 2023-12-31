using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class CreateAliasRequest : KmsRequest
{
    public CreateAliasRequest(string targetKeyId, string aliasName)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetKeyId);
        Ensure.LengthBetween(aliasName, 1, 256);

        TargetKeyId = targetKeyId;
        AliasName = aliasName;
    }

    [JsonPropertyName("TargetKeyId")]
    public string TargetKeyId { get; }

    [JsonPropertyName("AliasName")]
    public string AliasName { get; }
}