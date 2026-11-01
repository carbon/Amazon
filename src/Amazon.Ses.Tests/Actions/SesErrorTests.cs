using System.Text.Json;

namespace Amazon.Ses.Actions.Tests;

public class SesErrorTests
{
    [Fact]
    public void SupportsNull()
    {
        var a = JsonSerializer.Deserialize<SesError>("""{"message":null}""");

        Assert.Equal(null, a.Message);
    }
}
