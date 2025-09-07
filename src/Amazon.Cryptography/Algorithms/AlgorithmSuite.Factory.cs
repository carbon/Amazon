namespace Amazon.Cryptography.Algorithms;

public partial class AlgorithmSuite
{
    public static AlgorithmSuite Get(AlgorithmSuiteId id)
    {
        return id switch {
            AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY => AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default,
            AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384 => AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384.Default,
            _ => throw new NotImplementedException()
        };
    }
}