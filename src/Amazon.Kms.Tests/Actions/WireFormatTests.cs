using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

// KmsClient has no methods for several of these types, and they are not in KmsSerializerContext,
// so these tests use the reflection based serializer
public class WireFormatTests
{
    [Fact]
    public void CreateKeyOmitsUnsetFields()
    {
        Assert.Equal("{}", JsonSerializer.Serialize(new CreateKeyRequest()));
    }

    [Fact]
    public void CreateKey()
    {
        var json = JsonSerializer.SerializeToElement(new CreateKeyRequest {
            BypassPolicyLockoutSafetyCheck = false,
            CustomKeyStoreId = "store",
            Description = "description",
            KeySpec = KeySpec.RSA_2048,
            KeyUsage = "SIGN_VERIFY",
            MultiRegion = true,
            Origin = KeyOrigin.AWS_KMS,
            Policy = "policy",
            XksKeyId = "xks"
        });

        Assert.False(json.GetProperty("BypassPolicyLockoutSafetyCheck").GetBoolean());
        Assert.Equal("store", json.GetProperty("CustomKeyStoreId").GetString());
        Assert.Equal("description", json.GetProperty("Description").GetString());
        Assert.Equal("RSA_2048", json.GetProperty("KeySpec").GetString());
        Assert.Equal("SIGN_VERIFY", json.GetProperty("KeyUsage").GetString());
        Assert.True(json.GetProperty("MultiRegion").GetBoolean());
        Assert.Equal("policy", json.GetProperty("Policy").GetString());
        Assert.Equal("xks", json.GetProperty("XksKeyId").GetString());

        // AWS_KMS is the zero value of the enum and must still be sent
        Assert.Equal("AWS_KMS", json.GetProperty("Origin").GetString());
    }

    [Fact]
    public void ListKeys()
    {
        Assert.Equal("{}", JsonSerializer.Serialize(new ListKeysRequest()));

        Assert.Equal("""{"Limit":5,"Marker":"m"}""", JsonSerializer.Serialize(new ListKeysRequest { Limit = 5, Marker = "m" }));
    }

    [Fact]
    public void GenerateMac()
    {
        var json = JsonSerializer.SerializeToElement(new GenerateMacRequest("key", MacAlgorithm.HMAC_SHA_384, [0xFB, 0xFF, 0xBF]) {
            GrantTokens = ["token"],
            DryRun = true
        });

        Assert.Equal("key", json.GetProperty("KeyId").GetString());
        Assert.Equal("HMAC_SHA_384", json.GetProperty("MacAlgorithm").GetString());
        Assert.Equal("+/+/", json.GetProperty("Message").GetString());
        Assert.Equal("token", json.GetProperty("GrantTokens")[0].GetString());
        Assert.True(json.GetProperty("DryRun").GetBoolean());
    }

    [Fact]
    public void GenerateMacInitializer()
    {
        var request = new GenerateMacRequest {
            KeyId = "key",
            MacAlgorithm = MacAlgorithm.HMAC_SHA_224,
            Message = [1]
        };

        Assert.Equal("""{"KeyId":"key","MacAlgorithm":"HMAC_SHA_224","Message":"AQ=="}""", JsonSerializer.Serialize(request));
    }

    [Fact]
    public void VerifyInitializer()
    {
        var request = new VerifyRequest {
            KeyId = "key",
            Message = [1],
            Signature = [2],
            SigningAlgorithm = SigningAlgorithm.SM2DSA
        };

        Assert.Equal(
            """{"KeyId":"key","Message":"AQ==","Signature":"Ag==","SigningAlgorithm":"SM2DSA"}""",
            JsonSerializer.Serialize(request, KmsSerializerContext.Default.VerifyRequest));
    }

    [Fact]
    public void ListGrantsInitializer()
    {
        Assert.Equal("""{"KeyId":"key"}""", JsonSerializer.Serialize(new ListGrantsRequest { KeyId = "key" }, KmsSerializerContext.Default.ListGrantsRequest));
    }

