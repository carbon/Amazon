using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class ReceiveMessagesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new ReceiveMessageRequest(
            queueUrl            : "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
            maxNumberOfMessages : 5,
            visibilityTimeout   : TimeSpan.FromSeconds(10), 
            waitTime            : TimeSpan.FromSeconds(20)
        );

        Assert.Null(request.AttributeNames);
        Assert.Null(request.MessageAttributeNames);

        Assert.Equal(5,  request.MaxNumberOfMessages);
        Assert.Equal(10, request.VisibilityTimeout);
        Assert.Equal(20, request.WaitTimeSeconds);

        Assert.Equal(
            """
            {
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
              "MaxNumberOfMessages": 5,
              "VisibilityTimeout": 10,
              "WaitTimeSeconds": 20
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.ReceiveMessageRequest));
    }

   
    [Fact]
    public void CanSerializeAttributeNames()
    {
        var request = new ReceiveMessageRequest(
            queueUrl                : "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
            maxNumberOfMessages     : 5,
            visibilityTimeout       : TimeSpan.FromSeconds(15),
            attributeNames          : ["All"],
            messageAttributeNames   : ["id"]
        );

        Assert.Equal(
            """
            {
              "AttributeNames": [
                "All"
              ],
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
              "MaxNumberOfMessages": 5,
              "MessageAttributeNames": [
                "id"
              ],
              "VisibilityTimeout": 15
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.ReceiveMessageRequest));
    }

    [Fact]
    public void ThrowsWhenTakeIsOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReceiveMessageRequest("queue-url", 0,  visibilityTimeout: TimeSpan.FromSeconds(10), waitTime: TimeSpan.FromSeconds(20)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReceiveMessageRequest("queue-url", 11, visibilityTimeout: TimeSpan.FromSeconds(10), waitTime: TimeSpan.FromSeconds(20)));
    }
}