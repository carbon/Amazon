#nullable disable

namespace Amazon.Kms
{
    public sealed class GenerateDataKeyResponse : KmsResponse 
    {
        public string KeyId { get; init; }

        public byte[] CiphertextBlob { get; init; }

        public byte[] Plaintext { get; init; }
    }
}