using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Kms
{
    public class GrantConstraints
    {
        // Match all
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextEquals { get; set; }

        // Match any
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextSubset { get; set; }
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
