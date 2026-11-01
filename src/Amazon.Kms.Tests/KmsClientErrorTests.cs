using System.Net;

using Amazon.Exceptions;
using Amazon.Kms.Exceptions;
using Amazon.Scheduling;

namespace Amazon.Kms.Tests;

public class KmsClientErrorTests
{
    private static async Task<Exception> GetErrorAsync(string body, HttpStatusCode statusCode)
    {
        var client = FakeKms.CreateClient(new FakeKmsHandler(body, statusCode));

        return await Assert.ThrowsAnyAsync<Exception>(() => client.DecryptAsync(new DecryptRequest("key", [1], null)));
    }

    [Theory]
    [InlineData("AccessDeniedException", typeof(AccessDeniedException), HttpStatusCode.BadRequest)]
    [InlineData("KeyUnavailableException", typeof(KeyUnavailableException), HttpStatusCode.InternalServerError)]
    [InlineData("ValidationException", typeof(KmsValidationException), HttpStatusCode.BadRequest)]
    [InlineData("DryRunOperationException", typeof(DryRunOperationException), HttpStatusCode.BadRequest)]
    [InlineData("KMSInternalException", typeof(KmsInternalException), HttpStatusCode.InternalServerError)]
    [InlineData("DependencyTimeoutException", typeof(DependencyTimeoutException), HttpStatusCode.InternalServerError)]
    [InlineData("InvalidCiphertextException", typeof(KmsException), HttpStatusCode.BadRequest)]
    [InlineData("KMSInvalidStateException", typeof(KmsException), HttpStatusCode.BadRequest)]
    public async Task MapsErrorTypes(string type, Type exceptionType, HttpStatusCode statusCode)
    {
        Exception ex = await GetErrorAsync($$"""{"__type":"{{type}}","message":"details"}""", statusCode);

        Assert.IsType(exceptionType, ex);

        var kmsException = (KmsException)ex;

        Assert.Equal(type, kmsException.Type);
        Assert.Equal(type, kmsException.Error.Type);
        Assert.Equal("details", kmsException.Message);
        Assert.Equal(statusCode, kmsException.HttpStatusCode);
    }

    [Theory]
    [InlineData("AccessDeniedException", false)]
    [InlineData("ValidationException", false)]
    [InlineData("KeyUnavailableException", true)]
    [InlineData("ServiceUnavailableException", true)]
    public async Task ClassifiesTransientErrors(string type, bool isTransient)
    {
        Exception ex = await GetErrorAsync($$"""{"__type":"{{type}}","message":"details"}""", HttpStatusCode.InternalServerError);

        Assert.Equal(isTransient, Assert.IsAssignableFrom<IException>(ex).IsTransient);
    }

    [Fact]
    public async Task MapsServiceUnavailable()
    {
        Exception ex = await GetErrorAsync("""{"__type":"ServiceUnavailableException"}""", HttpStatusCode.ServiceUnavailable);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, Assert.IsType<ServiceUnavailableException>(ex).HttpStatusCode);
        Assert.Equal("Service unavailable", ex.Message);
    }

    [Fact]
    public async Task ServiceUnavailableKeepsMessage()
    {
        Exception ex = await GetErrorAsync("""{"__type":"ServiceUnavailableException","message":"details"}""", HttpStatusCode.ServiceUnavailable);

        Assert.Equal("details", ex.Message);
    }

    [Theory]
    [InlineData("com.amazon.coral.service#AccessDeniedException")]
    [InlineData("AccessDeniedException:http://internal.amazon.com/coral/com.amazon.coral.service/")]
    [InlineData("com.amazon.coral.service#AccessDeniedException:http://internal.amazon.com/coral/com.amazon.coral.service/")]
    public async Task MapsNamespacedErrorTypes(string type)
    {
        Exception ex = await GetErrorAsync($$"""{"__type":"{{type}}","message":"details"}""", HttpStatusCode.BadRequest);

        Assert.Equal(type, Assert.IsType<AccessDeniedException>(ex).Type);
    }

    [Fact]
    public async Task RetryableServiceErrorsAreTransient()
    {
        foreach (string type in (string[])["KMSInternalException", "DependencyTimeoutException"])
        {
            Exception ex = await GetErrorAsync($$"""{"__type":"{{type}}","message":"details"}""", HttpStatusCode.InternalServerError);

            Assert.True(Assert.IsAssignableFrom<IException>(ex).IsTransient);
        }
    }

    [Fact]
    public async Task ErrorWithoutMessageDescribesTypeAndStatus()
    {
        Exception ex = await GetErrorAsync("""{"__type":"NotFoundException"}""", HttpStatusCode.BadRequest);

        Assert.Equal("KMS exception - NotFoundException / BadRequest", ex.Message);
    }

    [Fact]
    public async Task ErrorWithoutTypeIsGeneric()
    {
        Exception ex = await GetErrorAsync("""{"message":"details"}""", HttpStatusCode.BadRequest);

        var kmsException = Assert.IsType<KmsException>(ex);

        Assert.Null(kmsException.Type);
        Assert.Equal("details", kmsException.Message);
    }

    [Fact]
    public async Task NonJsonErrorIsAwsException()
    {
        Exception ex = await GetErrorAsync("<html>Bad Gateway</html>", HttpStatusCode.BadGateway);

        var awsException = Assert.IsType<AwsException>(ex);

        Assert.Equal("<html>Bad Gateway</html>", awsException.Message);
        Assert.Equal(HttpStatusCode.BadGateway, awsException.HttpStatusCode);
    }

    [Fact]
    public async Task EmptyErrorIsAwsException()
    {
        Exception ex = await GetErrorAsync("", HttpStatusCode.InternalServerError);

        Assert.Equal(HttpStatusCode.InternalServerError, Assert.IsType<AwsException>(ex).HttpStatusCode);
    }

    [Theory]
    [InlineData("""{"__type":""")]
    [InlineData("""{"__type":1}""")]
    public async Task MalformedJsonErrorIsAwsException(string body)
    {
        Exception ex = await GetErrorAsync(body, HttpStatusCode.BadGateway);

        Assert.Equal(HttpStatusCode.BadGateway, Assert.IsType<AwsException>(ex).HttpStatusCode);
        Assert.Equal(body, ex.Message);
    }

    [Fact]
    public async Task OperationsWithoutResultThrowOnError()
    {
        var client = FakeKms.CreateClient(new FakeKmsHandler("""{"__type":"AccessDeniedException","message":"details"}""", HttpStatusCode.BadRequest));

        await Assert.ThrowsAsync<AccessDeniedException>(() => client.RetireGrantAsync(new RetireGrantRequest("token")));
        await Assert.ThrowsAsync<AccessDeniedException>(() => client.RevokeGrantAsync(new RevokeGrantRequest("key", "grant")));
    }
}
