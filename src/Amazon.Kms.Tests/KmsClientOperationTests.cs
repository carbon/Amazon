using System.Net;
using System.Text.Json;

namespace Amazon.Kms.Tests;

public class KmsClientOperationTests
{
    // Encodes to "+/+/", the characters that differ between base64 and base64url
    private static readonly byte[] s_binary = [0xFB, 0xFF, 0xBF];

    private const string KeyArn = "arn:aws:kms:us-east-1:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab";

    private static void AssertOperation(RecordedKmsRequest request, string action)
    {
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://kms.us-east-1.amazonaws.com/", request.RequestUri.ToString());
        Assert.Equal($"TrentService.{action}", request.Target);
        Assert.Equal("application/x-amz-json-1.1", request.ContentType);
    }

    [Fact]
    public async Task SignsRequestsWithoutExposingTheSecret()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","CiphertextBlob":"AQ=="}""");

        await FakeKms.CreateClient(handler).EncryptAsync(new EncryptRequest("key", [1]));

        string authorization = handler.Request.Authorization;

        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", authorization);
        Assert.Contains("/us-east-1/kms/aws4_request", authorization);
        Assert.Contains("x-amz-target", authorization);
        Assert.DoesNotContain("private", authorization);
    }

    [Fact]
    public async Task Encrypt()
    {
        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"+/+/","KeyId":"{{KeyArn}}","EncryptionAlgorithm":"RSAES_OAEP_SHA_256"}""");

        var request = new EncryptRequest("key", s_binary, new Dictionary<string, string> { { "a", "b" } }, ["token"]) {
            EncryptionAlgorithm = EncryptionAlgorithm.RSAES_OAEP_SHA_256
        };

        EncryptResult result = await FakeKms.CreateClient(handler).EncryptAsync(request);

        AssertOperation(handler.Request, "Encrypt");

        JsonElement json = handler.Request.Json;

        Assert.Equal("key", json.GetProperty("KeyId").GetString());
        Assert.Equal("+/+/", json.GetProperty("Plaintext").GetString());
        Assert.Equal("b", json.GetProperty("EncryptionContext").GetProperty("a").GetString());
        Assert.Equal("token", json.GetProperty("GrantTokens")[0].GetString());
        Assert.Equal("RSAES_OAEP_SHA_256", json.GetProperty("EncryptionAlgorithm").GetString());

        Assert.Equal(s_binary, result.CiphertextBlob);
        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(EncryptionAlgorithm.RSAES_OAEP_SHA_256, result.EncryptionAlgorithm);
    }

    [Fact]
    public async Task EncryptOmitsUnsetOptionalFields()
    {
        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"AQ==","KeyId":"{{KeyArn}}"}""");

        await FakeKms.CreateClient(handler).EncryptAsync(new EncryptRequest("key", [1]));

        Assert.Equal("""{"KeyId":"key","Plaintext":"AQ=="}""", handler.Request.Body);
    }

    [Fact]
    public async Task Decrypt()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","Plaintext":"+/+/","EncryptionAlgorithm":"SYMMETRIC_DEFAULT"}""");

        var request = new DecryptRequest("key", s_binary, new Dictionary<string, string> { { "a", "b" } }, ["token"]);

        DecryptResult result = await FakeKms.CreateClient(handler).DecryptAsync(request);

        AssertOperation(handler.Request, "Decrypt");

        JsonElement json = handler.Request.Json;

        Assert.Equal("+/+/", json.GetProperty("CiphertextBlob").GetString());
        Assert.Equal("b", json.GetProperty("EncryptionContext").GetProperty("a").GetString());
        Assert.Equal("token", json.GetProperty("GrantTokens")[0].GetString());
        Assert.False(json.TryGetProperty("Recipient", out _));

        Assert.Equal(s_binary, result.Plaintext);
        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(EncryptionAlgorithm.SYMMETRIC_DEFAULT, result.EncryptionAlgorithm);
    }

    // The context is authenticated data: KMS requires an exact, case-sensitive match on decrypt
    [Fact]
    public async Task EncryptionContextSurvivesJsonEscaping()
    {
        var context = new Dictionary<string, string> {
            { "Key", "a+b/c=" },
            { "key", "\"quoted\" <tag> & \\" },
            { "ünï", "日本語 \n" },
            { "empty", "" }
        };

        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"AQ==","KeyId":"{{KeyArn}}"}""");

        await FakeKms.CreateClient(handler).EncryptAsync(new EncryptRequest("key", [1], context));

        var sent = handler.Request.Json.GetProperty("EncryptionContext").EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetString());

        Assert.Equal(context.Count, sent.Count);

        foreach (var (key, value) in context)
        {
            Assert.Equal(value, sent[key]);
        }
    }

    [Fact]
    public async Task EmptyEncryptionContextIsSent()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","Plaintext":"AQ=="}""");

        await FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], new Dictionary<string, string>()));

        Assert.Equal(JsonValueKind.Object, handler.Request.Json.GetProperty("EncryptionContext").ValueKind);
    }

    [Fact]
    public async Task InvalidBase64InResultThrows()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","Plaintext":"not base64!"}""");

        await Assert.ThrowsAsync<JsonException>(() => FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], null)));
    }

    [Fact]
    public async Task MissingRequiredResultFieldThrows()
    {
        var handler = new FakeKmsHandler("""{"Plaintext":"AQ=="}""");

        await Assert.ThrowsAsync<JsonException>(() => FakeKms.CreateClient(handler).DecryptAsync(new DecryptRequest("key", [1], null)));
    }

    [Fact]
    public async Task Sign()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","Signature":"+/+/","SigningAlgorithm":"ECDSA_SHA_256"}""");

        SignResult result = await FakeKms.CreateClient(handler).SignAsync(new SignRequest {
            KeyId = "key",
            Message = s_binary,
            MessageType = "DIGEST",
            SigningAlgorithm = SigningAlgorithm.ECDSA_SHA_256,
            GrantTokens = ["token"],
            DryRun = true
        });

        AssertOperation(handler.Request, "Sign");

        JsonElement json = handler.Request.Json;

        Assert.Equal("key", json.GetProperty("KeyId").GetString());
        Assert.Equal("+/+/", json.GetProperty("Message").GetString());
        Assert.Equal("DIGEST", json.GetProperty("MessageType").GetString());
        Assert.Equal("ECDSA_SHA_256", json.GetProperty("SigningAlgorithm").GetString());
        Assert.Equal("token", json.GetProperty("GrantTokens")[0].GetString());
        Assert.True(json.GetProperty("DryRun").GetBoolean());

        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(s_binary, result.Signature);
        Assert.Equal(SigningAlgorithm.ECDSA_SHA_256, result.SigningAlgorithm);
    }

    // RSASSA_PSS_SHA_256 is the zero value of the enum and must still be sent
    [Fact]
    public async Task SignSendsDefaultEnumValue()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","Signature":"AQ==","SigningAlgorithm":"RSASSA_PSS_SHA_256"}""");

        await FakeKms.CreateClient(handler).SignAsync(new SignRequest {
            KeyId = "key",
            Message = [1],
            SigningAlgorithm = SigningAlgorithm.RSASSA_PSS_SHA_256
        });

        Assert.Equal("""{"Message":"AQ==","SigningAlgorithm":"RSASSA_PSS_SHA_256","KeyId":"key"}""", handler.Request.Body);
    }

    [Fact]
    public async Task Verify()
    {
        var handler = new FakeKmsHandler($$"""{"KeyId":"{{KeyArn}}","SignatureValid":true,"SigningAlgorithm":"RSASSA_PSS_SHA_384"}""");

        var request = new VerifyRequest("key", s_binary, [1, 2], SigningAlgorithm.RSASSA_PSS_SHA_384) {
            MessageType = "RAW",
            GrantTokens = ["token"],
            DryRun = true
        };

        VerifyResult result = await FakeKms.CreateClient(handler).VerifyAsync(request);

        AssertOperation(handler.Request, "Verify");

        JsonElement json = handler.Request.Json;

        Assert.Equal("+/+/", json.GetProperty("Message").GetString());
        Assert.Equal("AQI=", json.GetProperty("Signature").GetString());
        Assert.Equal("RAW", json.GetProperty("MessageType").GetString());
        Assert.Equal("RSASSA_PSS_SHA_384", json.GetProperty("SigningAlgorithm").GetString());

        Assert.True(result.SignatureValid);
        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(SigningAlgorithm.RSASSA_PSS_SHA_384, result.SigningAlgorithm);
    }

    [Fact]
    public async Task GenerateDataKey()
    {
        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"+/+/","KeyId":"{{KeyArn}}","Plaintext":"AQI="}""");

        GenerateDataKeyResult result = await FakeKms.CreateClient(handler).GenerateDataKeyAsync(new GenerateDataKeyRequest("key", KeySpec.AES_256, new Dictionary<string, string> { { "a", "b" } }));

        AssertOperation(handler.Request, "GenerateDataKey");

        Assert.Equal("""{"KeyId":"key","EncryptionContext":{"a":"b"},"KeySpec":"AES_256"}""", handler.Request.Body);

        Assert.Equal(KeyArn, result.KeyId);
        Assert.Equal(s_binary, result.CiphertextBlob);
        Assert.Equal([1, 2], result.Plaintext);
    }

    [Fact]
    public async Task GenerateDataKeyWithoutPlaintext()
    {
        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"+/+/","KeyId":"{{KeyArn}}"}""");

        GenerateDataKeyResult result = await FakeKms.CreateClient(handler).GenerateDataKeyWithoutPlaintextAsync(new GenerateDataKeyRequest {
            KeyId = "key",
            NumberOfBytes = 64,
            GrantTokens = ["token"],
            DryRun = true
        });

        AssertOperation(handler.Request, "GenerateDataKeyWithoutPlaintext");

        Assert.Equal("""{"KeyId":"key","GrantTokens":["token"],"NumberOfBytes":64,"DryRun":true}""", handler.Request.Body);

        Assert.Equal(s_binary, result.CiphertextBlob);
        Assert.Null(result.Plaintext);
    }

    [Fact]
    public async Task GenerateDataKeySendsRecipient()
    {
        var handler = new FakeKmsHandler($$"""{"CiphertextBlob":"AQ==","KeyId":"{{KeyArn}}"}""");

        await FakeKms.CreateClient(handler).GenerateDataKeyAsync(new GenerateDataKeyRequest {
            KeyId = "key",
            KeySpec = KeySpec.AES_256,
            Recipient = new RecipientInfo {
                AttestationDocument = s_binary,
                KeyEncryptionAlgorithm = "RSAES_OAEP_SHA_256"
            }
        });

        JsonElement recipient = handler.Request.Json.GetProperty("Recipient");

        Assert.Equal("+/+/", recipient.GetProperty("AttestationDocument").GetString());
        Assert.Equal("RSAES_OAEP_SHA_256", recipient.GetProperty("KeyEncryptionAlgorithm").GetString());
    }

    [Fact]
    public async Task CreateAlias()
    {
        // Per the docs a successful CreateAlias is a 200 with an empty body
        var handler = new FakeKmsHandler("");

        await FakeKms.CreateClient(handler).CreateAliasAsync(new CreateAliasRequest("key", "alias/name"));

        AssertOperation(handler.Request, "CreateAlias");

        Assert.Equal("""{"TargetKeyId":"key","AliasName":"alias/name"}""", handler.Request.Body);
    }

    [Fact]
    public async Task CreateAliasReturnsResult()
    {
        CreateAliasResult result = await FakeKms.CreateClient(new FakeKmsHandler("")).CreateAliasAsync(new CreateAliasRequest("key", "alias/name"));

        Assert.NotNull(result);
    }

    [Fact]
    public async Task EmptyBodyFromOperationWithResultThrows()
    {
        var client = FakeKms.CreateClient(new FakeKmsHandler(""));

        await Assert.ThrowsAsync<JsonException>(() => client.DecryptAsync(new DecryptRequest("key", [1], null)));
    }

    [Fact]
    public async Task CreateGrant()
    {
        var handler = new FakeKmsHandler("""{"GrantId":"grant-id","GrantToken":"grant-token"}""");

        CreateGrantResult result = await FakeKms.CreateClient(handler).CreateGrantAsync(new CreateGrantRequest {
            KeyId = "key",
            GranteePrincipal = "grantee",
            RetiringPrincipal = "retiree",
            Name = "name",
            GrantTokens = ["token"],
            Operations = [KmsOperations.Decrypt, KmsOperations.RetireGrant],
            Constraints = new GrantConstraints {
                EncryptionContextSubset = new Dictionary<string, string> { { "a", "b" } }
            }
        });

        AssertOperation(handler.Request, "CreateGrant");

        JsonElement json = handler.Request.Json;

        Assert.Equal("grantee", json.GetProperty("GranteePrincipal").GetString());
        Assert.Equal("retiree", json.GetProperty("RetiringPrincipal").GetString());
        Assert.Equal("name", json.GetProperty("Name").GetString());
        Assert.Equal("RetireGrant", json.GetProperty("Operations")[1].GetString());
        Assert.Equal("b", json.GetProperty("Constraints").GetProperty("EncryptionContextSubset").GetProperty("a").GetString());
        Assert.False(json.GetProperty("Constraints").TryGetProperty("EncryptionContextEquals", out _));

        Assert.Equal("grant-id", result.GrantId);
        Assert.Equal("grant-token", result.GrantToken);
    }

    [Fact]
    public async Task RetireGrant()
    {
        var handler = new FakeKmsHandler("");

        await FakeKms.CreateClient(handler).RetireGrantAsync(new RetireGrantRequest("token") { DryRun = true });

        AssertOperation(handler.Request, "RetireGrant");

        Assert.Equal("""{"GrantToken":"token","DryRun":true}""", handler.Request.Body);
    }

    [Fact]
    public async Task RevokeGrant()
    {
        var handler = new FakeKmsHandler("");

        await FakeKms.CreateClient(handler).RevokeGrantAsync(new RevokeGrantRequest("key", "grant") { DryRun = true });

        AssertOperation(handler.Request, "RevokeGrant");

        Assert.Equal("""{"KeyId":"key","GrantId":"grant","DryRun":true}""", handler.Request.Body);
    }

    [Fact]
    public async Task ListGrantsFollowsMarker()
    {
        var handler = new FakeKmsHandler("""{"Grants":[{"GrantId":"1","Constraints":{"EncryptionContextEquals":{"a":"b"}}}],"NextMarker":"next","Truncated":true}""");

        ListGrantsResult result = await FakeKms.CreateClient(handler).ListGrantsAsync(new ListGrantsRequest("key") {
            Limit = 10,
            Marker = "start"
        });

        AssertOperation(handler.Request, "ListGrants");

        Assert.Equal("""{"KeyId":"key","Limit":10,"Marker":"start"}""", handler.Request.Body);

        Assert.True(result.Truncated);
        Assert.Equal("next", result.NextMarker);
        Assert.Equal("1", Assert.Single(result.Grants).GrantId);
        Assert.Equal("b", result.Grants[0].Constraints.EncryptionContextEquals!["a"]);
    }

    [Fact]
    public async Task ListGrantsLastPageHasNoMarker()
    {
        ListGrantsResult result = await FakeKms.CreateClient(new FakeKmsHandler("""{"Grants":[],"Truncated":false}""")).ListGrantsAsync(new ListGrantsRequest("key"));

        Assert.False(result.Truncated);
        Assert.Null(result.NextMarker);
        Assert.Empty(result.Grants);
    }

    [Fact]
    public async Task NoContentReturnsNoBodyParseError()
    {
        var handler = new FakeKmsHandler("", HttpStatusCode.NoContent);

        await FakeKms.CreateClient(handler).RevokeGrantAsync(new RevokeGrantRequest("key", "grant"));

        Assert.Single(handler.Requests);
    }
}
