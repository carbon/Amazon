using System.Net;

using Amazon.Exceptions;
using Amazon.Sqs.Exceptions;

using Carbon.Messaging;

namespace Amazon.Sqs.Tests;

public class SqsClientErrorTests
{
    private static Task SendAsync(params Func<HttpResponseMessage>[] responses)
    {
        return FakeSqs.CreateClient(new FakeSqsHandler(responses)).SendMessageAsync(new SendMessageRequest(FakeSqs.QueueUrl, "hello"));
    }

    [Fact]
    public async Task JsonErrorBecomesSqsException()
    {
        var ex = await Assert.ThrowsAsync<SqsException>(() => SendAsync(FakeSqs.QueueDoesNotExist));

        Assert.Equal("com.amazonaws.sqs#QueueDoesNotExist", ex.Type);
        Assert.Equal("com.amazonaws.sqs#QueueDoesNotExist", ex.Error.Type);
        Assert.Equal("The specified queue does not exist.", ex.Message);
        Assert.False(ex.IsTransient);
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
    }

    [Theory]
    [InlineData("com.amazonaws.sqs#RequestThrottled", HttpStatusCode.BadRequest, true)]
    [InlineData("com.amazonaws.sqs#KmsThrottled", HttpStatusCode.BadRequest, true)]
    [InlineData("com.amazon.coral.availability#ThrottlingException", HttpStatusCode.Forbidden, true)]
    [InlineData("com.amazonaws.sqs#InternalFailure", HttpStatusCode.InternalServerError, true)]
    [InlineData("com.amazonaws.sqs#QueueDoesNotExist", HttpStatusCode.BadRequest, false)]
    [InlineData(null, HttpStatusCode.BadRequest, false)]
    public void IsTransient(string? type, HttpStatusCode status, bool expected)
    {
        Assert.Equal(expected, new SqsException(new ErrorResult { Type = type!, Message = "m" }, status).IsTransient);
    }

    [Fact]
    public void IsNotTransientWithoutAStatusCode()
    {
        Assert.False(new SqsException(new ErrorResult { Type = "t", Message = "m" }).IsTransient);
    }

    [Fact]
    public async Task ActionsWithoutAResultAlsoThrow()
    {
        var client = FakeSqs.CreateClient(new FakeSqsHandler(FakeSqs.QueueDoesNotExist));

        var ex = await Assert.ThrowsAsync<SqsException>(() => client.DeleteMessageAsync(new DeleteMessageRequest(FakeSqs.QueueUrl, "handle")));

        Assert.Equal("com.amazonaws.sqs#QueueDoesNotExist", ex.Type);
    }

    [Fact]
    public async Task ErrorMessageFieldIsCaseInsensitive()
    {
        var response = FakeSqs.Json("""{"__type":"com.amazon.coral.service#AccessDeniedException","Message":"Access to the resource is denied."}""", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<SqsException>(() => SendAsync(response));

        Assert.Equal("Access to the resource is denied.", ex.Message);
        Assert.Equal("com.amazon.coral.service#AccessDeniedException", ex.Type);
    }

    [Fact]
    public async Task ServiceUnavailableIsTransient()
    {
        var ex = await Assert.ThrowsAsync<ServiceUnavailableException>(() => SendAsync(FakeSqs.Unavailable));

        Assert.True(ex.IsTransient);
    }

    [Theory]
    [InlineData("<html>Forbidden</html>", "text/html", HttpStatusCode.Forbidden, "Forbidden")]
    [InlineData("null", "application/json", HttpStatusCode.BadRequest, "BadRequest")]
    [InlineData("[1,2]", "application/json", HttpStatusCode.BadRequest, "[1,2]")]
    public async Task UnparsableErrorBecomesQueueExceptionWithStatusAndBody(string text, string contentType, HttpStatusCode status, string expected)
    {
        var ex = await Assert.ThrowsAsync<QueueException>(() => SendAsync(FakeSqs.Content(text, contentType, status)));

        Assert.Contains($"status = {status}", ex.Message);
        Assert.Contains(expected, ex.Message);
    }

    [Theory]
    [InlineData("<html>Bad Gateway</html>", "text/html", HttpStatusCode.BadGateway)]
    [InlineData("", "text/plain", HttpStatusCode.GatewayTimeout)]
    [InlineData("null", "application/json", HttpStatusCode.InternalServerError)]
    public async Task UnparsableServerErrorIsTransient(string text, string contentType, HttpStatusCode status)
    {
        var ex = await Assert.ThrowsAsync<SqsException>(() => SendAsync(FakeSqs.Content(text, contentType, status)));

        Assert.True(ex.IsTransient);
        Assert.Equal(status, ex.StatusCode);
        Assert.Contains($"status = {status}", ex.Message);
        Assert.Contains(text, ex.Message);
    }

    [Fact]
    public async Task ErrorBodyMissingFieldsIsStillAnSqsException()
    {
        var ex = await Assert.ThrowsAsync<SqsException>(() => SendAsync(FakeSqs.Json("{}", HttpStatusCode.BadRequest)));

        Assert.Null(ex.Type);
        Assert.Null(ex.Error.Message);
    }
}
