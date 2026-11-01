using System.Net;

namespace Amazon.Ses.Tests;

public class SesExceptionTests
{
    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, true)]
    [InlineData(HttpStatusCode.ServiceUnavailable, true)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.Forbidden, false)]
    public void IsTransient(HttpStatusCode statusCode, bool expected)
    {
        Assert.Equal(expected, new SesException("message", statusCode).IsTransient);
    }
}
