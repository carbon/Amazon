using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public readonly struct KinesisIterator : IIterator
{
    public KinesisIterator(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        Value = value;
    }

    public string Value { get; }
}