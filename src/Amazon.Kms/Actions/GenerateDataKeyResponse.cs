#nullable disable

namespace Amazon.Kms
{
    public sealed class GenerateDataKeyResponse : KmsResponse 
    {
        public string KeyId { get; set; }

        public byte[] CiphertextBlob { get; set; }

        public byte[] Plaintext { get; set; }
    }
}