using System.Text.Json;

using Amazon.Kinesis.Serialization;

namespace Amazon.Kinesis.Tests;

public class GetShardIteratorResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        GetShardIteratorResult? result = JsonSerializer.Deserialize(
            """
            {
                "ShardIterator": "string"
            }
            """, KinesisSerializerContext.Default.GetShardIteratorResult);

        Assert.NotNull(result);

        Assert.Equal("string", result.ShardIterator);
    }
}