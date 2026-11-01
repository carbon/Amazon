namespace Amazon.Kms;

public static class KmsOperations
{
    public const string Decrypt                             = "Decrypt";
    public const string Encrypt                             = "Encrypt";
    public const string GenerateDataKey                     = "GenerateDataKey";
    public const string GenerateDataKeyWithoutPlaintext     = "GenerateDataKeyWithoutPlaintext";
    public const string ReEncryptFrom                       = "ReEncryptFrom";
    public const string ReEncryptTo                         = "ReEncryptTo";
    public const string Sign                                = "Sign";
    public const string Verify                              = "Verify";
    public const string GetPublicKey                        = "GetPublicKey";
    public const string CreateGrant                         = "CreateGrant";
    public const string RetireGrant                         = "RetireGrant";
    public const string DescribeKey                         = "DescribeKey";
    public const string GenerateDataKeyPair                 = "GenerateDataKeyPair";
    public const string GenerateDataKeyPairWithoutPlaintext = "GenerateDataKeyPairWithoutPlaintext";
    public const string GenerateMac                         = "GenerateMac";
    public const string VerifyMac                           = "VerifyMac";
    public const string DeriveSharedSecret                  = "DeriveSharedSecret";
    public const string Encapsulate                         = "Encapsulate";
    public const string Decapsulate                         = "Decapsulate";
}