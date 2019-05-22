using System;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Kms
{
    public sealed class DecryptRequest : KmsRequest
    {
        public DecryptRequest(string keyId, byte[] ciphertext, JsonObject context, string[] grantTokens = null)
        {
            if (keyId is null)
            {
                throw new ArgumentException(nameof(keyId));
            }

            if (keyId.Length == 0)
            {
                throw new ArgumentException("May not be empty", nameof(keyId));
            }

            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            if (ciphertext.Length == 0)
            {
                throw new ArgumentException("May not be empty", nameof(ciphertext));
            }

            KeyId = keyId;
            CiphertextBlob = ciphertext;
            EncryptionContext = context;
            GrantTokens = grantTokens;
        }

        public string KeyId { get; }

        // [MaxSize(6144)]
        public byte[] CiphertextBlob { get; }

        // String Map
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContext { get; }

        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; }
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
