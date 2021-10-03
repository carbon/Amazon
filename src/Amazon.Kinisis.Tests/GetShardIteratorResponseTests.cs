using System.Text.Json;

namespace Amazon.Kinesis.Tests
{
    public class GetShardIteratorResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"{
                ""ShardIterator"": ""string""
            }";

            var result = JsonSerializer.Deserialize<GetShardIteratorResponse>(text);

            Assert.Equal("string", result.ShardIterator);
        }
    }
}
