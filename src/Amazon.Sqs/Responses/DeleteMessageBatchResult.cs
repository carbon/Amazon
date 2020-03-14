#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public sealed class DeleteMessageBatchResponse
    {
        [XmlElement("DeleteMessageBatchResult")]
        public DeleteMessageBatchResult DeleteMessageBatchResult { get; set; }

        public static DeleteMessageBatchResponse Parse(string xmlText)
        {
            return SqsSerializer<DeleteMessageBatchResponse>.Deserialize(xmlText);
        }
    }

    public sealed class DeleteMessageBatchResult
    {
        [XmlElement("DeleteMessageBatchResultEntry")]
        public DeleteMessageBatchResultEntry[] Items { get; set; }
    }

    public struct DeleteMessageBatchResultEntry
    {
        [XmlElement("Id")]
        public string Id { get; set; }
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
