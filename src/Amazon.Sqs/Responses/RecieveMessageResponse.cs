using System.Collections.Generic;
using System.Xml.Linq;

using Carbon.Messaging;

namespace Amazon.Sqs.Models
{
    public static class RecieveMessageResponse
    {
        public static IEnumerable<SqsMessage> Parse(string xmlText)
        {
            var rootEl = XElement.Parse(xmlText); // <ReceiveMessageResponse>

            var receiveMessageResultEl = rootEl.Element(SqsClient.NS + "ReceiveMessageResult");

            foreach (var messageEl in receiveMessageResultEl.Elements(SqsClient.NS + "Message"))
            {
                yield return new SqsMessage
                {
                    Id = messageEl.Element(SqsClient.NS + "MessageId").Value,
                    Receipt = new MessageReceipt(messageEl.Element(SqsClient.NS + "ReceiptHandle").Value),
                    Body = messageEl.Element(SqsClient.NS + "Body").Value
                };
            }
        }
    }
}

/*
<?xml version="1.0"?>
<ReceiveMessageResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
	<ReceiveMessageResult>
		<Message>
			<MessageId>cafaea9a-70f8-47c7-89b3-7bbb572cf061</MessageId>
			<ReceiptHandle>+eXJYhj5rDpOurU2Eha3YoaLyDumgmEYIq0cwOVvLNF0DJ3gVOmjI2Gh/oFnb0IeJqy5Zc8kH4KpI3G0WSKZVTaAPSeOkXQZmBjyKQ1KcgLLwRTbA9pIMFw+5YXIKlHX8bm2DxcU7Kvzk2IhYpaeWHeo7sOfxDW+gaHFpJpeRF8=</ReceiptHandle>
			<MD5OfBody>5d41402abc4b2a76b9719d911017c592</MD5OfBody>
			<Body>hello</Body>
		</Message>
	</ReceiveMessageResult>
	<ResponseMetadata>
		<RequestId>d72a6dbd-c23a-4ea1-b9b4-cdff8aec22e0</RequestId>
	</ResponseMetadata>
</ReceiveMessageResponse>
*/
