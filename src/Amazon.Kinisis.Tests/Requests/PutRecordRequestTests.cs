using System.Text.Json;

namespace Amazon.Kinesis.Firehose;

public class PutRecordRequestTests
{
    private static JsonSerializerOptions _indented = new() { WriteIndented = true };

    [Fact]
    public void CanSerialize()
    {
        var putRecord = new PutRecordRequest("Events", new Record("hello"u8.ToArray()));

        Assert.Equal(
            """
            {
              "DeliveryStreamName": "Events",
              "Record": {
                "Data": "aGVsbG8="
              }
            }
            """, JsonSerializer.Serialize(putRecord, _indented));
    }
}