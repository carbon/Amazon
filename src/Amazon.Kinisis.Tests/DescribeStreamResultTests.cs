using System.Text.Json;

using Xunit;

namespace Amazon.Kinesis.Tests
{
    public class DescribeStreamResultTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"{
  ""StreamDescription"": {
    ""HasMoreShards"": false,
    ""Shards"": [
      {
        ""HashKeyRange"": {
          ""EndingHashKey"": ""1"",
          ""StartingHashKey"": ""0""
        },
        ""SequenceNumberRange"": {
          ""StartingSequenceNumber"": ""1""
        },
        ""ShardId"": ""shardId-000000000000""
      },
      {
        ""HashKeyRange"": {
          ""EndingHashKey"": ""a"",
          ""StartingHashKey"": ""b""
        },
        ""SequenceNumberRange"": {
          ""StartingSequenceNumber"": ""1""
        },
        ""ShardId"": ""shardId-000000000001""
      }
    ],
    ""StreamARN"": ""arn:aws:kinesis:us-east-1:1234:stream/Hits"",
    ""StreamName"": ""Hits"",
    ""StreamStatus"": ""ACTIVE""
  }
}";

            var result = JsonSerializer.Deserialize<DescribeStreamResult>(text);

            Assert.False(result.StreamDescription.HasMoreShards);
            Assert.Equal("shardId-000000000000", result.StreamDescription.Shards[0].ShardId);
            Assert.Equal("1", result.StreamDescription.Shards[0].HashKeyRange.EndingHashKey);

            Assert.Equal("ACTIVE", result.StreamDescription.StreamStatus);
            Assert.Equal("Hits", result.StreamDescription.StreamName);
            Assert.Equal("arn:aws:kinesis:us-east-1:1234:stream/Hits", result.StreamDescription.StreamARN);
        }
    }
}
