namespace Amazon.Sqs.Tests;

public class ReceiveMessagesRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new ReceiveMessagesRequest(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));

        Assert.Null(request.AttributeNames);
        Assert.Null(request.MessageAttributeNames);

        var nvc = request.GetParameters();

        Assert.Equal(new("Action", "ReceiveMessage"), nvc[0]);
        Assert.Equal(new("MaxNumberOfMessages", "5"), nvc[1]);
        Assert.Equal(new("VisibilityTimeout", "10"),  nvc[2]);
        Assert.Equal(new("WaitTimeSeconds", "20"),    nvc[3]);
    }

    [Fact]
    public void ThrowsWhenTakeIsOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReceiveMessagesRequest(0,  TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReceiveMessagesRequest(11, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)));
    }
}