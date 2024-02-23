namespace Amazon.Kms;

public sealed class RecipientInfo
{
    public byte[]? AttestationDocument { get; set; }

    // RSAES_OAEP_SHA_256
    public string? KeyEncryptionAlgorithm { get; set; }
}