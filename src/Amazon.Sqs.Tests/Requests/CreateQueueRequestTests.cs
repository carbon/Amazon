using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class CreateQueueRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new CreateQueueRequest("queue-name");

        Assert.Equal("""{"QueueName":"queue-name"}"""u8, JsonSerializer.SerializeToUtf8Bytes(request, SqsSerializerContext.Default.CreateQueueRequest));

        request.Tags = new() {
            { "QueueType", "Production" }
        };

        Assert.Equal(
         """
          {"QueueName":"queue-name","tags":{"QueueType":"Production"}}
          """u8, JsonSerializer.SerializeToUtf8Bytes(request, SqsSerializerContext.Default.CreateQueueRequest));


        request = new CreateQueueRequest("queue-name", new QueueAttributes {
            VisibilityTimeout = 40,
            DelaySeconds = 10
        });

        Assert.Equal(
        """
        {"QueueName":"queue-name","Attributes":{"DelaySeconds":"10","VisibilityTimeout":"40"}}
        """u8, JsonSerializer.SerializeToUtf8Bytes(request, SqsSerializerContext.Default.CreateQueueRequest));
    }

    [Fact]
    public void CanSerializeSecondaryConstructor()
    {
        var request = new CreateQueueRequest("queue-name", TimeSpan.FromSeconds(60));

        var expected = """{"QueueName":"queue-name","Attributes":{"VisibilityTimeout":"60"}}"""u8;

        Assert.Equal(expected, JsonSerializer.SerializeToUtf8Bytes(request, SqsSerializerContext.Default.CreateQueueRequest));
    }
}
