using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class CreateQueueResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        CreateQueueResult? result = JsonSerializer.Deserialize(
            """
            {
                "QueueUrl":"https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue"
            }
            """u8, SqsSerializerContext.Default.CreateQueueResult);

        Assert.NotNull(result);
        Assert.Equal("https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue", result.QueueUrl);
    }
}