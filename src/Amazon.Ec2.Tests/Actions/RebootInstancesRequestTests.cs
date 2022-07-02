namespace Amazon.Ec2.Tests;

public class RebootInstancesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new RebootInstancesRequest(new[] { "i-1234567890abcdef0" }) {
            DryRun = true
        };

        Assert.Equal("Action=RebootInstances&DryRun=True&InstanceId.1=i-1234567890abcdef0", request.Serialize());
    }
}
