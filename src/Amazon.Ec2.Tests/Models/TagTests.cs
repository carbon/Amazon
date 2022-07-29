namespace Amazon.Ec2.Models.Tests;

public class TagTests
{
    [Fact]
    public void CanConstruct()
    {
        var tag = new Tag("key", "value");

        Assert.Equal("key", tag.Key);
        Assert.Equal("value", tag.Value);
    }
}