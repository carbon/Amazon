using System.Net;

using Amazon.Ec2.Exceptions;
using Amazon.Ec2.Responses;

namespace Amazon.Ec2.Tests.Responses;

public class ErrorResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = ErrorResponse.Deserialize(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <Response>
              <Errors>
                <Error>
                  <Code>InsufficientInstanceCapacity</Code>
                  <Message>We currently do not have sufficient c7g.2xlarge capacity in the Availability Zone you requested (us-east-1a). Our system will be working on provisioning additional capacity. You can currently get c7g.2xlarge capacity by not specifying an Availability Zone in your request or choosing us-east-1b, us-east-1c, us-east-1d, us-east-1f.</Message>
                </Error>
              </Errors>
              <RequestID>87557736-2c6d-4a3e-82a3-b6f9571908bf</RequestID>
            </Response>
            """);

        string message = "We currently do not have sufficient c7g.2xlarge capacity in the Availability Zone you requested (us-east-1a). Our system will be working on provisioning additional capacity. You can currently get c7g.2xlarge capacity by not specifying an Availability Zone in your request or choosing us-east-1b, us-east-1c, us-east-1d, us-east-1f.";

        Assert.Equal("87557736-2c6d-4a3e-82a3-b6f9571908bf", response.RequestId);

        Assert.Single(response.Errors);

        var error = response.Errors[0];

        Assert.Equal("InsufficientInstanceCapacity", error.Code);
        Assert.Equal(message, error.Message);

        var ex = new Ec2Exception(response.Errors, HttpStatusCode.InternalServerError);

        Assert.Equal(message, ex.Message);
    }
}