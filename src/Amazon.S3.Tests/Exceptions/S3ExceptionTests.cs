using System.Net;

namespace Amazon.S3.Exceptions.Tests;

public class S3ExceptionTests
{
    [Fact]
    public void InternalServerErrorIsTransient()
    {
        var ex = new S3Exception("We encountered an internal error. Please try again.", HttpStatusCode.InternalServerError);

        Assert.True(ex.IsTransient);
    }
}