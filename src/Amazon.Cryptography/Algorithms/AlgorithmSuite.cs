namespace Amazon.Cryptography.Algorithms;

public abstract partial class AlgorithmSuite
{
    public abstract AlgorithmSuiteId AlgorithmId { get; }

    public abstract byte MessageFormatVersion { get; }

    public abstract KeySize DataKeyLength { get; }

    public abstract KeyDerivationAlgorithmType? KeyDerivationAlgorithm { get; }

    public virtual SignatureAlgorithmType? SignatureAlgorithm => null;

    public virtual int AlgorithmSuiteDataLengthBytes { get; }

    public virtual void DeriveKey(ReadOnlySpan<byte> key, ReadOnlySpan<byte> messageId, Span<byte> derivedKey)
    {
        throw new NotImplementedException();
    }
}