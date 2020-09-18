using Xunit;

namespace Amazon.S3.Actions.Tests
{
    public class ListVersionsRequestTests
    {
        [Fact]
        public void Construct()
        {
            var request = new ListVersionsRequest("s3.amazon.com", "bucket", new ListVersionsOptions { 
                Prefix = "apples",
                MaxKeys = 1000
            });

            Assert.Equal("/bucket?versions&prefix=apples&max-keys=1000", request.RequestUri.PathAndQuery);
        }
    }
}