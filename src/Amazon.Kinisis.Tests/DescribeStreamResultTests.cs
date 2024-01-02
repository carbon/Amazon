using System.Text.Json;

using Amazon.Kinesis.Serialization;

namespace Amazon.Kinesis.Tests;

public class DescribeStreamResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        DescribeStreamResult? result = JsonSerializer.Deserialize(
            """
            {
                "StreamDescription": {
                "HasMoreShards": false,
                "RetentionPeriodHours": 24,
                "StreamCreationTimestamp": 1.468346745E9,
                "Shards": [
                  {
                    "HashKeyRange": {
                      "EndingHashKey": "113427455640312821154458202477256070484",
                      "StartingHashKey": "0"
                    },
                    "SequenceNumberRange": {
                      "EndingSequenceNumber": "21269319989741826081360214168359141376",
                      "StartingSequenceNumber": "21267647932558653966460912964485513216"
                    },
                    "ShardId": "shardId-000000000000"
                  },
                  {
                    "HashKeyRange": {
                      "EndingHashKey": "226854911280625642308916404954512140969",
                      "StartingHashKey": "113427455640312821154458202477256070485"
                    },
                    "SequenceNumberRange": {
                      "StartingSequenceNumber": "21267647932558653966460912964485513217"
                    },
                    "ShardId": "shardId-000000000001"
                  },
                  {
                    "HashKeyRange": {
                      "EndingHashKey": "340282366920938463463374607431768211455",
                      "StartingHashKey": "226854911280625642308916404954512140970"
                    },
                    "SequenceNumberRange": {
                      "StartingSequenceNumber": "21267647932558653966460912964485513218"
                    },
                    "ShardId": "shardId-000000000002"
                  }
                ],
                "StreamARN": "arn:aws:kinesis:us-east-1:1234:stream/Hits",
                "StreamName": "Hits",
                "StreamStatus": "ACTIVE"
                }
            }
            """, KinesisSerializerContext.Default.DescribeStreamResult);

        Assert.NotNull(result);
        Assert.False(result.StreamDescription.HasMoreShards);
        Assert.Equal("shardId-000000000000", result.StreamDescription.Shards[0].ShardId);
        Assert.Equal("113427455640312821154458202477256070484", result.StreamDescription.Shards[0].HashKeyRange.EndingHashKey);

        Assert.Equal("ACTIVE", result.StreamDescription.StreamStatus);
        Assert.Equal("Hits", result.StreamDescription.StreamName);
        Assert.Equal("arn:aws:kinesis:us-east-1:1234:stream/Hits", result.StreamDescription.StreamARN);
    }
}