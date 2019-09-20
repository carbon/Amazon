#nullable disable
namespace Amazon.Kms
{
    public sealed class CreateGrantResponse : KmsResponse
    {
        public string GrantId { get; set; }

        public string GrantToken { get; set; }
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