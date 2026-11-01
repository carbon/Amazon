using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Amazon.Sns.Tests;

public class SnsMessageVerifierTests
{
    private const string CertUrl = "https://sns.us-east-2.amazonaws.com/SimpleNotificationService-abc.pem";

    private static readonly RSA s_key = RSA.Create(2048);
    private static readonly string s_certPem = CreateCertificatePem(s_key);

    private static string CreateCertificatePem(RSA key, int notBeforeDays = -1, int notAfterDays = 1)
    {
        var request = new CertificateRequest("CN=sns.amazonaws.com", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        using var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(notBeforeDays), DateTimeOffset.UtcNow.AddDays(notAfterDays));

        return certificate.ExportCertificatePem();
    }

    private sealed class CertHandler(Func<HttpResponseMessage> response, Uri? finalUri = null) : HttpMessageHandler
    {
        public int Count { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Count++;

            var result = response();

            result.RequestMessage = finalUri is null ? request : new HttpRequestMessage(request.Method, finalUri);

            return Task.FromResult(result);
        }
    }

    private static CertHandler ServePem(string? pem = null) => new(() => new HttpResponseMessage(HttpStatusCode.OK) {
        Content = new StringContent(pem ?? s_certPem)
    });

    private static SnsMessageVerifier CreateVerifier(CertHandler? handler = null) => new(new HttpClient(handler ?? ServePem()));

    private static Dictionary<string, string> Notification(string version = "2") => new() {
        ["Type"]             = "Notification",
        ["MessageId"]        = "4d4dc071-ddbf-465d-bba8-08f81c89da64",
        ["TopicArn"]         = "arn:aws:sns:us-east-2:123456789012:MyTopic",
        ["Subject"]          = "My subject",
        ["Message"]          = "My Test Message",
        ["Timestamp"]        = "2019-01-31T04:37:04.321Z",
        ["SignatureVersion"] = version,
        ["SigningCertURL"]   = CertUrl,
        ["UnsubscribeURL"]   = "https://sns.us-east-2.amazonaws.com/?Action=Unsubscribe"
    };

    private static Dictionary<string, string> Confirmation(string type) => new() {
        ["Type"]             = type,
        ["MessageId"]        = "3d891288-136d-417f-bc05-901c108273ee",
        ["Token"]            = "abc123",
        ["TopicArn"]         = "arn:aws:sns:us-east-2:123456789012:MyTopic",
        ["Message"]          = "Please confirm your subscription",
        ["SubscribeURL"]     = "https://sns.us-east-2.amazonaws.com/?Action=ConfirmSubscription",
        ["Timestamp"]        = "2024-01-01T00:00:00.000Z",
        ["SignatureVersion"] = "2",
        ["SigningCertURL"]   = CertUrl
    };

    // Written out separately from the verifier, following the format in the AWS docs
    private static string Sign(Dictionary<string, string> fields, RSA? key = null, params string[] names)
    {
        var text = new StringBuilder();

        foreach (var name in names)
        {
            if (fields.TryGetValue(name, out var value))
            {
                text.Append($"{name}\n{value}\n");
            }
        }

        var hash = fields["SignatureVersion"] is "1" ? HashAlgorithmName.SHA1 : HashAlgorithmName.SHA256;

        return Convert.ToBase64String((key ?? s_key).SignData(Encoding.UTF8.GetBytes(text.ToString()), hash, RSASignaturePadding.Pkcs1));
    }

    private static Dictionary<string, string> SignNotification(Dictionary<string, string> fields, RSA? key = null)
    {
        fields["Signature"] = Sign(fields, key, "Message", "MessageId", "Subject", "Timestamp", "TopicArn", "Type");

        return fields;
    }

    private static Dictionary<string, string> SignConfirmation(Dictionary<string, string> fields)
    {
        fields["Signature"] = Sign(fields, null, "Message", "MessageId", "SubscribeURL", "Timestamp", "Token", "TopicArn", "Type");

        return fields;
    }

    private static string ToJson(Dictionary<string, string> fields) => JsonSerializer.Serialize(fields);

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    public async Task Notification_Verifies(string version)
    {
        Assert.True(await CreateVerifier().VerifyAsync(ToJson(SignNotification(Notification(version)))));
    }

