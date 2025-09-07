namespace Amazon.Kinesis.Firehose;

public class PutRecordRequestTests
{
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
            """, putRecord.ToIndentedJsonString(), ignoreLineEndingDifferences: true);
    }
}