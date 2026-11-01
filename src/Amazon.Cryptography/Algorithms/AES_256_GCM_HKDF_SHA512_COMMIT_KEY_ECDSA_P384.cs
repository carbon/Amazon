namespace Amazon.Cryptography.Algorithms;

internal sealed class AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384 : CommittingAlgorithmSuite
{
    public static readonly AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384 Default = new();

    public override AlgorithmSuiteId AlgorithmId => AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384;

    public override SignatureAlgorithmType? SignatureAlgorithm => SignatureAlgorithmType.ECDSA_P384_SHA384;
}
