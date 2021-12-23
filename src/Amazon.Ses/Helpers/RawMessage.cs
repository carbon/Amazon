#nullable disable

namespace Amazon.Ses;

public sealed class RawMessage
{
    public RawMessage(byte[] data)
    {
        Data = data;
    }

    public byte[] Data { get; }
}