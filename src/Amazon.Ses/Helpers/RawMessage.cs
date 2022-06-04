namespace Amazon.Ses;

public readonly struct RawMessage
{
    public RawMessage(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Data = data;
    }

    public byte[] Data { get; }
}