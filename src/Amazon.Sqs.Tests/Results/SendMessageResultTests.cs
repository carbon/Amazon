using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class SendMessageResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        SendMessageResult? result = JsonSerializer.Deserialize(
            """
            {
                "MD5OfMessageAttributes": "c48838208d2b4e14e3ca0093a8443f09",
                "MD5OfMessageBody": "fafb00f5732ab283681e124bf8747ed1",
                "MessageId": "770a44de-c6a7-41f8-88e9-02d4b48d185a"
            }
            """u8, SqsSerializerContext.Default.SendMessageResult);

        Assert.NotNull(result);
        Assert.Equal("c48838208d2b4e14e3ca0093a8443f09", result.MD5OfMessageAttributes);
        Assert.Equal("fafb00f5732ab283681e124bf8747ed1", result.MD5OfMessageBody);
        Assert.Equal("770a44de-c6a7-41f8-88e9-02d4b48d185a", result.MessageId);
    }
}