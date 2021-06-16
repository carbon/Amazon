#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public sealed class DeleteMessageBatchResponse
    {
        [XmlElement("DeleteMessageBatchResult")]
        public DeleteMessageBatchResult DeleteMessageBatchResult { get; init; }

        public static DeleteMessageBatchResponse Parse(string xmlText)
        {
            return SqsSerializer<DeleteMessageBatchResponse>.Deserialize(xmlText);
        }
    }

    public sealed class DeleteMessageBatchResult
    {
        [XmlElement("DeleteMessageBatchResultEntry")]
        public DeleteMessageBatchResultEntry[] Items { get; init; }
    }

    public readonly struct DeleteMessageBatchResultEntry
    {
        [XmlElement("Id")]
        public string Id { get; init; }
    }
}