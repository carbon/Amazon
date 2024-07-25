using Amazon.CloudFront.Serialization;

namespace Amazon.CloudFront.Tests;

public class InvalidationBatchTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new InvalidationBatch {
            Paths = new Paths {
                Items = ["a", "b"],
                Quantity = 2
            },
            CallerReference = "caller-reference"
        };

        Assert.Equal(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <InvalidationBatch xmlns="http://cloudfront.amazonaws.com/doc/2020-05-31/">
              <Paths>
                <Items>
                  <Path>a</Path>
                  <Path>b</Path>
                </Items>
                <Quantity>2</Quantity>
              </Paths>
              <CallerReference>caller-reference</CallerReference>
            </InvalidationBatch>
            """u8, CloudFrontSerializer<InvalidationBatch>.SerializeToUtf8Bytes(request));
    }
}