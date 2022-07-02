using System.Text.Json;

namespace Amazon.Kinesis.Firehose;

public class PutRecordResultTests
{
    [Fact]
    public void Deserialize()
    {
        var result = JsonSerializer.Deserialize<PutRecordResult>(
            """
            {
                "Encrypted": false,
                "RecordId": "id"
            }
            """);

        Assert.False(result.Encrypted);
        Assert.Equal("id", result.RecordId);
    }
}