using System;
using System.Collections.Generic;

namespace Amazon.Kms;

public sealed class GenerateDataKeyRequest : KmsRequest
{
#nullable disable
    public GenerateDataKeyRequest() { }
#nullable enable

    public GenerateDataKeyRequest(string keyId, KeySpec keySpec, IReadOnlyDictionary<string, string>? encryptionContext)
    {
        KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
        KeySpec = keySpec;
        EncryptionContext = encryptionContext;
    }

    public IReadOnlyDictionary<string, string>? EncryptionContext { get; init; }

    public string[]? GrantTokens { get; init; }

    public string KeyId { get; init; }

    public KeySpec KeySpec { get; init; }

    public int? NumberOfBytes { get; init; }
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
