using Amazon.Sts.Serialization;

namespace Amazon.Sts.Responses.Tests;

public class GetCallerIdentityResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = StsXmlSerializer<GetCallerIdentityResponse>.Deserialize(
            """
            <GetCallerIdentityResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
              <GetCallerIdentityResult>
                <Arn>arn:aws:iam::123456789012:user/Alice</Arn>
                <UserId>AKIAI44QH8DHBEXAMPLE</UserId>
                <Account>123456789012</Account>
              </GetCallerIdentityResult>
              <ResponseMetadata>
                <RequestId>01234567-89ab-cdef-0123-456789abcdef</RequestId>
              </ResponseMetadata>
            </GetCallerIdentityResponse>
            """);

        var result = response.GetCallerIdentityResult;

        Assert.Equal("arn:aws:iam::123456789012:user/Alice", result.Arn);
        Assert.Equal("AKIAI44QH8DHBEXAMPLE",                 result.UserId);
        Assert.Equal("123456789012",                         result.Account);
    }
}