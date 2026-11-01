using System.Globalization;

namespace Amazon.Sts.Tests;

public class CallerIdentityVerificationParametersAgeTests
{
    private static CallerIdentityVerificationParameters Parameters(Dictionary<string, string> headers)
    {
        return new CallerIdentityVerificationParameters("https://sts.us-east-1.amazonaws.com/", headers, "body");
    }

    private static CallerIdentityVerificationParameters SignedAt(DateTime date)
    {
        return Parameters(new() { { "x-amz-date", date.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture) } });
    }

    [Fact]
    public void GetAge_TreatsDateAsUtc()
    {
        var age = SignedAt(DateTime.UtcNow.AddHours(-1)).GetAge();

        Assert.InRange(age, TimeSpan.FromMinutes(59), TimeSpan.FromMinutes(61));
    }

    [Fact]
    public void GetAge_IsZeroForFutureDate()
    {
        Assert.Equal(TimeSpan.Zero, SignedAt(DateTime.UtcNow.AddHours(1)).GetAge());
    }

    [Fact]
    public void GetAge_ThrowsWhenDateIsMissing()
    {
        Assert.Throws<KeyNotFoundException>(() => Parameters([]).GetAge());
    }

    [Theory]
    [InlineData("")]
    [InlineData("not a date")]
    [InlineData("2030-01-02T03:04:05Z")]
    public void GetAge_ThrowsWhenDateIsMalformed(string date)
    {
        Assert.Throws<FormatException>(() => Parameters(new() { { "x-amz-date", date } }).GetAge());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_RequiresUrl(string? url)
    {
        Assert.ThrowsAny<ArgumentException>(() => new CallerIdentityVerificationParameters(url!, new Dictionary<string, string>(), "body"));
    }

    [Fact]
    public void Constructor_RequiresHeadersAndBody()
    {
        Assert.Throws<ArgumentNullException>(() => new CallerIdentityVerificationParameters("url", null!, "body"));
        Assert.Throws<ArgumentNullException>(() => new CallerIdentityVerificationParameters("url", new Dictionary<string, string>(), null!));
    }

    [Fact]
    public async Task ClientParameters_TargetRegionalEndpointWithoutSessionToken()
    {
        var parameters = await new StsClient(AwsRegion.EUWest1, FakeSts.Credential).GetCallerIdentityVerificationParametersAsync();

        Assert.Equal("https://sts.eu-west-1.amazonaws.com/", parameters.Url);
        Assert.Equal("Action=GetCallerIdentity&Version=2011-06-15", parameters.Body);

        // No credential is exposed beyond the signature
        Assert.Equal(["Authorization", "x-amz-date"], parameters.Headers.Keys.Order(StringComparer.Ordinal));
        Assert.DoesNotContain(FakeSts.SecretAccessKey, parameters.Headers["Authorization"]);
        Assert.InRange(parameters.GetAge(), TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }
}
