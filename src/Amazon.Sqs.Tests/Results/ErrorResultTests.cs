using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class ErrorResultTests
{
    [Fact]
    public void CanDeserializeQueueDeletedRecently()
    {
        ErrorResult? result = JsonSerializer.Deserialize(
            """
            {
              "__type":"com.amazonaws.sqs#QueueDeletedRecently",
              "message":"You must wait 60 seconds after deleting a queue before you can create another with the same name."
            }
            """u8, SqsSerializerContext.Default.ErrorResult);


        Assert.NotNull(result);
        Assert.Equal("com.amazonaws.sqs#QueueDeletedRecently", result.Type);
        Assert.Equal("You must wait 60 seconds after deleting a queue before you can create another with the same name.", result.Message);
    }

    [Fact]
    public void CanDeserializeQueueDoesNotExist()
    {
        ErrorResult? result = JsonSerializer.Deserialize(
            """            
            {
                "__type": "com.amazonaws.sqs#QueueDoesNotExist",
                "message": "The specified queue does not exist."
            }            
            """u8, SqsSerializerContext.Default.ErrorResult);

        Assert.NotNull(result);
        Assert.Equal("com.amazonaws.sqs#QueueDoesNotExist", result.Type);
        Assert.Equal("The specified queue does not exist.", result.Message);
    }
}
