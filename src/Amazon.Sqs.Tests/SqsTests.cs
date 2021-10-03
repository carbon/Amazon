namespace Amazon.Sqs.Tests
{
    public class SqsClientTests
    {
        [Fact]
        public void VersionTest()
        {
            Assert.Equal("2012-11-05", SqsClient.Version);
        }
    }
}