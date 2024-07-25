namespace Amazon.Elb.Tests;

public class ErrorResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = ElbSerializer<ErrorResponse>.DeserializeXml(
            """
            <ErrorResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <Error>
                <Type>Sender</Type>
                <Code>InvalidTarget</Code>
                <Message>The following targets are not in a running state and cannot be registered: 'i-1'</Message>
              </Error>
              <RequestId>x</RequestId>
            </ErrorResponse>
            """u8.ToArray());

        Assert.Equal("Sender", response.Error.Type);
        Assert.Equal("InvalidTarget", response.Error.Code);
    }
}