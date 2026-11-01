using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Amazon.Metadata.Tests;

// The service is a singleton with no way to supply an HttpClient, so these tests replace its private fields.
// This is the only test class that uses it, and xUnit runs the tests within a class one at a time.
public sealed class InstanceMetadataServiceTests : IDisposable
{
    private const string Base = "http://169.254.169.254/latest/meta-data";

    private const string CredentialsJson = """
        {
          "Code": "Success",
          "LastUpdated": "2020-01-01T00:00:00Z",
          "Type": "AWS-HMAC",
          "AccessKeyId": "key",
          "SecretAccessKey": "secret",
          "Token": "session",
          "Expiration": "2999-01-01T00:00:00Z"
        }
        """;

    private static readonly InstanceMetadataService Service = InstanceMetadataService.Instance;

    private readonly HttpClient _originalClient = GetField<HttpClient>(Service, "httpClient");

    public InstanceMetadataServiceTests()
    {
        SetField(Service, "_token", null);
        ClearCurrentRegion();
    }

    public void Dispose()
    {
        SetField(Service, "httpClient", _originalClient);
        SetField(Service, "_token", null);
        ClearCurrentRegion();
    }

    private static T GetField<T>(object target, string name)
    {
        return (T)target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(target)!;
    }

    private static void SetField(object target, string name, object value)
    {
        target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(target, value);
    }

    private static void ClearCurrentRegion()
    {
        typeof(AwsRegion).GetField("current", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, null);
    }

