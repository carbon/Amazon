
using Xunit;

namespace Amazon.Security.Tests
{
    public class AwsCredentialTests
    {
        [Fact]
        public void Parse()
        {
            var accessKey = AwsCredential.Parse("a:b");

            Assert.Equal("a", accessKey.AccessKeyId);
            Assert.Equal("b", accessKey.SecretAccessKey);
        }
    }
}