namespace Amazon.Kms
{
    public class GenerateDataKeyResponse : KmsResponse 
    {
        public string KeyId { get; set; }

        public byte[] CiphertextBlob { get; set; }

        public byte[] Plaintext { get; set; }
    }
}