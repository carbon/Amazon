namespace Amazon.Tests
{
    public class AwsServiceTests
    {
        [Fact]
        public void A()
        {
            Assert.Equal("s3",       AwsService.S3.Name);
            Assert.Equal("dynamodb", AwsService.DynamoDb.Name);
        }
    }
}