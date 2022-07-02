using System.Text.Json;

namespace Amazon.Kinesis.Firehose;

public class PutRecordBatchResultTests
{
    [Fact]
    public void Deserialize()
    {
        var text = """{"FailedPutCount":0,"RequestResponses":[{"RecordId":"r1"},{"RecordId":"r2"}]}""";

        var result = JsonSerializer.Deserialize<PutRecordBatchResult>(text);

        Assert.Equal("r1", result.RequestResponses[0].RecordId);
        Assert.Equal("r2", result.RequestResponses[1].RecordId);

        Assert.Equal(0, result.FailedPutCount);
        Assert.Equal(2, result.RequestResponses.Length);
    }
}