    [Fact]
    public void DecryptSendsRecipientAsObject()
    {
        var request = new DecryptRequest("key", [1], null) {
            Recipient = new RecipientInfo {
                AttestationDocument = [0xFB, 0xFF, 0xBF],
                KeyEncryptionAlgorithm = "RSAES_OAEP_SHA_256"
            }
        };

        var recipient = JsonSerializer.SerializeToElement(request, KmsSerializerContext.Default.DecryptRequest).GetProperty("Recipient");

        Assert.Equal("+/+/", recipient.GetProperty("AttestationDocument").GetString());
        Assert.Equal("RSAES_OAEP_SHA_256", recipient.GetProperty("KeyEncryptionAlgorithm").GetString());
    }

    [Fact]
    public void GenerateDataKeyResultExposesCiphertextForRecipient()
    {
        var result = JsonSerializer.Deserialize("""{"KeyId":"key","CiphertextBlob":"AQ==","CiphertextForRecipient":"+/+/"}""", KmsSerializerContext.Default.GenerateDataKeyResult);

        Assert.Equal([0xFB, 0xFF, 0xBF], result!.CiphertextForRecipient);
        Assert.Null(result.Plaintext);
    }

    [Fact]
    public void DecryptResultAllowsMissingPlaintextForRecipient()
    {
        var result = JsonSerializer.Deserialize("""{"KeyId":"key","CiphertextForRecipient":"AQ=="}""", KmsSerializerContext.Default.DecryptResult);

        Assert.Equal([1], result!.CiphertextForRecipient);
        Assert.Null(result.Plaintext);
    }

    [Fact]
    public void GenerateDataKeyWithoutPlaintextResult()
    {
        var result = JsonSerializer.Deserialize<GenerateDataKeyWithoutPlaintextResult>("""{"KeyId":"key","CiphertextBlob":"AQ=="}""");

        Assert.Equal("key", result!.KeyId);
        Assert.Equal([1], result.CiphertextBlob);
    }

    [Fact]
    public void KeyListEntry()
    {
        var result = JsonSerializer.Deserialize<KeyListEntry>("""{"KeyArn":"arn","KeyId":"key"}""");

        Assert.Equal("arn", result!.KeyArn);
        Assert.Equal("key", result.KeyId);
    }

    [Fact]
    public void KeyMetadata()
    {
        var result = JsonSerializer.Deserialize<KeyMetadata>(
            """{"KeyId":"key","Arn":"arn","AWSAccountId":"1","CloudHsmClusterId":"c","CustomKeyStoreId":"s","Description":"d","Enabled":true,"EncryptionAlgorithms":["SYMMETRIC_DEFAULT","SM2PKE"],"ExpirationModel":"e","KeyManager":"CUSTOMER","KeySpec":"SYMMETRIC_DEFAULT","KeyState":"PendingDeletion","MultiRegion":true,"Origin":"AWS_KMS"}""");

        Assert.Equal("key", result!.KeyId);
        Assert.Equal("arn", result.Arn);
        Assert.Equal("1", result.AWSAccountId);
        Assert.Equal("c", result.CloudHsmClusterId);
        Assert.Equal("s", result.CustomKeyStoreId);
        Assert.Equal("d", result.Description);
        Assert.True(result.Enabled);
        Assert.Equal([EncryptionAlgorithm.SYMMETRIC_DEFAULT, EncryptionAlgorithm.SM2PKE], result.EncryptionAlgorithms);
        Assert.Equal("e", result.ExpirationModel);
        Assert.Equal(KeyManager.CUSTOMER, result.KeyManager);
        Assert.Equal(KeySpec.SYMMETRIC_DEFAULT, result.KeySpec);
        Assert.Equal(KeyState.PendingDeletion, result.KeyState);
        Assert.True(result.MultiRegion);
        Assert.Equal("AWS_KMS", result.Origin);
    }
}
