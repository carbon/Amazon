namespace Amazon.Cryptography;

public sealed class EncryptMessageRequest
{
    public required AlgorithmSuiteId AlgorithmId { get; init; }

    public required EncryptionContext EncryptionContext { get; init; }

    public uint FrameLength { get; } = 4096;

    public required byte[] Plaintext { get; init; }
}