    [Fact]
    public async Task SignedMessage_MatchesTheExactStringToSign()
    {
        var fields = Notification();

        const string stringToSign =
            "Message\nMy Test Message\n" +
            "MessageId\n4d4dc071-ddbf-465d-bba8-08f81c89da64\n" +
            "Subject\nMy subject\n" +
            "Timestamp\n2019-01-31T04:37:04.321Z\n" +
            "TopicArn\narn:aws:sns:us-east-2:123456789012:MyTopic\n" +
            "Type\nNotification\n";

        fields["Signature"] = Convert.ToBase64String(s_key.SignData(Encoding.UTF8.GetBytes(stringToSign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));

        Assert.True(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task Notification_WithoutSubject_Verifies()
    {
        var fields = Notification();

        fields.Remove("Subject");

        Assert.True(await CreateVerifier().VerifyAsync(ToJson(SignNotification(fields))));
    }

    [Fact]
    public async Task Notification_NullSubject_IsTreatedAsAbsent()
    {
        var fields = Notification();

        fields.Remove("Subject");

        var json = ToJson(SignNotification(fields)).Replace("{", """{"Subject":null,""");

        Assert.True(await CreateVerifier().VerifyAsync(json));
    }

    [Theory]
    [InlineData("SubscriptionConfirmation")]
    [InlineData("UnsubscribeConfirmation")]
    public async Task Confirmations_Verify(string type)
    {
        var json = ToJson(SignConfirmation(Confirmation(type)));

        Assert.True(await CreateVerifier().VerifyAsync(json));
    }

    [Fact]
    public async Task Verifies_EscapedCharacters_AndOriginalTimestampText()
    {
        var fields = Notification();

        fields["Message"]   = "line1\nline2 \"quoted\" héllo ✓";
        fields["Timestamp"] = "2019-01-31T04:37:04Z";

        Assert.True(await CreateVerifier().VerifyAsync(ToJson(SignNotification(fields))));
    }

    [Fact]
    public async Task VerifiedJson_ParsesAsSnsMessage()
    {
        var json = ToJson(SignConfirmation(Confirmation("SubscriptionConfirmation")));

        Assert.True(await CreateVerifier().VerifyAsync(json));

        Assert.Equal("abc123", Assert.IsType<SubscriptionConfirmation>(JsonSerializer.Deserialize<SnsMessage>(json)).Token);
    }

    [Theory]
    [InlineData("Message")]
    [InlineData("MessageId")]
    [InlineData("Subject")]
    [InlineData("Timestamp")]
    [InlineData("TopicArn")]
    public async Task TamperedNotification_Fails(string field)
    {
        var fields = SignNotification(Notification());

        fields[field] += "x";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Theory]
    [InlineData("SubscribeURL")]
    [InlineData("Token")]
    public async Task TamperedConfirmation_Fails(string field)
    {
        var fields = SignConfirmation(Confirmation("SubscriptionConfirmation"));

        fields[field] += "x";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task ChangedType_Fails()
    {
        var fields = SignConfirmation(Confirmation("SubscriptionConfirmation"));

        fields["Type"] = "UnsubscribeConfirmation";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task AddedSubject_Fails()
    {
        var fields = Notification();

        fields.Remove("Subject");
        SignNotification(fields);

        fields["Subject"] = "Added";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task SignatureFromAnotherKey_Fails()
    {
        using var otherKey = RSA.Create(2048);

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(SignNotification(Notification(), otherKey))));
    }

    [Fact]
    public async Task SignatureVersionDowngrade_Fails()
    {
        var fields = SignNotification(Notification("2"));

        fields["SignatureVersion"] = "1";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task DuplicateProperty_Fails()
    {
        // A parser reading the first Type would not see the one that was signed
        var json = ToJson(SignConfirmation(Confirmation("SubscriptionConfirmation")));

        Assert.False(await CreateVerifier().VerifyAsync("""{"Type":"Notification",""" + json[1..]));
    }

    [Theory]
    [InlineData("3")]
    [InlineData("")]
    public async Task UnknownSignatureVersion_Fails(string version)
    {
        var handler = ServePem();

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(Notification(version)))));
        Assert.Equal(0, handler.Count);
    }

    [Fact]
    public async Task UnknownType_Fails()
    {
        var fields = Notification();

        fields["Type"] = "Other";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(SignNotification(fields))));
    }

    [Theory]
    [InlineData("Signature")]
    [InlineData("SigningCertURL")]
    [InlineData("SignatureVersion")]
    [InlineData("Type")]
    [InlineData("Message")]
    [InlineData("MessageId")]
    [InlineData("Timestamp")]
    [InlineData("TopicArn")]
    public async Task MissingField_Fails(string field)
    {
        var fields = SignNotification(Notification());

        fields.Remove(field);

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Theory]
    [InlineData("Token")]
    [InlineData("SubscribeURL")]
    public async Task MissingConfirmationField_Fails(string field)
    {
        var fields = SignConfirmation(Confirmation("SubscriptionConfirmation"));

        fields.Remove(field);

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task NonStringField_Fails()
    {
        var json = ToJson(SignNotification(Notification())).Replace("\"My Test Message\"", "1");

        Assert.False(await CreateVerifier().VerifyAsync(json));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not json")]
    [InlineData("[]")]
    [InlineData("""{"Type":""")]
    public async Task InvalidJson_Fails(string json)
    {
        Assert.False(await CreateVerifier().VerifyAsync(json));
    }

    [Fact]
    public async Task InvalidBase64_Fails()
    {
        var fields = SignNotification(Notification());

        fields["Signature"] = "not base64!";

        Assert.False(await CreateVerifier().VerifyAsync(ToJson(fields)));
    }

    [Fact]
    public async Task RejectsNullJson()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateVerifier().VerifyAsync(null!));
    }

