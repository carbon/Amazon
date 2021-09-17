#nullable disable

namespace Amazon.Kms;

public sealed class DecryptResponse : KmsResponse
{
    public string KeyId { get; init; }

    public byte[] Plaintext { get; init; }
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