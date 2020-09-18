using Xunit;

namespace Amazon.S3.Actions.Tests
{
    public class RestoreObjectRequestTests
    {
        [Fact]
        public void Construct()
        {
            var request = new RestoreObjectRequest("s3.amazon.com", "bucket-name", "key", "version");

            Assert.Equal("/bucket-name/key?restore&versionId=version", request.RequestUri.PathAndQuery);
        }
    }
}