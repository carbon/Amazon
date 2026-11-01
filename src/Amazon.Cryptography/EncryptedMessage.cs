using System.Buffers;

using Amazon.Cryptography.Algorithms;
using Amazon.Cryptography.Buffers;

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
        uint expectedSequenceNumber = 1;
        do
        {
            frame = EncryptedMessageFrame.Read(header.FrameLength, ref reader);

            if (frame.SequenceNumber != expectedSequenceNumber)
            {
                throw new FormatException($"Expected frame sequence number {expectedSequenceNumber}. Was {frame.SequenceNumber}");
            }

            expectedSequenceNumber++;
            frames.Add(frame);
        } 
        while (!frame.IsFinal);

        byte[]? signature = null;

        if (AlgorithmSuite.Get(header.AlgorithmId).SignatureAlgorithm is not null)
        {
            signature = reader.ReadUInt16PrefixedBytes().ToArray();

            if (signature.Length is 0)
            {
                throw new FormatException("Signature must not be empty");
            }
        }

        if (!reader.IsEof)
        {
            throw new FormatException("Unexpected data after the end of the message");
        }

        return new EncryptedMessage {
            Header = header,
            Frames = frames,
            Signature = signature
        };
    }
}