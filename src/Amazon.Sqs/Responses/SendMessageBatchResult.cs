using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.Sqs
{
    public class SendMessageBatchResult
    {
        public static List<SendMessageBatchResultEntry> Parse(string xmlText)
        {
            var list = new List<SendMessageBatchResultEntry>();

            var rootEl = XElement.Parse(xmlText);   // <SendMessageBatchResponse>
            var batchResultEl = rootEl.Element(SqsClient.NS + "SendMessageBatchResult");

            foreach (var entryEl in batchResultEl.Elements(SqsClient.NS + "SendMessageBatchResultEntry"))
            {
                list.Add(new SendMessageBatchResultEntry {
                    Id = entryEl.Element(SqsClient.NS + "Id").Value,
                    MessageId = entryEl.Element(SqsClient.NS + "MessageId").Value,
                    MD5OfMessageBody = entryEl.Element(SqsClient.NS + "MD5OfMessageBody").Value
                });
            }

            return list;
        }
    }

    public class SendMessageBatchResultEntry
    {
        public string Id { get; set; }

        public string MessageId { get; set; }

        public string MD5OfMessageBody { get; set; }
    }
}

/*
<SendMessageBatchResponse>
	<SendMessageBatchResult>
		<SendMessageBatchResultEntry>
			<Id>test_msg_001</Id>
			<MessageId>0a5231c7-8bff-4955-be2e-8dc7c50a25fa</MessageId>
			<MD5OfMessageBody>0e024d309850c78cba5eabbeff7cae71</MD5OfMessageBody>
		</SendMessageBatchResultEntry>
		<SendMessageBatchResultEntry>
			<Id>test_msg_002</Id>
			<MessageId>15ee1ed3-87e7-40c1-bdaa-2e49968ea7e9</MessageId>
			<MD5OfMessageBody>7fb8146a82f95e0af155278f406862c2</MD5OfMessageBody>
		</SendMessageBatchResultEntry>
	</SendMessageBatchResult>

	<ResponseMetadata>
		<RequestId>ca1ad5d0-8271-408b-8d0f-1351bf547e74</RequestId>
	</ResponseMetadata>
</SendMessageBatchResponse>
*/
