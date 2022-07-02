namespace Amazon.Ec2.Tests;

public class StopInstancesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new StopInstancesRequest(new[] { "i-1234567890abcdef0" });

        Assert.Equal("Action=StopInstances&InstanceId.1=i-1234567890abcdef0", request.Serialize());
    }
}