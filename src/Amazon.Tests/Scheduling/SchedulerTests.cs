using Amazon.Exceptions;

namespace Amazon.Scheduling.Tests;

public class SchedulerTests
{
    private static readonly RetryPolicy s_policy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromMilliseconds(1),
        maxDelay     : TimeSpan.FromMilliseconds(2),
        maxRetries   : 3);

    private sealed class PermanentException : Exception, IException
    {
        public bool IsTransient => false;
    }

    [Fact]
    public async Task ReturnsResultWithoutRetrying()
    {
        int calls = 0;

        var result = await s_policy.ExecuteAsync(() => { calls++; return Task.FromResult(5); });

        Assert.Equal(5, result);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task RetriesTransientExceptions()
    {
        int calls = 0;

        var result = await s_policy.ExecuteAsync(() => ++calls < 2
            ? throw new ServiceUnavailableException()
            : Task.FromResult(5));

        Assert.Equal(5, result);
        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task ThrowsLastExceptionWhenExhausted()
    {
        int calls = 0;

        var ex = await Assert.ThrowsAsync<ServiceUnavailableException>(() => s_policy.ExecuteAsync<int>(
            () => throw new ServiceUnavailableException((++calls).ToString())));

        Assert.Equal("3", ex.Message);
        Assert.Equal(3, calls);
    }

    [Fact]
    public async Task DoesNotRetryPermanentExceptions()
    {
        int calls = 0;

        await Assert.ThrowsAsync<PermanentException>(() => s_policy.ExecuteAsync<int>(() => { calls++; throw new PermanentException(); }));

        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task DoesNotRetryOtherExceptions()
    {
        int calls = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(() => s_policy.ExecuteAsync<int>(() => { calls++; throw new InvalidOperationException(); }));

        Assert.Equal(1, calls);
    }
}

public class ExponentialBackoffRetryPolicyTests
{
    [Fact]
    public void MaxRetriesLimitsAttempts()
    {
        var policy = RetryPolicy.ExponentialBackoff(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), maxRetries: 3);

        Assert.True(policy.ShouldRetry(1));
        Assert.True(policy.ShouldRetry(2));
        Assert.False(policy.ShouldRetry(3));
    }

    [Fact]
    public void NegativeOneRetriesForever()
    {
        var policy = new ExponentialBackoffRetryPolicy(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), maxRetries: -1);

        Assert.True(policy.ShouldRetry(int.MaxValue));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    [InlineData(6, 32)]
    public void DelayIsAtMostTwoToTheRetryMinusOne(int retryCount, int maxSeconds)
    {
        var policy = new ExponentialBackoffRetryPolicy(TimeSpan.FromSeconds(1), TimeSpan.FromDays(1));

        for (int i = 0; i < 100; i++)
        {
            Assert.InRange(policy.GetDelay(retryCount), TimeSpan.Zero, TimeSpan.FromSeconds(maxSeconds));
        }
    }

    [Fact]
    public void DelayUsesFullRange()
    {
        var policy = new ExponentialBackoffRetryPolicy(TimeSpan.FromSeconds(1), TimeSpan.FromDays(1));

        var delays = Enumerable.Range(0, 1_000).Select(_ => policy.GetDelay(1)).ToArray();

        Assert.Contains(delays, static delay => delay < TimeSpan.FromSeconds(0.25));
        Assert.Contains(delays, static delay => delay > TimeSpan.FromSeconds(0.75));
    }

    [Fact]
    public void DelayIsCappedBeforeJitter()
    {
        var policy = new ExponentialBackoffRetryPolicy(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));

        var delays = Enumerable.Range(0, 1_000).Select(_ => policy.GetDelay(10)).ToArray();

        Assert.All(delays, static delay => Assert.InRange(delay, TimeSpan.Zero, TimeSpan.FromSeconds(3)));
        Assert.Contains(delays, static delay => delay < TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void DelayDoesNotOverflow()
    {
        var policy = new ExponentialBackoffRetryPolicy(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

        Assert.InRange(policy.GetDelay(5_000), TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }
}
