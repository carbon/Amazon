using System.Text.Json;

using Carbon.Messaging;

namespace Amazon.Sqs.Tests;

public class MessageModelTests
{
    [Fact]
    public void AttributeValue_LongIsANumber()
    {
        MessageAttributeValue value = 5_000_000_000L;

        Assert.Equal(MessageAttributeDataType.Number, value.DataType);
        Assert.Equal("5000000000", value.StringValue);
        Assert.Null(value.BinaryValue);
        Assert.Equal("5000000000", new MessageAttributeValue(5_000_000_000L).StringValue);
    }

    [Fact]
    public void AttributeValue_IntIsANumber()
    {
        MessageAttributeValue value = -3;

        Assert.Equal(MessageAttributeDataType.Number, value.DataType);
        Assert.Equal("-3", value.StringValue);
    }

    [Fact]
    public void AttributeValue_StringIsAString()
    {
        MessageAttributeValue value = "text";

        Assert.Equal(MessageAttributeDataType.String, value.DataType);
        Assert.Equal("text", value.StringValue);
    }

    [Fact]
    public void AttributeValue_BytesAreBinary()
    {
        MessageAttributeValue value = new byte[] { 1, 2, 3 };

        Assert.Equal(MessageAttributeDataType.Binary, value.DataType);
        Assert.Equal([1, 2, 3], value.BinaryValue);
        Assert.Null(value.StringValue);
    }

    [Fact]
    public void SqsMessage_RequiresABody()
    {
        Assert.Throws<ArgumentNullException>(() => new SqsMessage(null!));
        Assert.Equal("hello", new SqsMessage("hello").Body);
    }

    [Fact]
    public void SqsMessage_ImplementsQueueMessage()
    {
        IQueueMessage<string> message = FakeSqs.Received("handle", "hello");

        Assert.Equal("id-handle", message.Id);
        Assert.Equal("handle", message.Receipt.Handle);
        Assert.Equal("hello", message.Body);
    }

    [Fact]
    public void JsonEncodedMessage_ExposesTheUnderlyingMessage()
    {
        var message = JsonEncodedMessage<QueueItem>.Create(FakeSqs.Received("handle", """{"Position":3}"""));

        Assert.Equal("id-handle", message.Id);
        Assert.Equal("handle", message.Receipt.Handle);
        Assert.Equal(3, message.Body.Position);
        Assert.Null(message.Body.Text);
    }

    [Fact]
    public void JsonEncodedMessage_ThrowsOnMalformedBody()
    {
        Assert.Throws<JsonException>(() => JsonEncodedMessage<QueueItem>.Create(FakeSqs.Received("handle", "not json")));
    }
}
