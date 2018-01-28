using System;

namespace Amazon.Scheduling
{
    public abstract class RetryPolicy
    {
        public abstract bool ShouldRetry(int retryCount);

        public abstract TimeSpan GetDelay(int retryCount);

        public static ExponentialBackoffRetryPolicy ExponentialBackoff(TimeSpan initialDelay, TimeSpan maxDelay, int maxRetries = 3)
        {
            return new ExponentialBackoffRetryPolicy(initialDelay, maxDelay);
        }
    }

    public sealed class ExponentialBackoffRetryPolicy : RetryPolicy
    {
        private readonly TimeSpan initialDelay;
        private readonly TimeSpan maxDelay;
        private readonly int maxRetries;

        public ExponentialBackoffRetryPolicy(TimeSpan initialDelay, TimeSpan maxDelay, int maxRetries = 3)
        {
            this.initialDelay = initialDelay;
            this.maxDelay   = maxDelay;
            this.maxRetries = maxRetries;
        }

        public override bool ShouldRetry(int retryCount) => maxRetries == -1 || maxRetries > retryCount;

        public override TimeSpan GetDelay(int retryCount)
        {
            // R = A random number between 1 and 2
            // T = The initial timeout
            // F = The exponential factor (2)
            // N = Number of retries so far
            // M = Max Delay
            // delay = MIN( R * T * F ^ N , M )

            long exponentialDelay = (long)(initialDelay.Ticks * Math.Pow(retryCount, 2));

            long ticks = Math.Min(maxDelay.Ticks, exponentialDelay);

            return TimeSpan.FromTicks(ticks);
        }
    }
}
