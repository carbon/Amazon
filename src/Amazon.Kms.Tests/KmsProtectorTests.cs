using System.Collections.ObjectModel;
using System.Text.Json;

namespace Amazon.Kms.Tests;

public class KmsProtectorTests
{
    private const string EncryptResponse = """{"CiphertextBlob":"AQI=","KeyId":"arn"}""";
    private const string DecryptResponse = """{"Plaintext":"AwQ=","KeyId":"arn"}""";

    private static KmsProtector Create(FakeKmsHandler handler) => new(FakeKms.CreateClient(handler), "key");

    [Fact]
    public void ConstructorValidatesArguments()
    {
        var client = FakeKms.CreateClient(new FakeKmsHandler());
        var credential = new AwsCredential("public", "private");

        Assert.Throws<ArgumentNullException>("client", () => new KmsProtector(null!, "key"));
        Assert.Throws<ArgumentNullException>("keyId", () => new KmsProtector(client, null!));
        Assert.Throws<ArgumentNullException>("keyId", () => new KmsProtector(AwsRegion.USEast1, null!, credential));
        Assert.Throws<ArgumentNullException>(() => new KmsProtector(AwsRegion.USEast1, "key", null!));

        Assert.NotNull(new KmsProtector(AwsRegion.USEast1, "key", credential));
    }

    [Fact]
    public async Task EncryptReturnsCiphertext()
    {
        var handler = new FakeKmsHandler(EncryptResponse);

        byte[] ciphertext = await Create(handler).EncryptAsync([1]);

        Assert.Equal("TrentService.Encrypt", handler.Request.Target);
        Assert.Equal("""{"KeyId":"key","Plaintext":"AQ=="}""", handler.Request.Body);
        Assert.Equal([1, 2], ciphertext);
    }

    [Fact]
    public async Task DecryptReturnsPlaintext()
    {
        var handler = new FakeKmsHandler(DecryptResponse);

        byte[] plaintext = await Create(handler).DecryptAsync([1]);

        Assert.Equal("TrentService.Decrypt", handler.Request.Target);
        Assert.Equal("""{"KeyId":"key","CiphertextBlob":"AQ=="}""", handler.Request.Body);
        Assert.Equal([3, 4], plaintext);
    }

    [Fact]
    public async Task SendsReadOnlyDictionaryContext()
    {
        var handler = new FakeKmsHandler(EncryptResponse);

        await Create(handler).EncryptAsync([1], new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { "a", "b" } }));

        Assert.Equal("b", handler.Request.Json.GetProperty("EncryptionContext").GetProperty("a").GetString());
    }

    [Fact]
    public async Task SendsKeyValuePairContext()
    {
        var handler = new FakeKmsHandler(DecryptResponse);

        await Create(handler).DecryptAsync([1], [KeyValuePair.Create("a", "b"), KeyValuePair.Create("A", "c")]);

        JsonElement context = handler.Request.Json.GetProperty("EncryptionContext");

        Assert.Equal("b", context.GetProperty("a").GetString());
        Assert.Equal("c", context.GetProperty("A").GetString());
    }

    [Fact]
    public async Task EmptyContextIsNotDropped()
    {
        var handler = new FakeKmsHandler(EncryptResponse);

        await Create(handler).EncryptAsync([1], []);

        Assert.Equal(JsonValueKind.Object, handler.Request.Json.GetProperty("EncryptionContext").ValueKind);
    }

    [Fact]
    public async Task DuplicateContextKeysThrowBeforeSending()
    {
        var handler = new FakeKmsHandler(EncryptResponse);

        await Assert.ThrowsAsync<ArgumentException>(() => Create(handler).EncryptAsync([1], [KeyValuePair.Create("a", "b"), KeyValuePair.Create("a", "c")]));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task OversizedInputThrowsBeforeSending()
    {
        var handler = new FakeKmsHandler(EncryptResponse);

        await Assert.ThrowsAsync<ArgumentException>(() => Create(handler).EncryptAsync(new byte[4097]));
        await Assert.ThrowsAsync<ArgumentException>(() => Create(handler).DecryptAsync(new byte[6145]));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task GenerateKeyRequestsAes256()
    {
        var handler = new FakeKmsHandler("""{"CiphertextBlob":"AQI=","KeyId":"arn","Plaintext":"AwQ="}""");

        GenerateDataKeyResult result = await Create(handler).GenerateKeyAsync([KeyValuePair.Create("a", "b")]);

        Assert.Equal("TrentService.GenerateDataKey", handler.Request.Target);
        Assert.Equal("""{"KeyId":"key","EncryptionContext":{"a":"b"},"KeySpec":"AES_256"}""", handler.Request.Body);
        Assert.Equal([1, 2], result.CiphertextBlob);
        Assert.Equal([3, 4], result.Plaintext);
    }

    [Fact]
    public async Task RetireGrantUsesProtectorKey()
    {
        var handler = new FakeKmsHandler("");

        await Create(handler).RetireGrantAsync("grant");

        Assert.Equal("TrentService.RetireGrant", handler.Request.Target);
        Assert.Equal("""{"KeyId":"key","GrantId":"grant"}""", handler.Request.Body);
    }
}
