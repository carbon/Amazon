namespace Amazon.Ec2.Tests;

public class TerminateInstancesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new TerminateInstancesRequest(["a", "b", "c"]);

        Assert.Equal("Action=TerminateInstances&InstanceId.1=a&InstanceId.2=b&InstanceId.3=c", request.Serialize());

        request = new TerminateInstancesRequest("a", "b", "c");

        Assert.Equal("Action=TerminateInstances&InstanceId.1=a&InstanceId.2=b&InstanceId.3=c", request.Serialize());
    }
}