using System;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    public class SqsMessage : IQueueMessage<string>
    {
        public SqsMessage() { }

        public SqsMessage(string body)
        {
            #region Preconditions

            if (body == null) throw new ArgumentNullException(nameof(body));

            #endregion

            Body = body;
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

    /*
	Body—The message's contents (not URL-encoded)
	MD5OfBody—An MD5 digest of the non-URL-encoded message body string.
	MessageId—The message's SQS-assigned ID.
	ReceiptHandle—A string associated with a specific instance of receiving the message.
	Attribute—SenderId, SentTimestamp, ApproximateReceiveCount, and/or ApproximateFirstReceiveTimestamp. SentTimestamp and ApproximateFirstReceiveTimestamp are each returned as an integer representing the epoch time in milliseconds.
	*/
}
