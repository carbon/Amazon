namespace Amazon.Tests;

public class AwsRegionTests
{
    [Fact]
    public void Enum()
    {
        Assert.Equal(27, AwsRegion.All.Length);

        foreach (var region in AwsRegion.All)
        {
            Assert.Same(region, AwsRegion.Get(region.Name));
        }

        // Ensure all the regions are unique
        var names = new HashSet<string>(AwsRegion.All.Select(static a => a.Name));

        Assert.Equal(AwsRegion.All.Length, names.Count);
    }

    [Fact]
    public void A()
    {
        var usEast1 = AwsRegion.Get("us-east-1");
        var usEast2 = AwsRegion.Get("us-east-2");

        Assert.Same(AwsRegion.USEast1, usEast1);
        Assert.Same(AwsRegion.USEast2, usEast2);

        Assert.True(usEast1 != usEast2);

        Assert.Equal("us-east-1", usEast1.ToString());
        Assert.Equal("us-east-2", usEast2.ToString());

        Assert.Equal("us-east-1"u8, usEast1.Utf8Name);
    }

    [Fact]
    public void Equality()
    {
        Assert.True(AwsRegion.USEast1.Equals(AwsRegion.USEast1));
        Assert.False(AwsRegion.USEast1.Equals(AwsRegion.USEast2));
    }

    [Fact]
    public void AreSame()
    {
        Assert.Same(AwsRegion.USEast1,      AwsRegion.Get("us-east-1"));
        Assert.Same(AwsRegion.USEast2,      AwsRegion.Get("us-east-2"));
        Assert.Same(AwsRegion.USWest1,      AwsRegion.Get("us-west-1"));
        Assert.Same(AwsRegion.USWest2,      AwsRegion.Get("us-west-2"));
        Assert.Same(AwsRegion.CACentral1,   AwsRegion.Get("ca-central-1"));
        Assert.Same(AwsRegion.APSouth1,     AwsRegion.Get("ap-south-1"));
        Assert.Same(AwsRegion.APSouthEast1, AwsRegion.Get("ap-southeast-1"));
    }
}