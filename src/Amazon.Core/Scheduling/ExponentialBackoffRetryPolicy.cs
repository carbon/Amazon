namespace Amazon.Scheduling;

public sealed class ExponentialBackoffRetryPolicy : RetryPolicy
{
    private readonly TimeSpan _initialDelay;
    private readonly TimeSpan _maxDelay;
    private readonly int _maxRetries;

    public ExponentialBackoffRetryPolicy(TimeSpan initialDelay, TimeSpan maxDelay, int maxRetries = 3)
    {
        _initialDelay = initialDelay;
        _maxDelay = maxDelay;
        _maxRetries = maxRetries;
    }

    public override bool ShouldRetry(int retryCount) => _maxRetries is -1 || _maxRetries > retryCount;

    public override TimeSpan GetDelay(int retryCount)
    {
        // R = A random number between 1 and 2
        // T = The initial timeout
        // F = The exponential factor (2)
        // N = Number of retries so far
        // M = Max Delay
        // delay = MIN( R * T * F ^ N , M )

        long exponentialDelay = (long)(_initialDelay.Ticks * Math.Pow(retryCount, 2));

        long ticks = Math.Min(_maxDelay.Ticks, exponentialDelay);

        return TimeSpan.FromTicks(ticks);
    }
}