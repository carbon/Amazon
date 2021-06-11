#nullable disable

namespace Amazon.Kms
{
    public sealed class EncryptResponse : KmsResponse
    {
        public byte[] CiphertextBlob { get; init; }

        public string KeyId { get; init; }
    }
}