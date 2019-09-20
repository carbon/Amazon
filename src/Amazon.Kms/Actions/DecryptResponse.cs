#nullable disable

namespace Amazon.Kms
{
    public sealed class DecryptResponse : KmsResponse
    {
        public string KeyId { get; set; }

        public byte[] Plaintext { get; set; }
    }
}

/*
{
   "EncryptionContext": 
    {
        "string" : "string"
    },
    "GrantTokens": [
        "string"
    ],
    "KeyId": "string",
    "Plaintext": blob
}
*/