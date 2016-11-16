using System;
using System.Xml.Linq;

namespace Amazon.Sqs.Models
{
    public class SendMessageResult
    {
        public string MD5OfMessageBody { get; set; }

        public string MessageId { get; set; }

        public static SendMessageResult Parse(string xmlText)
        {
            /* 
			<SendMessageResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
				<SendMessageResult>
					<MD5OfMessageBody>5d41402abc4b2a76b9719d911017c592</MD5OfMessageBody>
					<MessageId>cafaea9a-70f8-47c7-89b3-7bbb572cf061</MessageId>
				</SendMessageResult>
			</SendMessageResponse>
			*/

            try
            {
                var createQueueResponseEl = XElement.Parse(xmlText);

                var sendMessageResultEl = createQueueResponseEl.Element(SqsClient.NS + "SendMessageResult");

                return new SendMessageResult {
                    MD5OfMessageBody = sendMessageResultEl.Element(SqsClient.NS + "MD5OfMessageBody").Value,
                    MessageId = sendMessageResultEl.Element(SqsClient.NS + "MessageId").Value
                };
            }
            catch
            {
                throw new Exception(xmlText);
            }
        }
    }
}