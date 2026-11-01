using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

using Amazon.Kms;
using Amazon.S3;
using Amazon.Ses.Tests;

namespace Amazon.Ses.Extensions.Tests;

public class EmailMessageDecryptorTests
{
    private static readonly byte[] s_key = Enumerable.Range(1, 32).Select(static i => (byte)i).ToArray();
    private static readonly byte[] s_iv  = Enumerable.Range(1, 12).Select(static i => (byte)i).ToArray();

    private static EmailMessageDecryptor CreateDecryptor(FakeHandler? handler = null)
    {
        handler ??= new FakeHandler(() => FakeHttp.Json(
            $$"""{"KeyId":"key","Plaintext":"{{Convert.ToBase64String(s_key)}}"}"""));

        return new EmailMessageDecryptor(FakeHttp.Use(new KmsClient(AwsRegion.USEast1, new AwsCredential("public", "private")), handler));
    }

    private static S3Object CreateBlob(byte[] plaintext, string cekAlg = "AES/GCM/NoPadding", string tagLength = "128", long? contentLength = null, byte[]? key = null)
    {
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[16];

        using (var aes = new AesGcm(key ?? s_key, 16))
        {
            aes.Encrypt(s_iv, plaintext, ciphertext, tag);
        }

        var response = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent([.. ciphertext, .. tag]),
            Headers = {
                { "x-amz-meta-x-amz-cek-alg", cekAlg },
                { "x-amz-meta-x-amz-matdesc", """{"kms_cmk_id":"arn:aws:kms:us-east-1:1:key/1"}""" },
                { "x-amz-meta-x-amz-key-v2", Convert.ToBase64String([1, 2, 3]) },
                { "x-amz-meta-x-amz-iv", Convert.ToBase64String(s_iv) },
                { "x-amz-meta-x-amz-unencrypted-content-length", (contentLength ?? plaintext.Length).ToString(CultureInfo.InvariantCulture) },
                { "x-amz-meta-x-amz-tag-len", tagLength }
            }
        };

        return new S3Object("key", response);
    }

    [Fact]
    public async Task ThrowsWhenNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateDecryptor().DecryptAsync(null));
    }

    [Fact]
    public async Task DecryptsMessage()
    {
        var handler = new FakeHandler(() => FakeHttp.Json(
            $$"""{"KeyId":"key","Plaintext":"{{Convert.ToBase64String(s_key)}}"}"""));

        var plaintext = Encoding.UTF8.GetBytes("Subject: Hello");

        using var blob = CreateBlob(plaintext);

        var result = await CreateDecryptor(handler).DecryptAsync(blob);

        Assert.Equal(plaintext, result);

        var request = Assert.Single(handler.Requests);

        Assert.Equal("TrentService.Decrypt", request.Target);
        Assert.Contains("arn:aws:kms:us-east-1:1:key/1", request.Body);
    }

    [Fact]
    public async Task ThrowsWhenCiphertextDoesNotMatchKey()
    {
        using var blob = CreateBlob("message"u8.ToArray(), key: new byte[32]);

        await Assert.ThrowsAnyAsync<CryptographicException>(() => CreateDecryptor().DecryptAsync(blob));
    }

    [Fact]
    public async Task RejectsUnexpectedAlgorithm()
    {
        using var blob = CreateBlob("message"u8.ToArray(), cekAlg: "AES/CBC/PKCS5Padding");

        var ex = await Assert.ThrowsAsync<Exception>(() => CreateDecryptor().DecryptAsync(blob));

        Assert.Equal("Expected AES/GCM/NoPadding. Was 'AES/CBC/PKCS5Padding'", ex.Message);
    }

    [Fact]
    public async Task RejectsUnexpectedTagLength()
    {
        using var blob = CreateBlob("message"u8.ToArray(), tagLength: "96");

        var ex = await Assert.ThrowsAsync<Exception>(() => CreateDecryptor().DecryptAsync(blob));

        Assert.Equal("x-amz-tag-len must be 128 bits. Was 96", ex.Message);
    }

    [Fact]
    public async Task RejectsMessagesOver40MB()
    {
        using var blob = CreateBlob("message"u8.ToArray(), contentLength: 40_000_001);

        var ex = await Assert.ThrowsAsync<Exception>(() => CreateDecryptor().DecryptAsync(blob));

        Assert.Equal("> 40MB. Was 40000001", ex.Message);
    }
}
