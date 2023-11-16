namespace Amazon.Ec2.Tests;

public class StopInstancesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new StopInstancesRequest(["i-1234567890abcdef0"]);

        Assert.Equal("Action=StopInstances&InstanceId.1=i-1234567890abcdef0", request.Serialize());
    }
}