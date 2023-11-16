using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class SendMessageBatchRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new SendMessageBatchRequest("queue-url", [
            new SendMessageBatchRequestEntry("1", "message 1"),
            new SendMessageBatchRequestEntry("2", "message 2", delay: TimeSpan.FromSeconds(60), messageAttributes: new() { ["data"] = new(Convert.FromBase64String("3ojpITEowNKFy0FkIUTr/0xhRTPf6OD2vlzur2p05KU=")) })
        ]);

        Assert.Equal(
            """
            {
              "QueueUrl": "queue-url",
              "Entries": [
                {
                  "Id": "1",
                  "MessageBody": "message 1"
                },
                {
                  "Id": "2",
                  "MessageAttributes": {
                    "data": {
                      "DataType": "Binary",
                      "BinaryValue": "3ojpITEowNKFy0FkIUTr/0xhRTPf6OD2vlzur2p05KU="
                    }
                  },
                  "MessageBody": "message 2",
                  "DelaySeconds": 60
                }
              ]
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.SendMessageBatchRequest)
        );

        Assert.Equal(
            """
            {"QueueUrl":"queue-url","Entries":[{"Id":"1","MessageBody":"message 1"},{"Id":"2","MessageAttributes":{"data":{"DataType":"Binary","BinaryValue":"3ojpITEowNKFy0FkIUTr/0xhRTPf6OD2vlzur2p05KU="}},"MessageBody":"message 2","DelaySeconds":60}]}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, SqsSerializerContext.Default.SendMessageBatchRequest));
    }

    [Fact]
    public void CanSerializeBasicMessages()
    {
        var request = new SendMessageBatchRequest("queue-url", [
            "message 1",
            "message 2"
        ]);

        Assert.Equal(
            """
            {
              "QueueUrl": "queue-url",
              "Entries": [
                {
                  "Id": "1",
                  "MessageBody": "message 1"
                },
                {
                  "Id": "2",
                  "MessageBody": "message 2"
                }
              ]
            }
            """, request.ToIndentedJsonString()
        );
    }
}
