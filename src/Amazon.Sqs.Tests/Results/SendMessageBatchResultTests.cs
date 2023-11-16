using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class SendMessageBatchResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        SendMessageBatchResult? result = JsonSerializer.Deserialize(
            """
            {
              "Failed": [],
              "Successful": [
                 {
                   "Id": "test_msg_001",
                   "MD5OfMessageBody": "0e024d309850c78cba5eabbeff7cae71",
                   "MessageId": "f4eb349f-cd33-4bc4-bdc2-e557c900d41d"
                 },
                 {
                   "Id": "test_msg_002",
                   "MD5OfMessageAttributes": "8ef4d60dbc8efda9f260e1dfd09d29f3",
                   "MD5OfMessageBody": "27118326006d3829667a400ad23d5d98",
                   "MessageId": "1dcfcd50-5a67-45ae-ae4c-1c152b5effb9"
                 }
              ]
            }
            """u8, SqsSerializerContext.Default.SendMessageBatchResult);

        Assert.NotNull(result);
        Assert.Empty(result.Failed);
        Assert.Equal(2, result.Successful.Length);

        var s0 = result.Successful[0];

        Assert.Equal("test_msg_001",                         s0.Id);
        Assert.Equal("f4eb349f-cd33-4bc4-bdc2-e557c900d41d", s0.MessageId);
        Assert.Equal("0e024d309850c78cba5eabbeff7cae71",     s0.MD5OfMessageBody);
    }
}