namespace Amazon.Cryptography.Algorithms;

internal sealed class AES256_GCM_HKDF_SHA512_COMMIT_KEY : CommittingAlgorithmSuite
{
    public static readonly AES256_GCM_HKDF_SHA512_COMMIT_KEY Default = new();

    public override AlgorithmSuiteId AlgorithmId => AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;
}