    private static HttpResponseMessage Text(string text, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) { Content = new StringContent(text, Encoding.UTF8) };
    }

    private static MetadataHandler Serve(Func<string, HttpResponseMessage> get)
    {
        var handler = new MetadataHandler(get);

        SetField(Service, "httpClient", new HttpClient(handler));

        return handler;
    }

    private sealed class MetadataHandler(Func<string, HttpResponseMessage> get) : HttpMessageHandler
    {
        public Func<HttpResponseMessage> TokenResponse { get; set; }

        public int TokenRequests { get; private set; }

        public List<string> Gets { get; } = [];

        public List<string> TokensSent { get; } = [];

        public string LastTtl { get; private set; }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Put)
            {
                Assert.Equal("http://169.254.169.254/latest/api/token", request.RequestUri!.ToString());

                TokenRequests++;

                LastTtl = request.Headers.GetValues("X-aws-ec2-metadata-token-ttl-seconds").Single();

                return TokenResponse?.Invoke() ?? Text($"token{TokenRequests}");
            }

            Gets.Add(request.RequestUri!.ToString());
            TokensSent.Add(request.Headers.GetValues("X-aws-ec2-metadata-token").Single());

            return get(request.RequestUri!.ToString());
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request, cancellationToken));
        }
    }

    [Fact]
    public async Task RequestsAndReusesToken()
    {
        var handler = Serve(_ => Text("value"));

        await Service.GetInstanceIdAsync();
        await Service.GetInstanceIdAsync();

        Assert.Equal(1, handler.TokenRequests);
        Assert.Equal("3600", handler.LastTtl);
        Assert.Equal(["token1", "token1"], handler.TokensSent);
    }

    [Fact]
    public async Task RenewsTokenNearExpiration()
    {
        var handler = Serve(_ => Text("value"));

        SetField(Service, "_token", new MetadataToken("old", DateTime.UtcNow.AddMinutes(4)));

        await Service.GetInstanceIdAsync();

        Assert.Equal(["token1"], handler.TokensSent);

        SetField(Service, "_token", new MetadataToken("old", DateTime.UtcNow.AddMinutes(4)));

        Assert.Equal("value", Service.GetAvailabilityZone());
        Assert.Equal(["token1", "token2"], handler.TokensSent);
    }

    [Fact]
    public async Task ReadsStrings()
    {
        var handler = Serve(_ => Text("value"));

        Assert.Equal("value", await Service.GetInstanceIdAsync());
        Assert.Equal("value", await Service.GetAvailabilityZoneAsync());
        Assert.Equal("value", await Service.GetUserDataStringAsync());
        Assert.Equal("value"u8.ToArray(), await Service.GetUserDataAsync());

        Assert.Equal([
            $"{Base}/instance-id",
            $"{Base}/placement/availability-zone",
            $"{Base}/user-data",
            $"{Base}/user-data"
        ], handler.Gets);
    }

    [Fact]
    public async Task IamRoleName_IsNullWhenNotConfigured()
    {
        Serve(_ => Text("", HttpStatusCode.NotFound));

        Assert.Null(await Service.GetIamRoleNameAsync());
        Assert.Null(Service.GetIamRoleName());
    }

    [Fact]
    public async Task IamRoleName_ReadsName()
    {
        var handler = Serve(_ => Text("role"));

        Assert.Equal("role", await Service.GetIamRoleNameAsync());
        Assert.Equal("role", Service.GetIamRoleName());
        Assert.All(handler.Gets, static url => Assert.Equal($"{Base}/iam/security-credentials/", url));
    }

    [Fact]
    public async Task PublicIpv4()
    {
        Serve(_ => Text("1.2.3.4"));

        Assert.Equal(IPAddress.Parse("1.2.3.4"), await Service.GetPublicIpv4Async());

        Serve(_ => Text(""));

        Assert.Null(await Service.GetPublicIpv4Async());

        Serve(_ => Text("", HttpStatusCode.NotFound));

        Assert.Null(await Service.GetPublicIpv4Async());
    }

    [Fact]
    public async Task SpotInstanceAction()
    {
        Serve(_ => Text("""{"action":"terminate","time":"2017-09-18T08:22:00Z"}"""));

        Assert.Equal("terminate", (await Service.GetSpotInstanceActionOrDefaultAsync()).Action);

        Serve(_ => Text(""));

        Assert.Null(await Service.GetSpotInstanceActionOrDefaultAsync());

        Serve(_ => Text("", HttpStatusCode.NotFound));

        Assert.Null(await Service.GetSpotInstanceActionOrDefaultAsync());
    }

    [Fact]
    public async Task InstanceIdentityDocument()
    {
        var handler = Serve(_ => Text("""{"instanceId":"i-1","region":"us-east-1"}"""));

        var identity = await InstanceIdentity.GetAsync();

        Assert.Equal("i-1", identity.InstanceId);
        Assert.Equal("http://169.254.169.254/latest/dynamic/instance-identity/document", handler.Gets[0]);
    }

    [Fact]
    public async Task Credentials_Async_RetriesThenReads()
    {
        int count = 0;

        var handler = Serve(_ => ++count < 3 ? Text("", HttpStatusCode.InternalServerError) : Text(CredentialsJson));

        var credentials = await Service.GetIamSecurityCredentialsAsync("role");

        Assert.Equal("key", credentials.AccessKeyId);
        Assert.Equal(3, handler.Gets.Count);
        Assert.Equal($"{Base}/iam/security-credentials/role", handler.Gets[0]);
    }

    [Fact]
    public async Task Credentials_Async_ThrowsAfterThreeAttempts()
    {
        var handler = Serve(_ => Text("", HttpStatusCode.InternalServerError));

        var ex = await Assert.ThrowsAsync<Exception>(() => Service.GetIamSecurityCredentialsAsync("role"));

        Assert.Equal("error fetching security-credentials", ex.Message);
        Assert.NotNull(ex.InnerException);
        Assert.Equal(3, handler.Gets.Count);
    }

    [Fact]
    public void Credentials_Sync_RetriesWithNewToken()
    {
        int count = 0;

        var handler = Serve(_ => ++count < 2 ? Text("", HttpStatusCode.Unauthorized) : Text(CredentialsJson));

        var credentials = Service.GetIamSecurityCredentials("role");

        Assert.Equal("secret", credentials.SecretAccessKey);
        Assert.Equal(["token1", "token2"], handler.TokensSent);
    }

    [Fact]
    public void Credentials_Sync_ThrowsAfterThreeAttempts()
    {
        var handler = Serve(_ => Text("", HttpStatusCode.InternalServerError));

        var ex = Assert.Throws<Exception>(() => Service.GetIamSecurityCredentials("role"));

        Assert.Equal("error getting security credentials", ex.Message);
        Assert.Equal(3, handler.Gets.Count);
    }

    [Fact]
    public async Task Credentials_Async_RetriesWithNewToken()
    {
        int count = 0;

        var handler = Serve(_ => ++count < 2 ? Text("", HttpStatusCode.Unauthorized) : Text(CredentialsJson));

        await Service.GetIamSecurityCredentialsAsync("role");

        Assert.Equal(["token1", "token2"], handler.TokensSent);
    }

    [Fact]
    public async Task Token_Async_ThrowsOnError()
    {
        var handler = Serve(_ => Text("value"));

        handler.TokenResponse = () => Text("Forbidden", HttpStatusCode.Forbidden);

        await Assert.ThrowsAnyAsync<Exception>(() => Service.GetInstanceIdAsync());

        Assert.Empty(handler.Gets);
    }

    [Fact]
    public void Token_Sync_ThrowsOnError()
    {
        var handler = Serve(_ => Text("value"));

        handler.TokenResponse = () => Text("Forbidden", HttpStatusCode.Forbidden);

        Assert.ThrowsAny<Exception>(() => Service.GetAvailabilityZone());

        Assert.Empty(handler.Gets);
    }

    [Fact]
    public async Task Strings_Async_ThrowOnServerError()
    {
        Serve(_ => Text("<html>Error</html>", HttpStatusCode.InternalServerError));

        await Assert.ThrowsAnyAsync<Exception>(() => Service.GetAvailabilityZoneAsync());
    }

    [Fact]
    public void Strings_Sync_ThrowOnServerError()
    {
        Serve(_ => Text("<html>Error</html>", HttpStatusCode.InternalServerError));

        Assert.ThrowsAny<Exception>(() => Service.GetAvailabilityZone());
    }

    [Fact]
    public void AwsRegion_Get_ReadsAndCachesAvailabilityZone()
    {
        var handler = Serve(_ => Text("us-west-2b"));

        Assert.Same(AwsRegion.USWest2, AwsRegion.Get());
        Assert.Same(AwsRegion.USWest2, AwsRegion.Get());

        Assert.Single(handler.Gets);
    }

    [Fact]
    public async Task AwsRegion_GetAsync_ReadsAndCachesAvailabilityZone()
    {
        var handler = Serve(_ => Text("eu-west-1a"));

        Assert.Same(AwsRegion.EUWest1, await AwsRegion.GetAsync());
        Assert.Same(AwsRegion.EUWest1, await AwsRegion.GetAsync());

        Assert.Single(handler.Gets);
    }

    private static HttpResponseMessage RoleOrCredentials(string url)
    {
        return Text(url.EndsWith("/security-credentials/", StringComparison.Ordinal) ? "role" : CredentialsJson);
    }

    [Fact]
    public void RoleCredential_Get()
    {
        Serve(RoleOrCredentials);

        var credential = InstanceRoleCredential.Get();

        Assert.Equal("role", credential.RoleName);
        Assert.Equal("key", credential.AccessKeyId);
    }

    [Fact]
    public async Task RoleCredential_GetAsync()
    {
        Serve(RoleOrCredentials);

        var credential = await InstanceRoleCredential.GetAsync();

        Assert.Equal("role", credential.RoleName);
        Assert.Equal("session", credential.SecurityToken);
        Assert.Equal(new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc), credential.Modified);
    }

    [Fact]
    public async Task RoleCredential_GetAsync_ByName()
    {
        var handler = Serve(RoleOrCredentials);

        var credential = await InstanceRoleCredential.GetAsync("named");

        Assert.Equal("named", credential.RoleName);
        Assert.Equal($"{Base}/iam/security-credentials/named", Assert.Single(handler.Gets));

        await Assert.ThrowsAsync<ArgumentNullException>(() => InstanceRoleCredential.GetAsync(null));
    }

    [Fact]
    public async Task RoleCredential_RenewAsync_DiscoversRole()
    {
        Serve(RoleOrCredentials);

        var credential = new InstanceRoleCredential();

        Assert.True(await credential.RenewAsync());

        Assert.Equal("role", credential.RoleName);
        Assert.Equal("key", credential.AccessKeyId);
        Assert.Equal(1, credential.RenewCount);
        Assert.False(credential.ShouldRenew);
        Assert.True(credential.ExpiresIn > TimeSpan.Zero);
    }

    [Fact]
    public async Task RoleCredential_RenewAsync_DoesNothingWhenValid()
    {
        var handler = Serve(RoleOrCredentials);

        var credential = await InstanceRoleCredential.GetAsync("role");

        int requests = handler.Gets.Count;

        Assert.False(await credential.RenewAsync());

        Assert.Equal(requests, handler.Gets.Count);
        Assert.Equal(0, credential.RenewCount);
    }

    [Fact]
    public async Task RoleCredential_RenewAsync_OnlyRenewsOnceWhenConcurrent()
    {
        Serve(RoleOrCredentials);

        var credential = new InstanceRoleCredential("role");

        await Task.WhenAll(credential.RenewAsync(), credential.RenewAsync(), credential.RenewAsync());

        Assert.Equal(1, credential.RenewCount);
    }

    [Fact]
    public async Task RoleCredential_RenewAsync_ThrowsWithoutRole()
    {
        Serve(_ => Text("", HttpStatusCode.NotFound));

        var ex = await Assert.ThrowsAsync<Exception>(() => new InstanceRoleCredential().RenewAsync());

        Assert.Equal("The instance is not configured with an IAM role", ex.Message);
    }

    [Fact]
    public void RoleCredential_RequiresRoleName()
    {
        Assert.Throws<ArgumentNullException>(() => new InstanceRoleCredential(null));
        Assert.Throws<ArgumentException>(() => new InstanceRoleCredential(""));
    }

    [Fact]
    public void MetadataToken_RequiresValue()
    {
        Assert.Throws<ArgumentNullException>(() => new MetadataToken(null, DateTime.UtcNow));
    }

    [Fact]
    public void IamRoleInfo_ReadsProfileName()
    {
        var info = new IamRoleInfo {
            Code = "Success",
            LastUpdated = new DateTime(2020, 01, 01),
            InstanceProfileArn = "arn:aws:iam::123:instance-profile/web",
            InstanceProfileId = "id"
        };

        Assert.Equal("web", info.ProfileName);
    }

    [Fact]
    public async Task Errors_ClearCachedToken()
    {
        int count = 0;

        var handler = Serve(_ => ++count is 1 ? Text("", HttpStatusCode.Unauthorized) : Text("value"));

        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetInstanceIdAsync());

        Assert.Equal("value", await Service.GetInstanceIdAsync());
        Assert.Equal(["token1", "token2"], handler.TokensSent);
    }

    [Fact]
    public async Task Errors_ThrowForOtherReads()
    {
        Serve(_ => Text("Error", HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetUserDataAsync());
        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetUserDataStringAsync());
        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetInstanceIdentityAsync());
        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetSpotInstanceActionOrDefaultAsync());
        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetPublicIpv4Async());
        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetIamRoleNameAsync());
        Assert.Throws<Amazon.Exceptions.AwsException>(() => Service.GetIamRoleName());
    }

    [Fact]
    public async Task Errors_DoNotCacheRegion()
    {
        int count = 0;

        Serve(_ => ++count is 1 ? Text("Error", HttpStatusCode.InternalServerError) : Text("us-west-2b"));

        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(async () => await AwsRegion.GetAsync());

        Assert.Same(AwsRegion.USWest2, await AwsRegion.GetAsync());
    }

    [Fact]
    public async Task Errors_ForTokenAreNotCached()
    {
        int count = 0;

        var handler = Serve(_ => Text("value"));

        handler.TokenResponse = () => ++count is 1 ? Text("Forbidden", HttpStatusCode.Forbidden) : Text("good");

        await Assert.ThrowsAsync<Amazon.Exceptions.AwsException>(() => Service.GetInstanceIdAsync());

        Assert.Equal("value", await Service.GetInstanceIdAsync());
        Assert.Equal(["good"], handler.TokensSent);
    }
}
