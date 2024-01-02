using System.Text.Json;

using Amazon.Kinesis.Serialization;

namespace Amazon.Kinesis.Tests;

public class PutRecordsResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = JsonSerializer.Deserialize(
            """
            {
                "FailedRecordCount": 0,
                "Records": [
                    {
                        "SequenceNumber": "49543463076548007577105092703039560359975228518395019266",
                        "ShardId": "shardId-000000000000"
                    },
                    {
                        "SequenceNumber": "49543463076570308322303623326179887152428262250726293522",
                        "ShardId": "shardId-000000000001"
                    },
                    {
                        "SequenceNumber": "49543463076570308322303623326179887152428262250726293588",
                        "ShardId": "shardId-000000000003"
                    }
                ]
            }
            """, KinesisSerializerContext.Default.PutRecordsResult);

        Assert.NotNull(result);
        Assert.Equal(3, result.Records.Count);
        Assert.Equal("49543463076548007577105092703039560359975228518395019266", result.Records[0].SequenceNumber);
        Assert.Equal("shardId-000000000000", result.Records[0].ShardId);
    }
}