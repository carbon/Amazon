using System;

namespace Amazon.Kms
{
    [Flags]
    public enum KmsOperation
    {
        Decrypt = 1,
        Encrypt = 2,
        GenerateDataKey = 3,
        GenerateDataKeyWithoutPlaintext = 4,
        ReEncryptFrom = 5,
        ReEncryptTo = 6,
        CreateGrant = 7,
        RetireGrant = 8

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
