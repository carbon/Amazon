namespace Amazon.S3.Tests
{
    public class GetPresignedUrlTests
    {
        private static readonly AwsCredential credential = new ("test", "test");

        [Fact]
        public void Get()
        {
            var date = new DateTime(2000, 01, 01, 00, 00, 00, DateTimeKind.Utc);

            string url = S3Helper.GetPresignedUrl(
                new GetPresignedUrlRequest("GET", "us-east-1.s3.aws.com", AwsRegion.USEast1, "test", "hi.txt", TimeSpan.FromMinutes(10)), 
                credential,
                date
            );

            Assert.Equal("https://us-east-1.s3.aws.com/test/hi.txt?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=test%2F20000101%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20000101T000000Z&X-Amz-Expires=600&X-Amz-SignedHeaders=host&X-Amz-Signature=55cb96b2f5a8c36b168294b3dc1081b939b1674ca89c97deced6e31b93ec5be2", url);
        }
      
        [Fact]
        public void StressGetPresignedUrl()
        {
            var date = new DateTime(2000, 01, 01, 00, 00, 00, DateTimeKind.Utc);
            string expect = "https://us-east-1.s3.aws.com/test/hi.txt?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=test%2F20000101%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20000101T000000Z&X-Amz-Expires=600&X-Amz-SignedHeaders=host&X-Amz-Signature=55cb96b2f5a8c36b168294b3dc1081b939b1674ca89c97deced6e31b93ec5be2";

            var request = new GetPresignedUrlRequest("GET", "us-east-1.s3.aws.com", AwsRegion.USEast1, "test", "hi.txt", TimeSpan.FromMinutes(10));

            // 2.2s

            for (int i = 0; i < 100_000; i++)
            {
                string url = S3Helper.GetPresignedUrl(request, credential, date);

                Assert.Equal(expect, url);
            }
        }

        [Fact]
        public void Put()
        {
            var date = new DateTime(2000, 01, 01, 00, 00, 00, DateTimeKind.Utc);

            string url = S3Helper.GetPresignedUrl(
                new GetPresignedUrlRequest("PUT", "us-east-1.s3.aws.com", AwsRegion.USEast1, "test", "hi.txt", TimeSpan.FromMinutes(10)),
                credential,
                date
            );

            Assert.Equal("https://us-east-1.s3.aws.com/test/hi.txt?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=test%2F20000101%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20000101T000000Z&X-Amz-Expires=600&X-Amz-SignedHeaders=host&X-Amz-Signature=e0fa58692735cb96ec84db30112ea47b3f798955bc9803f461698ef670b69c3a", url);
        }
    }
}