namespace Amazon.Kms.Actions.Tests;

public class RequestValidationTests
{
    [Fact]
    public void EncryptValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>("keyId", () => new EncryptRequest(null!, [1]));
        Assert.Throws<ArgumentException>("keyId", () => new EncryptRequest("", [1]));
        Assert.Throws<ArgumentNullException>("plaintext", () => new EncryptRequest("key", null!));
    }

    [Fact]
    public void EncryptAllowsUpTo4096Bytes()
    {
        Assert.Equal(4096, new EncryptRequest("key", new byte[4096]).Plaintext.Length);

        Assert.Throws<ArgumentException>("plaintext", () => new EncryptRequest("key", new byte[4097]));
    }

    [Fact]
    public void DecryptValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>("keyId", () => new DecryptRequest(null!, [1], null));
        Assert.Throws<ArgumentException>("keyId", () => new DecryptRequest("", [1], null));
        Assert.Throws<ArgumentNullException>("ciphertext", () => new DecryptRequest("key", null!, null));
        Assert.Throws<ArgumentException>("ciphertext", () => new DecryptRequest("key", [], null));
    }

    [Fact]
    public void DecryptAllowsUpTo6144Bytes()
    {
        Assert.Equal(6144, new DecryptRequest("key", new byte[6144], null).CiphertextBlob.Length);

        var ex = Assert.Throws<ArgumentException>("ciphertext", () => new DecryptRequest("key", new byte[6145], null));

        Assert.Contains("6145", ex.Message);
    }

    [Fact]
    public void CreateAliasValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>("targetKeyId", () => new CreateAliasRequest(null!, "alias/a"));
        Assert.Throws<ArgumentException>("targetKeyId", () => new CreateAliasRequest("", "alias/a"));
        Assert.Throws<ArgumentNullException>("argument", () => new CreateAliasRequest("key", null!));
        Assert.Throws<ArgumentException>("aliasName", () => new CreateAliasRequest("key", ""));
    }

    [Fact]
    public void CreateAliasAllowsUpTo256Characters()
    {
        Assert.Equal(256, new CreateAliasRequest("key", new string('a', 256)).AliasName.Length);

        var ex = Assert.Throws<ArgumentException>("aliasName", () => new CreateAliasRequest("key", new string('a', 257)));

        Assert.Contains("Was 257.", ex.Message);
    }

    [Fact]
    public void CreateAliasRejectsLongNameContainingBraces()
    {
        Assert.Throws<ArgumentException>("aliasName", () => new CreateAliasRequest("key", new string('{', 257)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void RetireGrantRejectsMissingValues(string? value)
    {
        Assert.ThrowsAny<ArgumentException>(() => new RetireGrantRequest(value!));
        Assert.ThrowsAny<ArgumentException>(() => new RetireGrantRequest(value!, "grant"));
        Assert.ThrowsAny<ArgumentException>(() => new RetireGrantRequest("key", value!));
    }

    [Fact]
    public void RevokeGrantRejectsNullKey()
    {
        Assert.Throws<ArgumentNullException>("keyId", () => new RevokeGrantRequest(null!, "grant"));
    }

    [Fact]
    public void RevokeGrantRejectsNullGrant()
    {
        Assert.Throws<ArgumentNullException>(() => new RevokeGrantRequest("key", null!));
    }

    [Fact]
    public void RevokeGrantNamesGrantParameter()
    {
        Assert.Throws<ArgumentNullException>("grantId", () => new RevokeGrantRequest("key", null!));
    }

    [Fact]
    public void GenerateDataKeyValidatesKey()
    {
        Assert.Throws<ArgumentNullException>("keyId", () => new GenerateDataKeyRequest(null!, KeySpec.AES_256, null));
        Assert.Throws<ArgumentException>("keyId", () => new GenerateDataKeyRequest("", KeySpec.AES_256, null));
    }

    [Fact]
    public void ListGrantsValidatesKey()
    {
        Assert.Throws<ArgumentNullException>("keyId", () => new ListGrantsRequest(null!));
        Assert.Throws<ArgumentException>("keyId", () => new ListGrantsRequest(""));
    }

    [Fact]
    public void EnableKeyValidatesKey()
    {
        Assert.Equal("key", new EnableKeyRequest("key").KeyId);

        Assert.Throws<ArgumentNullException>("keyId", () => new EnableKeyRequest(null!));
        Assert.Throws<ArgumentException>("keyId", () => new EnableKeyRequest(""));
    }

    [Fact]
    public void DisableKeyValidatesKey()
    {
        Assert.Equal("key", new DisableKeyRequest("key").KeyId);

        Assert.Throws<ArgumentNullException>("keyId", () => new DisableKeyRequest(null!));
        Assert.Throws<ArgumentException>("keyId", () => new DisableKeyRequest(""));
    }

    [Fact]
    public void CancelKeyDeletionValidatesKey()
    {
        Assert.Equal("key", new CancelKeyDeletionRequest("key").KeyId);

        Assert.Throws<ArgumentNullException>("keyId", () => new CancelKeyDeletionRequest(null!));
    }

    [Fact]
    public void GenerateMacValidatesMessage()
    {
        Assert.Throws<ArgumentNullException>("message", () => new GenerateMacRequest("key", MacAlgorithm.HMAC_SHA_256, null!));
    }
}
