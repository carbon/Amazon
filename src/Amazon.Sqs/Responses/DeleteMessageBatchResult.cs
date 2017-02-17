using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.Sqs
{
    public class DeleteMessageBatchResult
    {
        public static List<DeleteMessageBatchResultEntry> Parse(string xmlText)
        {
            var list = new List<DeleteMessageBatchResultEntry>();

            var rootEl = XElement.Parse(xmlText);   // <DeleteMessageBatchResponse>
            var batchResultEl = rootEl.Element(SqsClient.NS + "DeleteMessageBatchResult");

            foreach (var entryEl in batchResultEl.Elements(SqsClient.NS + "DeleteMessageBatchResultEntry"))
            {
                list.Add(new DeleteMessageBatchResultEntry(
                    id: entryEl.Element(SqsClient.NS + "Id").Value
                ));
            }

            return list;
        }
    }

    public struct DeleteMessageBatchResultEntry
    {
        public DeleteMessageBatchResultEntry(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}

/*
<DeleteMessageBatchResponse>
    <DeleteMessageBatchResult>
        <DeleteMessageBatchResultEntry>
            <Id>msg1</Id>
        </DeleteMessageBatchResultEntry>
        <DeleteMessageBatchResultEntry>
            <Id>msg2</Id>
        </DeleteMessageBatchResultEntry>
    </DeleteMessageBatchResult>
    <ResponseMetadata>
        <RequestId>d6f86b7a-74d1-4439-b43f-196a1e29cd85</RequestId>
    </ResponseMetadata>
</DeleteMessageBatchResponse>
*/
