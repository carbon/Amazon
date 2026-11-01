namespace Amazon.Tests;

public class AwsServiceTests
{
    [Fact]
    public void NamesAreCorrect()
    {
        Assert.Equal("ec2",      AwsService.Ec2.Name);
        Assert.Equal("dynamodb", AwsService.DynamoDb.Name);
        Assert.Equal("s3",       AwsService.S3.Name);
        Assert.Equal("s3",       AwsService.S3.ToString());
        Assert.Equal("s3"u8,     AwsService.S3.Utf8Name);
    }

    [Fact]
    public void RequiresName()
    {
        Assert.Throws<ArgumentNullException>(() => new AwsService(null));
    }

    [Fact]
    public void ConvertsFromString()
    {
        AwsService service = "s3";

        Assert.Equal(AwsService.S3, service);
        Assert.Throws<ArgumentNullException>(() => { AwsService _ = (string)null; });
        Assert.Throws<ArgumentException>(() => { AwsService _ = ""; });
    }

    [Fact]
    public void ComparesByName()
    {
        AwsService s3 = new("s3");

        Assert.True(s3 == AwsService.S3);
        Assert.False(s3 != AwsService.S3);
        Assert.True(s3.Equals((object)AwsService.S3));
        Assert.False(s3.Equals((object)"s3"));
        Assert.False(s3.Equals(null));
        Assert.True(AwsService.S3 != AwsService.Ec2);
        Assert.Equal(AwsService.S3.GetHashCode(), s3.GetHashCode());
    }

    [Fact]
    public void ComparesWithNull()
    {
        AwsService service = null;

        Assert.True(service == null);
        Assert.False(service != null);
    }
}
