using System;
using System.Collections.Generic;
using System.Globalization;

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

            if (lockTime.HasValue && lockTime.Value.TotalHours > 12)
            {
                throw new ArgumentException("Must be less than 12 hours", nameof(lockTime));
            }

            if (waitTime.HasValue && waitTime.Value.TotalSeconds > 20)
            {
                throw new ArgumentException("Must be less than 20 seconds", nameof(waitTime));
            }

            this.take = take;
            this.lockTime = lockTime;
            this.waitTime = waitTime;
        }

        public string[]? AttributeNames { get; set; }

        public string[]? MessageAttributeNames { get; set; }

        internal List<KeyValuePair<string, string>> GetParameters()
        {
            var parameters = new List<KeyValuePair<string, string>> {
                new ("Action", "ReceiveMessage")
            };

            if (take > 1) // default is 1
            {
                parameters.Add(new ("MaxNumberOfMessages", take.ToString(CultureInfo.InvariantCulture)));
            }

            if (AttributeNames != null)
            {
                for (int i = 0; i < AttributeNames.Length; i++)
                {
                    parameters.Add(new ("AttributeName." + (i + 1).ToString(CultureInfo.InvariantCulture), AttributeNames[i]));
                }
            }

            if (MessageAttributeNames != null)
            {
                for (int i = 0; i < MessageAttributeNames.Length; i++)
                {
                    parameters.Add(new ("MessageAttributeName." + (i + 1), MessageAttributeNames[i]));
                }
            }

            if (lockTime.HasValue) // Defaults to the queue visibility timeout
            {
                parameters.Add(new ("VisibilityTimeout", ((int)lockTime.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture)));
            }

            if (waitTime.HasValue) // Defaults to queue default
            {
                parameters.Add(new ("WaitTimeSeconds", ((int)waitTime.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture)));
            }

            return parameters;
        }
    }
}

// specification: http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html