using System;

namespace Amazon.Sqs
{
    public sealed class RecieveMessagesRequest
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
            if (take <= 0)
            {
                throw new ArgumentException("Must be greater than 0", nameof(take));
            }

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

            this.take = take;
            this.lockTime = lockTime;
            this.waitTime = waitTime;
        }

        public string[]? AttributeNames { get; set; }

        public string[]? MessageAttributeNames { get; set; }

        internal SqsRequest ToParams()
        {
            var parameters = new SqsRequest {
                { "Action", "ReceiveMessage" }
            };

            if (take > 1) // default is 1
            {
                parameters.Add("MaxNumberOfMessages", take);
            }

            if (AttributeNames != null)
            {

                for (int i = 0; i < AttributeNames.Length; i++)
                {
                    parameters.Add("AttributeName." + (i + 1), AttributeNames[i]);
                }
            }

            if (MessageAttributeNames != null)
            {
                for (int i = 0; i < MessageAttributeNames.Length; i++)
                {
                    parameters.Add("MessageAttributeName." + (i + 1), MessageAttributeNames[i]);
                }
            }

            if (lockTime != null) // Defaults to the queue visibility timeout
            {
                parameters.Add("VisibilityTimeout", (int)lockTime.Value.TotalSeconds);
            }

            if (waitTime != null) // Defaults to queue default
            {
                parameters.Add("WaitTimeSeconds", (int)waitTime.Value.TotalSeconds);
            }

            return parameters;
        }
    }
}

// specification: http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html