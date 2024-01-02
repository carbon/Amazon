using System.Text.Json;

namespace Amazon.Sts.Tests;

public class CallerIdentityVerificationParametersTests
{
    private static CallerIdentityVerificationParameters GetMock()
    {
        return new CallerIdentityVerificationParameters
        {
            Body = "body",
            Headers = new Dictionary<string, string>
                {
                    { "a", "1" },
                    { "b", "2" }
                },
            Url = "url"
        };
    }

    [Fact]
    public void CanRoundtrip()
    {
        var a = GetMock();

        Assert.Equal("""{"url":"url","headers":{"a":"1","b":"2"},"body":"body"}""", JsonSerializer.Serialize(a));       
        Assert.Equal("""{"url":"url","headers":{"a":"1","b":"2"},"body":"body"}""", JsonSerializer.Serialize(a, StsSerializerContext.Default.CallerIdentityVerificationParameters));

        var b = JsonSerializer.Deserialize<CallerIdentityVerificationParameters>(JsonSerializer.Serialize(a));

        Assert.NotNull(b);
        Assert.Equal("body", b.Body);
        Assert.Equal("url", b.Url);

        Assert.Equal("1", b.Headers["a"]);
        Assert.Equal("2", b.Headers["b"]);

        Assert.Equal(typeof(Dictionary<string, string>), b.Headers.GetType());
    }

    [Fact]
    public void CanRoundtripWithJsonContext()
    {
        var a = GetMock();

        var json = """{"url":"url","headers":{"a":"1","b":"2"},"body":"body"}""";

        Assert.Equal(json, JsonSerializer.Serialize(a, StsSerializerContext.Default.CallerIdentityVerificationParameters));

        var b = JsonSerializer.Deserialize(json, StsSerializerContext.Default.CallerIdentityVerificationParameters);

        Assert.NotNull(b);
        Assert.Equal("body", b.Body);
        Assert.Equal("url", b.Url);

        Assert.Equal("1", b.Headers["a"]);
        Assert.Equal("2", b.Headers["b"]);        
    }
}
