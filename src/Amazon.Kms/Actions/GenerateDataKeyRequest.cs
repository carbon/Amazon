using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Amazon.Kms
{
    public sealed class GenerateDataKeyRequest : KmsRequest
    {
#nullable disable
        public GenerateDataKeyRequest() { }
#nullable enable

        public GenerateDataKeyRequest(string keyId, KeySpec keySpec, IReadOnlyDictionary<string, string>? encryptionContext)
        {
            KeyId             = keyId ?? throw new ArgumentNullException(nameof(keyId));
            KeySpec           = keySpec;
            EncryptionContext = encryptionContext;
        }

        public IReadOnlyDictionary<string, string>? EncryptionContext { get; set; }

        public string[]? GrantTokens { get; set; }

        public string KeyId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public KeySpec KeySpec { get; set; }

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
