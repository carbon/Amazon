namespace Amazon.Ec2.Models.Tests;

public class InstanceTypeTests
{
    [Fact]
    public void A()
    {
        Assert.Equal(70, InstanceTypeMap.All.Length);
    }
}