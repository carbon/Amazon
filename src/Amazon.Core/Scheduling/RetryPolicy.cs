namespace Amazon.Scheduling;

public abstract class RetryPolicy
{
    public abstract bool ShouldRetry(int retryCount);

    public abstract TimeSpan GetDelay(int retryCount);

    public static ExponentialBackoffRetryPolicy ExponentialBackoff(TimeSpan initialDelay, TimeSpan maxDelay, int maxRetries = 3)
    {
        return new ExponentialBackoffRetryPolicy(initialDelay, maxDelay, maxRetries);
    }
}