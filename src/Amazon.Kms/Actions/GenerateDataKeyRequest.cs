using System;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Kms
{
    public class GenerateDataKeyRequest : KmsRequest
    {
        public GenerateDataKeyRequest() { }

        public GenerateDataKeyRequest(string keyId, KeySpec keySpec, JsonObject encryptionContext)
        {
            KeyId             = keyId ?? throw new ArgumentNullException(nameof(keyId));
            KeySpec           = keySpec;
            EncryptionContext = encryptionContext;
        }

        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContext { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; set; }

        [DataMember]
        public string KeyId { get; set; }

        [DataMember]
        public KeySpec KeySpec { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? NumberOfBytes { get; set; }
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
