using System;

namespace Amazon.Sqs
{
    public sealed class SendMessageRequest
    {
        public SendMessageRequest(string body)
        {
            MessageBody = body ?? throw new ArgumentNullException(nameof(body));
        }

        public SendMessageRequest(string body, string messageDeduplicationId, string messageGroupId)
        {
            MessageBody = body ?? throw new ArgumentNullException(nameof(body));
            MessageDeduplicationId = messageDeduplicationId;
            MessageGroupId = messageGroupId;
        }

        public string MessageBody { get; }

        // 128 characters
        public string? MessageDeduplicationId { get; }

        // Required for FIFO queues
        public string? MessageGroupId { get; }

        /// <summary>
        /// The number of seconds (0 to 900 - 15 minutes) to delay a specific message. 
        /// Messages with a positive DelaySeconds value become available for processing after the delay time is finished. 
        /// If you don't specify a value, the default value for the queue applies.
        /// </summary>
        public TimeSpan? Delay { get; set; }

        public MessageAttribute[]? MessageAttributes { get; set; }

        internal SqsRequest GetParameters()
        {
            var parameters = new SqsRequest {
                { "Action", "SendMessage" },
                { "MessageBody", MessageBody }
            };

            // Defaults to the queue visibility timeout
            if (Delay != null)
            {
                parameters.Add("DelaySeconds", (int)Delay.Value.TotalSeconds);
            }

            if (MessageDeduplicationId != null)
            {
                parameters.Add("MessageDeduplicationId", MessageDeduplicationId);
            }

            if (MessageGroupId != null)
            {
                parameters.Add("MessageGroupId", MessageGroupId);
            }

            if (MessageAttributes != null)
            {
                for (int i = 0; i < MessageAttributes.Length; i++)
                {
                    ref MessageAttribute attr = ref MessageAttributes[i];

                    string prefix = "MessageAttribute." + (i + 1).ToString() + ".";

                    parameters.Add(prefix + "Name", attr.Name);

                    if (attr.Value.DataType == MessageAttributeDataType.Binary)
                    {
                        parameters.Add(prefix + "Value.BinaryValue", attr.Value.Value);
                    }
                    else
                    {
                        parameters.Add(prefix + "Value.StringValue", attr.Value.Value);
                    }

                    parameters.Add(prefix + "Value.DataType", attr.Value.DataType.ToString());
                }
            }

            return parameters;
        }
    }

}
