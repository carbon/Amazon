namespace Amazon.Kms
{
    public class GenerateDataKeyWithoutPlaintextResponse : KmsResponse
    {
        public string KeyId { get; set; }

        public byte[] CiphertextBlob { get; set; }
    }
}