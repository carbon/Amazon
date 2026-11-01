using System.Diagnostics;
using System.Net;

using Amazon.Exceptions;

namespace Amazon.Sqs.Tests;

// These wait out the real retry delays, which can't be replaced from a test
public class SqsQueueRetryTests
{
    [Fact]
    public async Task Get_RetriesTransientErrors()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable, FakeSqs.Messages("hello"));

        var result = await FakeSqs.CreateQueue(handler).GetAsync(1, null);

        Assert.Equal("hello", Assert.Single(result).Body);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task Get_GivesUpAfterThreeAttempts()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable);

        await Assert.ThrowsAsync<ServiceUnavailableException>(() => FakeSqs.CreateQueue(handler).GetAsync(1, null));

        Assert.Equal(3, handler.Requests.Count);
    }

    [Fact]
    public async Task Get_CancellationStopsRetrying()
    {
        using var cts = new CancellationTokenSource();

        var handler = new FakeSqsHandler(() => {
            cts.Cancel();

            return FakeSqs.Unavailable();
        });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FakeSqs.CreateQueue(handler).GetAsync(1, null, cts.Token));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Get_DoesNotDelayAfterTheFinalAttempt()
    {
        var stopwatch = new Stopwatch();

        var handler = new FakeSqsHandler(() => {
            stopwatch.Restart();

            return FakeSqs.Unavailable();
        });

        await Assert.ThrowsAsync<ServiceUnavailableException>(() => FakeSqs.CreateQueue(handler).GetAsync(1, null));

        // A delay after the last attempt would be a random 0 to 2 seconds, so this catches it most of the time
        Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(0.5), $"took {stopwatch.Elapsed} after the last attempt");
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, """{"__type":"com.amazonaws.sqs#InternalFailure","message":"failed"}""")]
    [InlineData(HttpStatusCode.BadRequest, """{"__type":"com.amazonaws.sqs#RequestThrottled","message":"slow down"}""")]
    [InlineData(HttpStatusCode.BadGateway, "<html>Bad Gateway</html>")]
    public async Task Get_RetriesThrottlingAndServerErrors(HttpStatusCode status, string body)
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(body, status), FakeSqs.Messages("hello"));

        var result = await FakeSqs.CreateQueue(handler).GetAsync(1, null);

        Assert.Equal("hello", Assert.Single(result).Body);
    }

    [Fact]
    public async Task Delete_RetriesThrottling()
    {
        var handler = new FakeSqsHandler(
            FakeSqs.Json("""{"__type":"com.amazonaws.sqs#RequestThrottled","message":"slow down"}""", HttpStatusCode.BadRequest),
            FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateQueue(handler).DeleteAsync(FakeSqs.Received("h1"));

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task Delete_RetriesTransientErrors()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable, FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateQueue(handler).DeleteAsync(FakeSqs.Received("h1"));

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task Delete_GivesUpAfterThreeAttempts()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable);

        await Assert.ThrowsAsync<ServiceUnavailableException>(() => FakeSqs.CreateQueue(handler).DeleteAsync(FakeSqs.Received("h1")));

        Assert.Equal(3, handler.Requests.Count);
    }
}
