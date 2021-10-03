using System.Net;

namespace Amazon.S3.Tests.Exceptions;

public class S3ExceptionTests
{
    [Fact]
    public void InternalServerErrorIsTransient()
    {
        var ex = new S3Exception("We encountered an internal error. Please try again.", HttpStatusCode.InternalServerError);

        Assert.True(ex.IsTransient);
    }
}