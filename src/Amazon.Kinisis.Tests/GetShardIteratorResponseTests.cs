using System.Text.Json;

namespace Amazon.Kinesis.Tests;

public class GetShardIteratorResponseTests
{
    [Fact]
    public void Deserialize()
    {
        var result = JsonSerializer.Deserialize<GetShardIteratorResponse>(
            """
            {
                "ShardIterator": "string"
            }
            """);

        Assert.Equal("string", result.ShardIterator);
    }
}