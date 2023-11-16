using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class SendMessageRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new SendMessageRequest(
            queueUrl: "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
            messageBody: "This is a test message",
            messageAttributes: new() {
                { "my_attribute_name_1", new("my_attribute_value_1") },
                { "my_attribute_name_2", new("my_attribute_value_2") },
                { "id", 3 }
            }
        );

        Assert.Equal(
            """
            {
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
              "MessageAttributes": {
                "my_attribute_name_1": {
                  "DataType": "String",
                  "StringValue": "my_attribute_value_1"
                },
                "my_attribute_name_2": {
                  "DataType": "String",
                  "StringValue": "my_attribute_value_2"
                },
                "id": {
                  "DataType": "Number",
                  "StringValue": "3"
                }
              },
              "MessageBody": "This is a test message"
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.SendMessageRequest));
    }

    [Fact]
    public void CanSerializeDelaySeconds()
    {
        var request = new SendMessageRequest(
            queueUrl    : "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
            messageBody : "This is a test message",
            delay       : TimeSpan.FromSeconds(0)
        );

        Assert.Equal(
            """
            {
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
              "DelaySeconds": 0,
              "MessageBody": "This is a test message"
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.SendMessageRequest));
    }
}