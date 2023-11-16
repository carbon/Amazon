using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class DeleteMessageBatchResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        DeleteMessageBatchResult? result = JsonSerializer.Deserialize(
            """
            {
                "Failed": [],
                "Successful": [
                    {
                        "Id": "msg2"
                    },
                    {
                        "Id": "msg1"
                    }
                ]
            }
            """u8, SqsSerializerContext.Default.DeleteMessageBatchResult);

        Assert.NotNull(result);
        Assert.Empty(result.Failed);
        Assert.Equal(2, result.Successful.Length);

        Assert.Equal("msg2", result.Successful[0].Id);
        Assert.Equal("msg1", result.Successful[1].Id);
    }
}
