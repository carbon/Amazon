using System.Globalization;

namespace Amazon.Sqs
{
    public sealed class RecieveMessagesRequest
    {
        private readonly int _take;
        private readonly TimeSpan? _lockTime;
        private readonly TimeSpan? _waitTime;

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

            _take = take;
            _lockTime = lockTime;
            _waitTime = waitTime;
        }

        public string[]? AttributeNames { get; set; }

        public string[]? MessageAttributeNames { get; set; }

        internal List<KeyValuePair<string, string>> GetParameters()
        {
            var parameters = new List<KeyValuePair<string, string>>(4) {
                new ("Action", "ReceiveMessage")
            };

            if (_take > 1) // default is 1
            {
                parameters.Add(new ("MaxNumberOfMessages", _take.ToString(CultureInfo.InvariantCulture)));
            }

            if (AttributeNames is { Length: > 0 })
            {
                for (int i = 0; i < AttributeNames.Length; i++)
                {
                    parameters.Add(new ("AttributeName." + (i + 1).ToString(CultureInfo.InvariantCulture), AttributeNames[i]));
                }
            }

            if (MessageAttributeNames is { Length: > 0 })
            {
                for (int i = 0; i < MessageAttributeNames.Length; i++)
                {
                    parameters.Add(new ("MessageAttributeName." + (i + 1), MessageAttributeNames[i]));
                }
            }

            if (_lockTime.HasValue) // Defaults to the queue visibility timeout
            {
                int lockTimeSeconds = (int)_lockTime.Value.TotalSeconds;

                parameters.Add(new ("VisibilityTimeout", lockTimeSeconds.ToString(CultureInfo.InvariantCulture)));
            }

            if (_waitTime.HasValue) // Defaults to queue default
            {
                int waitTimeSeconds = (int)_waitTime.Value.TotalSeconds;

                parameters.Add(new ("WaitTimeSeconds", waitTimeSeconds.ToString(CultureInfo.InvariantCulture)));
            }

            return parameters;
        }
    }
}

// specification: http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html