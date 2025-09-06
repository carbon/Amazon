using System.Text.Json;

using Amazon.Kinesis.Serialization;

namespace Amazon.Kinesis.Tests;

public class GetRecordsResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = JsonSerializer.Deserialize(
            """
            {
              "MillisBehindLatest": 2100,
              "NextShardIterator": "AAAAAAAAAAHsW8zCWf9164uy8Epue6WS3w6wmj4a4USt+CNvMd6uXQ+HL5vAJMznqqC0DLKsIjuoiTi1BpT6nW0LN2M2D56zM5H8anHm30Gbri9ua+qaGgj+3XTyvbhpERfrezgLHbPB/rIcVpykJbaSj5tmcXYRmFnqZBEyHwtZYFmh6hvWVFkIwLuMZLMrpWhG5r5hzkE=",
              "Records": [
                {
                  "Data": "XzxkYXRhPl8w",
                  "PartitionKey": "partitionKey",
                  "ApproximateArrivalTimestamp": 1.441215410867E9,
                  "SequenceNumber": "21269319989652663814458848515492872193"
                }
              ]
            }
            """u8, KinesisSerializerContext.Default.GetRecordsResult);

        Assert.NotNull(result);
        Assert.Single(result.Records);
        Assert.Equal(2100, result.MillisBehindLatest);

        Assert.Equal("partitionKey", result.Records[0].PartitionKey);

        Assert.Equal("21269319989652663814458848515492872193", result.Records[0].SequenceNumber);            
    }
}