using System.Text.Json;

namespace Amazon.Metadata.Tests
{
    public sealed class InstanceIdentityTests
    {
        [Fact]
        public void Parse()
        {
            // From: https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/instance-identity-documents.html
            string json = @"{
    ""devpayProductCodes"" : null,
    ""marketplaceProductCodes"" : [ ""1abc2defghijklm3nopqrs4tu"" ], 
    ""availabilityZone"" : ""us-west-2b"",
    ""privateIp"" : ""10.158.112.84"",
    ""version"" : ""2017-09-30"",
    ""instanceId"" : ""i-1234567890abcdef0"",
    ""billingProducts"" : null,
    ""instanceType"" : ""t2.micro"",
    ""accountId"" : ""123456789012"",
    ""imageId"" : ""ami-5fb8c835"",
    ""pendingTime"" : ""2016-11-19T16:32:11Z"",
    ""architecture"" : ""x86_64"",
    ""kernelId"" : null,
    ""ramdiskId"" : null,
    ""region"" : ""us-west-2""
}";

            var result = JsonSerializer.Deserialize<InstanceIdentity>(json);

            Assert.Null(result.KernelId);
            Assert.Equal("i-1234567890abcdef0", result.InstanceId);
            Assert.Equal("t2.micro", result.InstanceType);
            Assert.Equal("123456789012", result.AccountId);
            Assert.Equal("ami-5fb8c835", result.ImageId);
            Assert.Equal("10.158.112.84", result.PrivateIp);
        }
    }
}