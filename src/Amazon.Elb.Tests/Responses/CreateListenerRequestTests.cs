namespace Amazon.Elb.Tests;

public class CreateListenerRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new CreateListenerRequest {
            LoadBalancerArn = "arn",
            Protocol = Protocol.TLS
        };

        Assert.Equal("Action=CreateListener&LoadBalancerArn=arn&Protocol=TLS", Serializer.Serialize(request));
    }
}
