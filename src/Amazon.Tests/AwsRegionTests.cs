namespace Amazon.Tests;

public class AwsRegionTests
{
    [Fact]
    public void AllRegionsAreUniqueAndResolvable()
    {
        Assert.Equal(27, AwsRegion.All.Length);

        foreach (var region in AwsRegion.All)
        {
            Assert.Same(region, AwsRegion.Get(region.Name));
        }

        var names = new HashSet<string>(AwsRegion.All.Select(static a => a.Name));

        Assert.Equal(AwsRegion.All.Length, names.Count);
    }

    [Fact]
    public void Get_CreatesUnknownRegions()
    {
        Assert.Equal("xx-east-9", AwsRegion.Get("xx-east-9").Name);
    }

    [Fact]
    public void RequiresName()
    {
        Assert.Throws<ArgumentNullException>(() => new AwsRegion(null));
    }

    [Fact]
    public void ExposesName()
    {
        Assert.Equal("us-east-1", AwsRegion.USEast1.ToString());
        Assert.Equal("us-east-1"u8, AwsRegion.USEast1.Utf8Name);
    }

    [Fact]
    public void ConvertsFromString()
    {
        AwsRegion region = "us-east-1";

        Assert.Equal(AwsRegion.USEast1, region);
        Assert.Throws<ArgumentNullException>(() => { AwsRegion _ = (string)null; });
        Assert.Throws<ArgumentException>(() => { AwsRegion _ = ""; });
    }

    [Fact]
    public void ComparesByName()
    {
        AwsRegion region = new("us-east-1");

        Assert.True(region == AwsRegion.USEast1);
        Assert.False(region != AwsRegion.USEast1);
        Assert.True(region.Equals((object)AwsRegion.USEast1));
        Assert.False(region.Equals((object)"us-east-1"));
        Assert.False(region.Equals(null));
        Assert.True(AwsRegion.USEast1 != AwsRegion.USEast2);
        Assert.False(AwsRegion.USEast1.Equals(AwsRegion.USEast2));
        Assert.Equal(AwsRegion.USEast1.GetHashCode(), region.GetHashCode());
    }

    [Fact]
    public void ComparesWithNull()
    {
        AwsRegion region = null;

        Assert.True(region == null);
        Assert.False(region != null);
    }

    [Theory]
    [InlineData("us-east-1a", "us-east-1")]
    [InlineData("eu-west-2c", "eu-west-2")]
    [InlineData("us-gov-west-1a", "us-gov-west-1")]
    [InlineData("us-east-1-wl1-bos-wlz-1", "us-east-1")]
    [InlineData("xx-east-12b", "xx-east-12")]
    public void FromAvailabilityZone(string zone, string region)
    {
        Assert.Equal(region, AwsRegion.FromAvailabilityZone(zone).Name);
    }

    [Fact]
    public void FromAvailabilityZone_LocalZone()
    {
        Assert.Same(AwsRegion.USWest2, AwsRegion.FromAvailabilityZone("us-west-2-lax-1a"));
    }
}
