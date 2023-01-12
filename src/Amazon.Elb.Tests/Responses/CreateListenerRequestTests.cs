namespace Amazon.Elb.Tests;

public class CreateListenerRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new CreateListenerRequest {
            LoadBalancerArn = "arn",
            Protocal = Protocol.TLS
        };

        Assert.Equal("Action=CreateListener&LoadBalancerArn=arn&Protocal=TLS", Serializer.Serialize(request));
    }
}
