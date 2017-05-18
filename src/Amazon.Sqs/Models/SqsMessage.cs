using System;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    public class SqsMessage : IQueueMessage<string>
    {
        public SqsMessage() { }

        public SqsMessage(string body)
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public string Id { get; set; }

        public MessageReceipt Receipt { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }

        public int ApproximateReceiveCount { get; set; }

        // FIFO queues only (128 bits)
        public string SequenceNumber { get; set; }

        // TODO: Attributes
    }
}
