namespace Amazon.Ec2.Tests;

public class TerminateInstancesRequestTests
{
    [Fact]
    public void Serialize()
    {
        var request = new TerminateInstancesRequest(new[] { "a", "b", "c" });

        var data = string.Join('&', request.ToParams().Select(a => a.Key + "=" + a.Value));

        Assert.Equal("Action=TerminateInstances&InstanceId.1=a&InstanceId.2=b&InstanceId.3=c", data);

        request = new TerminateInstancesRequest("a", "b", "c");

        data = string.Join('&', request.ToParams().Select(a => a.Key + "=" + a.Value));

        Assert.Equal("Action=TerminateInstances&InstanceId.1=a&InstanceId.2=b&InstanceId.3=c", data);
    }
}