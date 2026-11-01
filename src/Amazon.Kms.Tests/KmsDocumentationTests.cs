using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

using Amazon.Kms.Exceptions;
using Amazon.Kms.Serialization;

namespace Amazon.Kms.Tests;

// Samples and valid values from https://docs.aws.amazon.com/kms/latest/APIReference/
public class KmsDocumentationTests
{
    private const string KeyArn = "arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab";
    private const string KeyMaterialId = "0b7fd7ddbac6eef27907413567cad8c810e2883dc8a7534067a82ee1142fc1e6";
    private const string SamplePlaintext = "VGhpcyBpcyBEYXkgMSBmb3IgdGhlIEludGVybmV0Cg==";

    [Fact]
    public async Task EncryptSample()
    {
        const string ciphertext = "CiDPoCH188S65r5Cy7pAhIFJMXDlU7mewhSlYUpuQIVBrhKmAQEBAgB4z6Ah9fPEuua+Qsu6QISBSTFw5VO5nsIUpWFKbkCFQa4AAAB9MHsGCSqGSIb3DQEHBqBuMGwCAQAwZwYJKoZIhvcNAQcBMB4GCWCGSAFlAwQBLjARBAxLc9b6QThC9jB/ZjYCARCAOt8la8qXLO5wB3JH2NlwWWzWRU2RKqpO9A/0psE5UWwkK6CnwoeC3Zj9Q0A66apZkbRglFfY1lTY+Tc=";

        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"{{ciphertext}}","KeyId":"{{KeyArn}}","EncryptionAlgorithm":"SYMMETRIC_DEFAULT"}""");

        EncryptResult result = await FakeKms.CreateClient(handler).EncryptAsync(new EncryptRequest("1234abcd-12ab-34cd-56ef-1234567890ab", Encoding.UTF8.GetBytes("This is Day 1 for the Internet\n")));

        Assert.Equal(SamplePlaintext, handler.Request.Json.GetProperty("Plaintext").GetString());
        Assert.Equal(ciphertext, Convert.ToBase64String(result.CiphertextBlob));
        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(EncryptionAlgorithm.SYMMETRIC_DEFAULT, result.EncryptionAlgorithm);
    }

    [Fact]
    public async Task DecryptSample()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","KeyMaterialId":"{{KeyMaterialId}}","Plaintext":"{{SamplePlaintext}}","EncryptionAlgorithm":"SYMMETRIC_DEFAULT"}""");

        DecryptResult result = await FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], null));

        Assert.Equal("This is Day 1 for the Internet\n", Encoding.UTF8.GetString(result.Plaintext));
        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(KeyMaterialId, result.KeyMaterialId);
        Assert.Equal(EncryptionAlgorithm.SYMMETRIC_DEFAULT, result.EncryptionAlgorithm);
        Assert.Null(result.CiphertextForRecipient);
    }

    [Fact]
    public async Task GenerateDataKeySample()
    {
        const string ciphertext = "AQEDAHjRYf5WytIc0C857tFSnBaPn2F8DgfmThbJlGfR8P3WlwAAAH4wfAYJKoZIhvcNAQcGoG8wbQIBADBoBgkqhkiG9w0BBwEwHgYJYIZIAWUDBAEuMBEEDEFogLqPWZconQhwHAIBEIA7d9AC7GeJJM34njQvg4Wf1d5sw0NIo1MrBqZa+YdhV8MrkBQPeac0ReRVNDt9qleAt+SHgIRF8P0H+7U=";
        const string plaintext = "VdzKNHGzUAzJeRBVY+uUmofUGGiDzyB3+i9fVkh3piw=";

        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"{{ciphertext}}","KeyId":"{{KeyArn}}","KeyMaterialId":"{{KeyMaterialId}}","Plaintext":"{{plaintext}}"}""");

        GenerateDataKeyResult result = await FakeKms.CreateClient(handler).GenerateDataKeyAsync(new GenerateDataKeyRequest("alias/ExampleAlias", KeySpec.AES_256, null));

        Assert.Equal("""{"KeyId":"alias/ExampleAlias","KeySpec":"AES_256"}""", handler.Request.Body);

        Assert.Equal(ciphertext, Convert.ToBase64String(result.CiphertextBlob));
        Assert.Equal(plaintext, Convert.ToBase64String(result.Plaintext));
        Assert.Equal(32, result.Plaintext.Length);
        Assert.Equal(KeyMaterialId, result.KeyMaterialId);
    }

    [Fact]
    public async Task CreateGrantSample()
    {
        const string grantId = "0c237476b39f8bc44e45212e08498fbe3151305030726c0590dd8d3e9f3d6a60";
        const string grantToken = "AQpAM2RhZTk1MGMyNTk2ZmZmMzEyYWVhOWViN2I1MWM4Mzc0MWFiYjc0ZDE1ODkyNGFlNTIzODZhMzgyZjBlNGY3NiKIAgEBAgB4Pa6VDCWW__MSrqnre1HIN0Grt00ViSSuUjhqOC8OT3YAAADfMIHcBgkqhkiG9w0BBwaggc4wgcsCAQAwgcUGCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQMmqLyBTAegIn9XlK5AgEQgIGXZQjkBcl1dykDdqZBUQ6L1OfUivQy7JVYO2-ZJP7m6f1g8GzV47HX5phdtONAP7K_HQIflcgpkoCqd_fUnE114mSmiagWkbQ5sqAVV3ov-VeqgrvMe5ZFEWLMSluvBAqdjHEdMIkHMlhlj4ENZbzBfo9Wxk8b8SnwP4kc4gGivedzFXo-dwN8fxjjq_ZZ9JFOj2ijIbj5FyogDCN0drOfi8RORSEuCEmPvjFRMFAwcmwFkN2NPp89amA";

        var handler = new FakeKmsHandler($$"""{"GrantId":"{{grantId}}","GrantToken":"{{grantToken}}"}""");

        CreateGrantResult result = await FakeKms.CreateClient(handler).CreateGrantAsync(new CreateGrantRequest {
            KeyId = KeyArn,
            GranteePrincipal = "arn:aws:iam::111122223333:role/ExampleRole",
            Operations = [KmsOperations.Encrypt, KmsOperations.Decrypt]
        });

        JsonElement json = handler.Request.Json;

        Assert.Equal("arn:aws:iam::111122223333:role/ExampleRole", json.GetProperty("GranteePrincipal").GetString());
        Assert.Equal(["Encrypt", "Decrypt"], json.GetProperty("Operations").EnumerateArray().Select(static o => o.GetString()));
        Assert.False(json.TryGetProperty("GranteeServicePrincipal", out _));

        Assert.Equal(grantId, result.GrantId);
        Assert.Equal(grantToken, result.GrantToken);
    }

    [Fact]
    public async Task CreateGrantForServicePrincipal()
    {
        var handler = new FakeKmsHandler("""{"GrantId":"id","GrantToken":"token"}""");

        await FakeKms.CreateClient(handler).CreateGrantAsync(new CreateGrantRequest {
            KeyId = "key",
            GranteeServicePrincipal = "logs.us-east-1.amazonaws.com",
            RetiringServicePrincipal = "logs.us-east-1.amazonaws.com",
            Operations = [KmsOperations.Decrypt],
            Constraints = new GrantConstraints {
                SourceArn = "arn:aws:logs:us-east-1:111122223333:log-group:example"
            },
            DryRun = true
        });

        Assert.Equal(
            """{"KeyId":"key","GranteeServicePrincipal":"logs.us-east-1.amazonaws.com","Operations":["Decrypt"],"Constraints":{"SourceArn":"arn:aws:logs:us-east-1:111122223333:log-group:example"},"RetiringServicePrincipal":"logs.us-east-1.amazonaws.com","DryRun":true}""",
            handler.Request.Body);
    }

    [Fact]
    public void ListGrantsReadsServicePrincipalFields()
    {
        ListGrantsResult? result = JsonSerializer.Deserialize(
            """{"Grants":[{"GrantId":"id","GranteeServicePrincipal":"logs.us-east-1.amazonaws.com","RetiringServicePrincipal":"events.us-east-1.amazonaws.com","Constraints":{"SourceArn":"arn:source"}}]}""",
            KmsSerializerContext.Default.ListGrantsResult);

        Grant grant = Assert.Single(result!.Grants);

        Assert.Equal("logs.us-east-1.amazonaws.com", grant.GranteeServicePrincipal);
        Assert.Equal("events.us-east-1.amazonaws.com", grant.RetiringServicePrincipal);
        Assert.Equal("arn:source", grant.Constraints.SourceArn);
        Assert.Null(grant.GranteePrincipal);
    }

    // The docs say Name is null when none was provided, and their ListGrants sample shows an empty string
    [Theory]
    [InlineData("""{"Grants":[{"Name":null}]}""", null)]
    [InlineData("""{"Grants":[{"Name":""}]}""", "")]
    public void ListGrantsAllowsMissingName(string json, string? name)
    {
        ListGrantsResult? result = JsonSerializer.Deserialize(json, KmsSerializerContext.Default.ListGrantsResult);

        Assert.Equal(name, result!.Grants[0].Name);
    }

    [Fact]
    public async Task DryRunIsSent()
    {
        var encrypt = new FakeKmsHandler("""{"CiphertextBlob":"AQ==","KeyId":"k"}""");
        var decrypt = new FakeKmsHandler("""{"Plaintext":"AQ==","KeyId":"k"}""");

        await FakeKms.CreateClient(encrypt).EncryptAsync(new EncryptRequest("key", [1]) { DryRun = true });
        await FakeKms.CreateClient(decrypt).DecryptAsync(new DecryptRequest("key", [1], null) { DryRun = true, DryRunModifiers = ["IGNORE_CIPHERTEXT"] });

        Assert.True(encrypt.Request.Json.GetProperty("DryRun").GetBoolean());
        Assert.True(decrypt.Request.Json.GetProperty("DryRun").GetBoolean());
        Assert.Equal("IGNORE_CIPHERTEXT", decrypt.Request.Json.GetProperty("DryRunModifiers")[0].GetString());
    }

    [Fact]
    public async Task DryRunIsOmittedByDefault()
    {
        var handler = new FakeKmsHandler("""{"Plaintext":"AQ==","KeyId":"k"}""");

        await FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], null));

        Assert.False(handler.Request.Json.TryGetProperty("DryRun", out _));
        Assert.False(handler.Request.Json.TryGetProperty("DryRunModifiers", out _));
    }

    [Fact]
    public async Task SuccessfulDryRunIsReportedAsException()
    {
        var handler = new FakeKmsHandler("""{"__type":"DryRunOperationException","message":"details"}""", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<DryRunOperationException>(() => FakeKms.CreateClient(handler).EncryptAsync(new EncryptRequest("key", [1]) { DryRun = true }));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
    }

    [Fact]
    public async Task InvalidSignatureIsAnException()
    {
        var handler = new FakeKmsHandler("""{"__type":"KMSInvalidSignatureException"}""", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<KmsException>(() => FakeKms.CreateClient(handler).VerifyAsync(new VerifyRequest("key", [1], [2], SigningAlgorithm.ECDSA_SHA_256)));

        Assert.Equal("KMSInvalidSignatureException", ex.Type);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
    }

    [Theory]
    [InlineData("AlreadyExistsException")]
    [InlineData("DisabledException")]
    [InlineData("IncorrectKeyException")]
    [InlineData("InvalidAliasNameException")]
    [InlineData("InvalidCiphertextException")]
    [InlineData("InvalidGrantTokenException")]
    [InlineData("InvalidKeyUsageException")]
    [InlineData("KMSInvalidStateException")]
    [InlineData("LimitExceededException")]
    [InlineData("NotFoundException")]
    public async Task DocumentedClientErrorsKeepTheirType(string type)
    {
        var handler = new FakeKmsHandler($$"""{"__type":"{{type}}","message":"details"}""", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<KmsException>(() => FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], null)));

        Assert.Equal(type, ex.Type);
        Assert.Equal("details", ex.Message);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
    }

    [Theory]
    [InlineData(SigningAlgorithm.ML_DSA_SHAKE_256, "EXTERNAL_MU")]
    [InlineData(SigningAlgorithm.ED25519_SHA_512, "RAW")]
    [InlineData(SigningAlgorithm.ED25519_PH_SHA_512, "DIGEST")]
    public async Task SignsWithNewAlgorithms(SigningAlgorithm algorithm, string messageType)
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"k","Signature":"AQ==","SigningAlgorithm":"{{algorithm}}"}""");

        SignResult result = await FakeKms.CreateClient(handler).SignAsync(new SignRequest {
            KeyId = "key",
            Message = [1],
            MessageType = messageType,
            SigningAlgorithm = algorithm
        });

        Assert.Equal(algorithm.ToString(), handler.Request.Json.GetProperty("SigningAlgorithm").GetString());
        Assert.Equal(messageType, handler.Request.Json.GetProperty("MessageType").GetString());
        Assert.Equal(algorithm, result.SigningAlgorithm);
    }

    [Theory]
    [InlineData(SigningAlgorithm.ML_DSA_SHAKE_256)]
    [InlineData(SigningAlgorithm.ED25519_SHA_512)]
    public async Task VerifiesWithNewAlgorithms(SigningAlgorithm algorithm)
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"k","SignatureValid":true,"SigningAlgorithm":"{{algorithm}}"}""");

        VerifyResult result = await FakeKms.CreateClient(handler).VerifyAsync(new VerifyRequest("key", [1], [2], algorithm));

        Assert.Equal(algorithm, result.SigningAlgorithm);
    }

    [Theory]
    [InlineData("ECC_NIST_EDWARDS25519")]
    [InlineData("ML_DSA_65")]
    [InlineData("ML_KEM_768")]
    public void KeyMetadataReadsNewKeySpecs(string keySpec)
    {
        var result = JsonSerializer.Deserialize<KeyMetadata>($$"""{"KeyId":"key","KeySpec":"{{keySpec}}","KeyState":"Enabled"}""");

        Assert.Equal(keySpec, result!.KeySpec.ToString());
    }

    [Fact]
    public void KeySpecsMatchDocumentedValues()
    {
        // AES_128 and AES_256 are the GenerateDataKey values; the rest are the CreateKey values
        AssertSameValues<KeySpec>("AES_256 | AES_128 | RSA_2048 | RSA_3072 | RSA_4096 | ECC_NIST_P256 | ECC_NIST_P384 | ECC_NIST_P521 | ECC_SECG_P256K1 | SYMMETRIC_DEFAULT | HMAC_224 | HMAC_256 | HMAC_384 | HMAC_512 | SM2 | ML_DSA_44 | ML_DSA_65 | ML_DSA_87 | ECC_NIST_EDWARDS25519 | ML_KEM_512 | ML_KEM_768 | ML_KEM_1024");
    }

    [Fact]
    public void SigningAlgorithmsMatchDocumentedValues()
    {
        AssertSameValues<SigningAlgorithm>("RSASSA_PSS_SHA_256 | RSASSA_PSS_SHA_384 | RSASSA_PSS_SHA_512 | RSASSA_PKCS1_V1_5_SHA_256 | RSASSA_PKCS1_V1_5_SHA_384 | RSASSA_PKCS1_V1_5_SHA_512 | ECDSA_SHA_256 | ECDSA_SHA_384 | ECDSA_SHA_512 | SM2DSA | ML_DSA_SHAKE_256 | ED25519_SHA_512 | ED25519_PH_SHA_512");
    }

    [Fact]
    public void OtherEnumsMatchDocumentedValues()
    {
        AssertSameValues<EncryptionAlgorithm>("SYMMETRIC_DEFAULT | RSAES_OAEP_SHA_1 | RSAES_OAEP_SHA_256 | SM2PKE");
        AssertSameValues<MacAlgorithm>("HMAC_SHA_224 | HMAC_SHA_256 | HMAC_SHA_384 | HMAC_SHA_512");
        AssertSameValues<KeyState>("Creating | Enabled | Disabled | PendingDeletion | PendingImport | PendingReplicaDeletion | Unavailable | Updating");
        AssertSameValues<KeyManager>("AWS | CUSTOMER");
        AssertSameValues<KeyOrigin>("AWS_KMS | EXTERNAL | AWS_CLOUDHSM | EXTERNAL_KEY_STORE");
    }

    [Fact]
    public void OperationsMatchDocumentedGrantOperations()
    {
        var constants = typeof(KmsOperations).GetFields(BindingFlags.Public | BindingFlags.Static).Select(static f => (string)f.GetRawConstantValue()!);

        AssertSameValues("Decrypt | Encrypt | GenerateDataKey | GenerateDataKeyWithoutPlaintext | ReEncryptFrom | ReEncryptTo | Sign | Verify | GetPublicKey | CreateGrant | RetireGrant | DescribeKey | GenerateDataKeyPair | GenerateDataKeyPairWithoutPlaintext | GenerateMac | VerifyMac | DeriveSharedSecret | Encapsulate | Decapsulate", constants);
    }

    private static void AssertSameValues<T>(string documented)
        where T : struct, Enum
    {
        AssertSameValues(documented, Enum.GetNames<T>());
    }

    private static void AssertSameValues(string documented, IEnumerable<string> actual)
    {
        Assert.Equal(documented.Split(" | ").Order(StringComparer.Ordinal), actual.Order(StringComparer.Ordinal));
    }
}
