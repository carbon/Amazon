using System;

namespace Amazon.Sqs
{
    public sealed class ChangeMessageVisibilityRequest
    {
        public ChangeMessageVisibilityRequest(string receiptHandle, TimeSpan visibilityTimeout)
        {
            if (visibilityTimeout < TimeSpan.Zero)
            {
                throw new ArgumentException("Must be greater than 0.", nameof(visibilityTimeout));
            }

            if (visibilityTimeout > TimeSpan.FromHours(12))
            {
                throw new ArgumentException("Must be less than 12 hours.", nameof(visibilityTimeout));
            }

            ReceiptHandle = receiptHandle ?? throw new ArgumentNullException(nameof(receiptHandle));
        }

        public string ReceiptHandle { get; }

        public TimeSpan VisibilityTimeout { get; }

        internal SqsRequest ToParams()
        {
            var parameters = new SqsRequest {
                { "Action", "ChangeMessageVisibility" },
                { "ReceiptHandle", ReceiptHandle },
                { "VisibilityTimeout", (int)VisibilityTimeout.TotalSeconds }
            };
            
            return parameters;
        }
    }
}
