namespace Amazon.Metadata.Tests;

public class IamInfoTests
{
    [Fact]
    public void RegionTest()
    {
        var region = AwsRegion.FromAvailabilityZone("us-east-1a");

        Assert.Equal(AwsRegion.USEast1, region);
    }
}