using System.Text.Json;

namespace Amazon.Kinesis.Firehose;

public class PutRecordResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = JsonSerializer.Deserialize<PutRecordResult>(
            """
            {
                "Encrypted": false,
                "RecordId": "id"
            }
            """u8);

        Assert.NotNull(result);
        Assert.False(result.Encrypted);
        Assert.Equal("id", result.RecordId);
    }
}