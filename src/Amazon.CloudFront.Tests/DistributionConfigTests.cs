using Amazon.CloudFront.Serialization;

namespace Amazon.CloudFront.Tests;

public class DistributionConfigTests
{
    [Fact]
    public void CanSerialize()
    {
        var distribution = new DistributionConfig { CName = "test" };

        Assert.Equal(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <DistributionConfig xmlns="http://cloudfront.amazonaws.com/doc/2020-05-31/">
              <CNAME>test</CNAME>
              <Enabled>true</Enabled>
            </DistributionConfig>
            """u8, CloudFrontSerializer<DistributionConfig>.SerializeToUtf8Bytes(distribution));
    }
}