    [Theory]
    [InlineData("http://sns.us-east-2.amazonaws.com/cert.pem")]
    [InlineData("https://evil.example.com/cert.pem")]
    [InlineData("https://sns.us-east-2.amazonaws.com.evil.example.com/cert.pem")]
    [InlineData("https://evil.example.com/sns.us-east-2.amazonaws.com/cert.pem")]
    [InlineData("https://sns.us-east-2.amazonaws.com@evil.example.com/cert.pem")]
    [InlineData("https://user@sns.us-east-2.amazonaws.com/cert.pem")]
    [InlineData("https://sns.us-east-2.amazonaws.com:8443/cert.pem")]
    [InlineData("https://s3.us-east-2.amazonaws.com/cert.pem")]
    [InlineData("https://sns.amazonaws.com.cn.evil.example.com/cert.pem")]
    [InlineData("https://sns.us-east-2.amazonaws.com/cert.txt")]
    [InlineData("https://sns.us-east-2.amazonaws.com/cert.pem?x=1")]
    [InlineData("/cert.pem")]
    [InlineData("")]
    public async Task UntrustedCertificateUrl_Fails_WithoutDownloading(string url)
    {
        var fields = Notification();

        fields["SigningCertURL"] = url;

        var handler = ServePem();

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(fields))));
        Assert.Equal(0, handler.Count);
    }

    [Theory]
    [InlineData("https://sns.us-east-1.amazonaws.com/SimpleNotificationService-1.pem")]
    [InlineData("https://sns.us-gov-west-1.amazonaws.com/SimpleNotificationService-1.pem")]
    [InlineData("https://sns.cn-north-1.amazonaws.com.cn/SimpleNotificationService-1.pem")]
    [InlineData("https://SNS.US-EAST-1.AMAZONAWS.COM/SimpleNotificationService-1.pem")]
    public async Task TrustedCertificateUrl_Verifies(string url)
    {
        var fields = Notification();

        fields["SigningCertURL"] = url;

        Assert.True(await CreateVerifier().VerifyAsync(ToJson(SignNotification(fields))));
    }

    [Fact]
    public async Task RedirectToUntrustedHost_Fails()
    {
        var handler = new CertHandler(
            () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(s_certPem) },
            new Uri("https://evil.example.com/cert.pem"));

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(Notification()))));
    }

    [Fact]
    public async Task CertificateNotFound_Fails()
    {
        var handler = new CertHandler(() => new HttpResponseMessage(HttpStatusCode.NotFound));

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(Notification()))));
    }

    [Fact]
    public async Task InvalidCertificate_Fails()
    {
        Assert.False(await CreateVerifier(ServePem("not a pem")).VerifyAsync(ToJson(SignNotification(Notification()))));
    }

    [Theory]
    [InlineData(-10, -1)]
    [InlineData(1, 10)]
    public async Task CertificateOutsideValidityPeriod_Fails(int notBeforeDays, int notAfterDays)
    {
        var handler = ServePem(CreateCertificatePem(s_key, notBeforeDays, notAfterDays));

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(Notification()))));
    }

    [Fact]
    public async Task NonRsaCertificate_Fails()
    {
        using var key = ECDsa.Create();

        using var certificate = new CertificateRequest("CN=sns.amazonaws.com", key, HashAlgorithmName.SHA256)
            .CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));

        var handler = ServePem(certificate.ExportCertificatePem());

        Assert.False(await CreateVerifier(handler).VerifyAsync(ToJson(SignNotification(Notification()))));
    }

    [Fact]
    public async Task Certificate_IsCachedByUrl()
    {
        var handler = ServePem();
        var verifier = CreateVerifier(handler);

        var json = ToJson(SignNotification(Notification()));

        Assert.True(await verifier.VerifyAsync(json));
        Assert.True(await verifier.VerifyAsync(json));

        Assert.Equal(1, handler.Count);
    }

    [Fact]
    public async Task FailedDownload_IsNotCached()
    {
        var calls = 0;

        var handler = new CertHandler(() => ++calls is 1
            ? new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(s_certPem) });

        var verifier = CreateVerifier(handler);

        var json = ToJson(SignNotification(Notification()));

        Assert.False(await verifier.VerifyAsync(json));
        Assert.True(await verifier.VerifyAsync(json));
    }

    [Fact]
    public async Task HonorsCancellation()
    {
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreateVerifier().VerifyAsync(ToJson(SignNotification(Notification())), new CancellationToken(canceled: true)));
    }

    [Fact]
    public void Constructors()
    {
        Assert.NotNull(new SnsMessageVerifier());
        Assert.Throws<ArgumentNullException>(() => new SnsMessageVerifier(null!));
    }
}
