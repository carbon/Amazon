namespace Amazon.Ec2.Tests;

public class DescribeVolumesRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new DescribeVolumesRequest(new[] { "a", "b", "c" });

        Assert.Equal("Action=DescribeVolumes&VolumeId.1=a&VolumeId.2=b&VolumeId.3=c", request.Serialize());
    }

    [Fact]
    public void CanSerializeEmpty()
    {
        var request = new DescribeVolumesRequest();

        Assert.Equal("Action=DescribeVolumes", request.Serialize());
    }
}