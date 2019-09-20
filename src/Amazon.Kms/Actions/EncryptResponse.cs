#nullable disable

namespace Amazon.Kms
{
    public sealed class EncryptResponse : KmsResponse
    {
        public byte[] CiphertextBlob { get; set; }

        public string KeyId { get; set; }
    }
}