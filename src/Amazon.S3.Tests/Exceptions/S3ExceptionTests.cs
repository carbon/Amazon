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

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, true)]
    [InlineData(HttpStatusCode.ServiceUnavailable, true)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.Forbidden, false)]
    public void IsTransient(HttpStatusCode statusCode, bool expected)
    {
        Assert.Equal(expected, new S3Exception("message", statusCode).IsTransient);
    }

    [Fact]
    public void UsesErrorMessage()
    {
        var error = new S3Error { Code = "NoSuchKey", Message = "Not found" };

        var ex = new S3Exception(error, HttpStatusCode.NotFound);

        Assert.Same(error, ex.Error);
        Assert.Equal("Not found", ex.Message);
        Assert.Equal(HttpStatusCode.NotFound, ex.HttpStatusCode);
    }

    [Fact]
    public void KeepsErrorFromInnerException()
    {
        var error = new S3Error { Code = "SlowDown", Message = "Reduce your request rate" };

        var inner = new S3Exception(error, HttpStatusCode.ServiceUnavailable);

        var ex = new S3Exception("Error copying", inner, HttpStatusCode.ServiceUnavailable);

        Assert.Same(error, ex.Error);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void IgnoresOtherInnerExceptions()
    {
        var ex = new S3Exception("Error", new IOException("Broken pipe"), HttpStatusCode.BadGateway);

        Assert.Null(ex.Error);
    }
}