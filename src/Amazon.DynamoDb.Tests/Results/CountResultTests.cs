using System.Text.Json;

namespace Amazon.DynamoDb.Results.Tests;

public class CountResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = JsonSerializer.Deserialize<CountResult>("""{ "Count": 17 }""");

        Assert.Equal(17, result.Count);
    }
}