namespace Amazon.Sqs.Tests;

public class SqsMessageTests
{
    [Fact]
    public void TypedMessageTests()
    {
        var m = new SqsMessage {
            Body = """{ "Position": 1, "Text": "hello" }"""
        };

        var b = JsonEncodedMessage<SampleMessage>.Create(m);

        Assert.Equal(1, b.Body.Position);
        Assert.Equal("hello", b.Body.Text);
    }
}

public class SampleMessage
{
    public int Position { get; set; }

    public required string Text { get; set; }
}