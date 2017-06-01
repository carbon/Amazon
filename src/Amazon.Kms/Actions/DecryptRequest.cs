using System;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Kms
{
    public class DecryptRequest : KmsRequest
    {
        public DecryptRequest() { }

        public DecryptRequest(string keyId, byte[] ciphertext, JsonObject context)
        {
            KeyId             = keyId      ?? throw new ArgumentNullException(nameof(keyId));
            CiphertextBlob    = ciphertext ?? throw new ArgumentNullException(nameof(ciphertext));
            EncryptionContext = context;
        }

        public string KeyId { get; set; }

        // [MaxSize(6144)]
        public byte[] CiphertextBlob { get; set; }

        // String Map
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContext { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; set; }
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

/*
A unique identifier for the customer master key. 
This value can be a globally unique identifier, a fully specified ARN to either an alias or a key,
or an alias name prefixed by "alias/".

Key ARN Example - arn:aws:kms:us-east-1:123456789012:key/12345678-1234-1234-1234-123456789012
Alias ARN Example - arn:aws:kms:us-east-1:123456789012:alias/MyAliasName
Globally Unique Key ID Example - 12345678-1234-1234-1234-123456789012
Alias Name Example - alias/MyAliasName
*/
