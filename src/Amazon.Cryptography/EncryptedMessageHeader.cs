using System.Buffers;

using Amazon.Cryptography.Algorithms;

namespace Amazon.Cryptography;

public readonly struct EncryptedMessageHeader
{
    public required byte Version { get; init; }

    public required byte[] MessageId { get; init; }

    public required AlgorithmId AlgorithmId { get; init; }

    public required EncryptionContext EncryptionContext { get; init; }

    public required EncryptedDataKey[] EncryptedDataKeys { get; init; }

    // non-framed = 1
    // framed = 2s
    public byte ContentType { get; init; }

    public uint FrameLength { get; init; }

    public byte[]? AlgorithmSuiteData { get; init; }

    public byte[] AuthenticationTag { get; init; }

    public void WriteTo(IBufferWriter<byte> writer)
    {
        writer.Write([(byte)2]);
        writer.WriteUInt16((ushort)AlgorithmId);
        writer.Write(MessageId);

        writer.WriteUInt16(EncryptionContext.GetLength());
        EncryptionContext.WriteTo(writer);

        writer.WriteUInt16((ushort)EncryptedDataKeys.Length);

        foreach (var key in EncryptedDataKeys)
        {
            key.WriteTo(writer);
        }

        writer.Write([(byte)2]); // content type (framed)
        writer.WriteUInt32(FrameLength);

        if (AlgorithmSuiteData != null)
        {
            writer.Write(AlgorithmSuiteData);
        }

        writer.Write(AuthenticationTag);        
    }

    internal static EncryptedMessageHeader Read(ref BufferReader reader)
    {
        // Parse the message header
        byte version = reader.ReadByte();

        if (version != 2) // expect 2
        {
            throw new InvalidOperationException($"Invalid format version: {version}");
        }

        var algorithmId = (AlgorithmId)reader.ReadUInt16();
        var messageId = reader.ReadBytes(32).ToArray();

        ushort encryptionContextLength = reader.ReadUInt16();

        var encryptionContext = EncryptionContext.Parse(reader.ReadBytes(encryptionContextLength));
        var encryptedDataKeys = ReadEncryptedDataKeys(ref reader);
        var contentType = reader.ReadByte();
        var frameLength = reader.ReadUInt32();

        if (contentType != 2)
        {
            throw new Exception("ContentType must be 2 (framed)");
        }

        byte[]? algorithmSuiteData = null;

        var suite = AlgorithmSuite.Get(algorithmId);

        if (suite.AlgorithmSuiteDataLengthBytes is int algSuiteDataLengthBytes)
        {
            algorithmSuiteData = reader.ReadBytes(algSuiteDataLengthBytes).ToArray();
        }
                
        var authenticationTag = reader.ReadBytes(16).ToArray();
        
        // Alg Suite Data
        // Header authentication

        return new EncryptedMessageHeader {
            Version = version,
            AlgorithmId = algorithmId,
            MessageId = messageId,
            EncryptionContext = encryptionContext,
            EncryptedDataKeys = encryptedDataKeys,
            ContentType = contentType,
            FrameLength = frameLength,
            AlgorithmSuiteData = algorithmSuiteData,
            AuthenticationTag = authenticationTag
        };
    }

    private static EncryptedDataKey[] ReadEncryptedDataKeys(ref BufferReader reader)
    {
        var count = reader.ReadUInt16();

        var encryptedDataKeys = new EncryptedDataKey[count];

        for (int i = 0; i < count; i++)
        {
            encryptedDataKeys[i] = EncryptedDataKey.Read(ref reader);
        }

        return encryptedDataKeys;
    }
}