using System.Buffers;

namespace Amazon.Cryptography;

public sealed class EncryptedMessage
{
    public EncryptedMessageHeader Header { get; init; } 

    // body
    public required List<EncryptedMessageFrame> Frames { get; init; }

    public byte[]? Signature { get; init; }

    public void WriteTo(IBufferWriter<byte> writer)
    {
        Header.WriteTo(writer);

        foreach (var frame in Frames)
        {
            frame.WriteTo(writer);
        }

        if (Signature != null)
        {
            writer.WriteUInt16((ushort)Signature.Length);
            writer.Write(Signature);
        }
    }

    public static EncryptedMessage Parse(ReadOnlySpan<byte> buffer)
    {
        var reader = new BufferReader(buffer);

        var header = EncryptedMessageHeader.Read(ref reader);

        var frames = new List<EncryptedMessageFrame>();
        EncryptedMessageFrame? frame;
        do
        {
            frame = EncryptedMessageFrame.Read((int)header.FrameLength, ref reader);
            frames.Add(frame);
        } 
        while (!frame.IsFinal);

        ReadOnlySpan<byte> signature = null;

        if (!reader.IsEof)
        {
            var signatureLength = reader.ReadUInt16();
            signature = reader.ReadBytes(signatureLength);
        }

        return new EncryptedMessage {
            Header = header,
            Frames = frames,
            Signature = signature.ToArray()
        };
    }
}