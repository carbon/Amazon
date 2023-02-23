namespace Amazon.Kms;

public enum EncryptionAlgorithm
{
    SYMMETRIC_DEFAULT  = 1,
    RSAES_OAEP_SHA_1   = 2,
    RSAES_OAEP_SHA_256 = 3,
    SM2PKE             = 4
}