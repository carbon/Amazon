namespace Amazon.Tests;

public class AwsServiceTests
{
    [Fact]
    public void NamesAreCorrect()
    {
        Assert.Equal("ec2",      AwsService.Ec2);
        Assert.Equal("dynamodb", AwsService.DynamoDb.Name);
        Assert.Equal("s3",       AwsService.S3.Name);
    }
}