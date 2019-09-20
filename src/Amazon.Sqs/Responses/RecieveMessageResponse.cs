#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public class ReceiveMessageResponse
    {
        [XmlElement("ReceiveMessageResult")]
        public ReceiveMessageResult ReceiveMessageResult { get; set; }

        public static ReceiveMessageResponse Parse(string xmlText)
        {
            return SqsSerializer<ReceiveMessageResponse>.Deserialize(xmlText);
        }
    }

    public class ReceiveMessageResult
    {
        [XmlElement("Message")]
        public SqsMessage[] Items { get; set; }
    }
}

/*
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
