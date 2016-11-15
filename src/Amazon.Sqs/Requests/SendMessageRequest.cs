using System;

namespace Amazon.Sqs
{
    public class SendMessageRequest
    {
        private readonly string body;

        public SendMessageRequest(string body)
        {
            #region Preconditions

            if (body == null) throw new ArgumentNullException(nameof(body));

            #endregion

            this.body = body;
        }

        /// <summary>
        /// The number of seconds (0 to 900 - 15 minutes) to delay a specific message. 
        /// Messages with a positive DelaySeconds value become available for processing after the delay time is finished. 
        /// If you don't specify a value, the default value for the queue applies.
        /// </summary>
        public TimeSpan? Delay { get; set; }

        // TODO: Attributes

        public SqsRequest ToParams()
        {
            var parameters = new SqsRequest {
                { "Action", "SendMessage" },
                { "MessageBody", body }
            };

            // Defaults to the queue visibility timeout
            if (Delay != null)
            {
                parameters.Add("DelaySeconds", (int)Delay.Value.TotalSeconds);
            }

            return parameters;
        }

    }
}
