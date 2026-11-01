namespace Amazon.Sns;

public sealed class PublishResult(string messageId, string? sequenceNumber = null)
{
    public string MessageId { get; } = messageId;

    // FIFO topics only
    public string? SequenceNumber { get; } = sequenceNumber;

    internal static PublishResult Parse(byte[] utf8Xml)
    {
        var root = SnsXml.Parse(utf8Xml);

        return new PublishResult(
            messageId      : SnsXml.GetValue(root, "MessageId") ?? throw new FormatException("The response is missing MessageId"),
            sequenceNumber : SnsXml.GetValue(root, "SequenceNumber"));
    }
}
