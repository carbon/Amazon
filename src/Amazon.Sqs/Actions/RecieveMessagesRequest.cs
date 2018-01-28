using System;

namespace Amazon.Sqs
{
    public class RecieveMessagesRequest
    {
        private readonly int take;
        private readonly TimeSpan? lockTime;
        private readonly TimeSpan? waitTime;

        /// <param name="lockTime">The time to hide the message from other workers.</param>
        /// <param name="waitTime">The maximium amount of time to wait for queue items.</param>
        public RecieveMessagesRequest(
            int take = 1,
            TimeSpan? lockTime = null, 
            TimeSpan? waitTime = null)
        {
            #region Preconditions

            if (take > 10)
            {
                throw new ArgumentException("Must be less than 10", nameof(take));
            }

            if (lockTime != null && lockTime.Value.TotalHours > 12)
            {
                throw new ArgumentException("Must be less than 12 hours ", nameof(lockTime));
            }

            if (waitTime != null && waitTime.Value.TotalSeconds > 20)
            {
                throw new ArgumentException("Must be less than 20 seconds ", nameof(waitTime));
            }

            #endregion

            this.take = take;
            this.lockTime = lockTime;
            this.waitTime = waitTime;
        }

        public SqsRequest ToParams()
        {
            var parameters = new SqsRequest {
                { "Action",              "ReceiveMessage" },
                { "MaxNumberOfMessages", take },
            };

            if (lockTime != null) // Defaults to the queue visibility timeout
            {
                parameters.Add("VisibilityTimeout", (int)lockTime.Value.TotalSeconds);
            }

            // Default: The ReceiveMessageWaitTimeSeconds of the queue.
            if (waitTime != null)
            {
                parameters.Add("WaitTimeSeconds", (int)waitTime.Value.TotalSeconds);
            }

            return parameters;
        }
    }
}

// specification: http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html