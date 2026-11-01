namespace Amazon.Scheduling;

public sealed class ExponentialBackoffRetryPolicy(
    TimeSpan initialDelay,
    TimeSpan maxDelay,
    int maxRetries = 3) : RetryPolicy
{
    private readonly TimeSpan _initialDelay = initialDelay;
    private readonly TimeSpan _maxDelay = maxDelay;
    private readonly int _maxRetries = maxRetries;

    public override bool ShouldRetry(int retryCount) => _maxRetries is -1 || _maxRetries > retryCount;

    public override TimeSpan GetDelay(int retryCount)
    {
        // Full jitter, as used by the AWS SDKs:
        // https://docs.aws.amazon.com/sdkref/latest/guide/feature-retry-behavior.html

        // R = A random number between 0 and 1
        // T = The initial delay
        // N = The retry number, starting at 1
        // M = The max delay
        // delay = R * MIN( T * 2 ^ (N - 1) , M )

        // Compare as doubles, since a large retry count would overflow a long
        double maxDelay = Math.Min(_maxDelay.Ticks, _initialDelay.Ticks * Math.Pow(2, retryCount - 1));

        long ticks = (long)(Random.Shared.NextDouble() * maxDelay);

        return TimeSpan.FromTicks(ticks);
    }
}