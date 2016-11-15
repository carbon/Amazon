using System;
using System.Threading.Tasks;

namespace Amazon.Scheduling
{
    public static class Scheduler
    {
        const string ExceededMaximumRetryCount = "The action has exceeded the maximum number of retries.";

        public static async Task<T> ExecuteAsync<T>(this RetryPolicy policy, Func<Task<T>> action)
        {
            Exception lastError;
            int retryCount = 0;

            do
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex) when (ex.IsTransient())
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(policy.GetDelay(retryCount)).ConfigureAwait(false);
            }
            while (policy.ShouldRetry(retryCount));

            // TODO: Throw aggregate exception

            throw lastError;
        }

        private static bool IsTransient(this Exception ex)
        {
            var failure = ex as IException;

            return (failure != null && failure.IsTransient);
        }
    }
}