namespace Amazon.Sts.Tests
{
    public class GetCallerIdentityRequestTests
    {
        private static readonly AwsCredential awsCredential = new AwsCredential("public", "private");

        [Fact]
        public async Task A()
        {
            var client = new StsClient(AwsRegion.USEast1, awsCredential);

            var q = await client.GetCallerIdentityVerificationParametersAsync();

            Assert.Equal("Action=GetCallerIdentity&Version=2011-06-15", q.Body);

            Assert.Equal(2, q.Headers.Count);

            Assert.NotNull(q.Headers["x-amz-date"]);
            Assert.NotNull(q.Headers["Authorization"]);

            var verifier = new CallerIdentityVerifier();

            var age = verifier.GetAge(q);

            Assert.True(age > TimeSpan.Zero);
        }
    }
}