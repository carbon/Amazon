namespace Amazon.DynamoDb.Models;

public sealed class StreamSpecification
{
    public StreamSpecification() { }

    public StreamSpecification(bool streamEnabled, StreamViewType streamViewType)
    {
        StreamEnabled = streamEnabled;
        StreamViewType = streamViewType;
    }

    public bool StreamEnabled { get; set; }

    public StreamViewType StreamViewType { get; set; }
}
