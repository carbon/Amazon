#nullable disable

namespace Amazon.Kms
{
    public sealed class GenerateDataKeyWithoutPlaintextResponse : KmsResponse
    {
        public string KeyId { get; set; }

        public byte[] CiphertextBlob { get; set; }
    }
}