using System;
using System.Threading.Tasks;

namespace Amazon.Scheduling
{
    public static class Scheduler
    {
        public static async Task<T> ExecuteAsync<T>(this RetryPolicy policy, Func<Task<T>> action)
        {
            Exception lastError;
            int retryCount = 0;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(policy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex) when (ex.IsTransient())
                {
                    lastError = ex;
                }

                retryCount++;
            }
            while (policy.ShouldRetry(retryCount));

            // TODO: Throw aggregate exception

            throw lastError;
        }

        private static bool IsTransient(this Exception ex)
        {
            return ex is IException { IsTransient: true };
        }
    }
}