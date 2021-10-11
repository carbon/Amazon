namespace Amazon.Ec2.Tests;

public class RebootInstancesRequestTests
{
    [Fact]
    public void Serialize()
    {
        var a = new RebootInstancesRequest(new[] { "i-1234567890abcdef0" })
        {
            DryRun = true
        };

        var result = string.Join('&', a.ToParams().Select(pair => $"{pair.Key}={pair.Value}"));

        Assert.Equal("Action=RebootInstances&DryRun=True&InstanceId.1=i-1234567890abcdef0", result);
    }
}
