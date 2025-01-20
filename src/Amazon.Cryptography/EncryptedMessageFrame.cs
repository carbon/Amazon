using System.Buffers;

namespace Amazon.Cryptography;

public sealed class EncryptedMessageFrame
{
    // Framed data must start at sequence number 1. Subsequent frames must be in order and must contain an increment of 1 of the previous frame.
    // Otherwise, the decryption process stops and reports an error.
    public required uint SequenceNumber { get; init; }

    public required byte[] IV { get; init; }

    // Variable. Equal to the value specified in the Frame Length of the header.
    public required byte[] EncryptedContent { get; init; }

    // Variable. Determined by the algorithm used, as specified in the Algorithm ID of the header.
    public required byte[] AuthenticationTag { get; init; }

    public bool IsFinal { get; init; }

    public static EncryptedMessageFrame Read(int defaultFrameLength, ref BufferReader reader)
    {
        var sequenceNumber = reader.ReadUInt32();
        bool isFinal = sequenceNumber is uint.MaxValue; // FFFFFFFF
        ReadOnlySpan<byte> iv;
        ReadOnlySpan<byte> encryptedContent;
        ReadOnlySpan<byte> authenticationTag;
        
        if (isFinal)
        {
            sequenceNumber = reader.ReadUInt32();
            iv = reader.ReadBytes(12);
            var encryptedContentLength = reader.ReadUInt32();
            encryptedContent = reader.ReadBytes((int)encryptedContentLength);
        }
        else
        {
            iv = reader.ReadBytes(12);
            encryptedContent = reader.ReadBytes(defaultFrameLength);
        }

        authenticationTag = reader.ReadBytes(16);

        return new EncryptedMessageFrame {
            SequenceNumber = sequenceNumber,
            IV = iv.ToArray(),
            EncryptedContent = encryptedContent.ToArray(),
            AuthenticationTag = authenticationTag.ToArray(),
            IsFinal = isFinal
        };
    }

    internal void WriteTo(IBufferWriter<byte> writer)
    {
        if (IsFinal)
        {
            writer.WriteUInt32(uint.MaxValue); // final frame indicator
            writer.WriteUInt32(SequenceNumber);
            writer.Write(IV);
            writer.WriteUInt32((uint)EncryptedContent.Length);
            writer.Write(EncryptedContent);
        }
        else
        {
            writer.WriteUInt32(SequenceNumber);
            writer.Write(IV);
            writer.Write(EncryptedContent);
        }

        writer.Write(AuthenticationTag);
    }
}