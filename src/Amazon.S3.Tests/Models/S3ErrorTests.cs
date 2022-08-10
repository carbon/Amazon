namespace Amazon.S3.Models.Tests;

public class S3ErrorTests
{
    [Fact]
    public void CanDeserialize_NoSuchKey()
    {
        var error = S3Error.Deserialize(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <Error>
              <Code>NoSuchKey</Code>
              <Message>The resource you requested does not exist</Message>
              <Resource>/mybucket/myfoto.jpg</Resource> 
              <RequestId>4442587FB7D0A2F9</RequestId>
            </Error>
            """);

        Assert.Equal("NoSuchKey", error.Code);
        Assert.Equal("The resource you requested does not exist", error.Message);
        Assert.Equal("/mybucket/myfoto.jpg", error.Resource);
        Assert.Equal("4442587FB7D0A2F9", error.RequestId);
    }

    [Fact]
    public void CanDeserialize_BadDigest()
    {
        var error = S3Error.Deserialize(
            """
            <Error>
                <Code>BadDigest</Code>
                <Message>The Content-MD5 you specified did not match what we received.</Message>
                <RequestId>FE689C8C5E73D8B6</RequestId>
                <CalculatedDigest>1B2M2Y8AsgTpgAmY7PhCfg==</CalculatedDigest>
                <ExpectedDigest>d41d8cd98f00b204e9800998ecf8427e</ExpectedDigest>
                <HostId>3oDR1WldBiVFbMm4EKHrDb4W1haIG5Z8uXYwLuSc/Z1/YWupOApcCtnwmneoAYN4</HostId>
            </Error>
            """);

        Assert.Equal("BadDigest", error.Code);
        Assert.Equal("The Content-MD5 you specified did not match what we received.", error.Message);
        Assert.Null(error.Resource);
        Assert.Equal("FE689C8C5E73D8B6", error.RequestId);
    }

    [Fact]
    public void CanDeserialize_InvalidRange()
    {
        var error = S3Error.Deserialize(
            """
            <?xml version="1.0" encoding="UTF-8" ?> 
            <Error>
              <Code>InvalidRange</Code>
              <Message>The requested range is not satisfiable</Message>
              <RangeRequested>bytes=5242880-10485759</RangeRequested>
              <ActualObjectSize>39240</ActualObjectSize>
              <RequestId>QQRDR31F3YKX4763</RequestId>
              <HostId>udJqRzAlgPy4n2ia+yPm3OmLVEeV8bLq4HK2ExFSRp2F34G0mZ+SuG6FoG9d53XSpPrIWbCxaQ8=</HostId>
            </Error>
            """);

        Assert.Equal("InvalidRange", error.Code);
        Assert.Equal("The requested range is not satisfiable", error.Message);
        Assert.Null(error.Resource);
        Assert.Equal("QQRDR31F3YKX4763", error.RequestId);
        Assert.Equal("udJqRzAlgPy4n2ia+yPm3OmLVEeV8bLq4HK2ExFSRp2F34G0mZ+SuG6FoG9d53XSpPrIWbCxaQ8=", error.HostId);
    }

    [Fact]
    public void CanDeserialize_WasabiErrorResponse()
    {
        var response = S3Serializer<S3ErrorResponse>.Deserialize(
            """
            <ErrorResponse xmlns="https://iam.amazonaws.com/doc/2010-05-08/">
              <Error>
                <Type>Sender</Type>
                <Code>TemporarilyUnavailable</Code>
                <Message>Maximum number of server active requests exceeded</Message>
              </Error>
            </ErrorResponse>
            """);

        Assert.Equal("TemporarilyUnavailable", response.Error.Code);
        Assert.Equal("Maximum number of server active requests exceeded", response.Error.Message);
    }
}