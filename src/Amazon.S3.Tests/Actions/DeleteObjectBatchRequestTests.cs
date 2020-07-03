using System.Net.Http;

using Xunit;

namespace Amazon.S3.Actions.Tests
{
    public class DeleteObjectBatchRequestTests
    {
        [Fact]
        public void Construct()
        {
            var batch = new DeleteBatch(new[] { "a", "b" });

            var request = new DeleteObjectBatchRequest("s3.amazon.com", "bucket", batch);

            Assert.Equal("https://s3.amazon.com/bucket?delete", request.RequestUri.ToString());
            Assert.Equal(HttpMethod.Post, request.Method);
        }
    }
}