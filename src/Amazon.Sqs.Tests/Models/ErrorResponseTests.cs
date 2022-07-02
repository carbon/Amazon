namespace Amazon.Sqs.Models.Tests;

public class ErrorResponseTests
{
    [Fact]
    public void CanParse()
    {
        var result = ErrorResponse.ParseXml(
            """
            <?xml version="1.0"?>
                <ErrorResponse xmlns="http://queue.amazonaws.com/doc/2012-11-05/">
                <Error>
                    <Type>Sender</Type>
                    <Code>InvalidParameterValue</Code>
                    <Message>Value x for parameter ReceiptHandle is invalid. Reason: Message does not exist or is not available for visibility timeout change.</Message>
                    <Detail/>
                </Error>
                <RequestId>rid</RequestId>
            </ErrorResponse>
            """);

        Assert.Equal("Sender", result.Error.Type);
        Assert.Equal("InvalidParameterValue", result.Error.Code);
        Assert.StartsWith("Value x", result.Error.Message);
        Assert.Null(result.Error.Detail);
        Assert.Equal("rid", result.RequestId);
    }
}