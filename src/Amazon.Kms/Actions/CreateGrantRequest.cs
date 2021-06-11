namespace Amazon.Kms
{
    public sealed class CreateGrantRequest : KmsRequest
    {
        public GrantConstraints? Constraints { get; init; }

        /// <summary>
        /// There may a slight delay for a grant created in AWS KMS to take effect throughout a region.
        /// If you need to mitigate this delay, a grant token is a type of identifier that is 
        /// designed to let the permissions in the grant take effect immediately.
        /// </summary>
        public string[]? GrantTokens { get; init; }

#nullable disable

        /// <summary>
        /// The principal that is given permission to perform the operations that the grant permits.
        /// </summary>
        public string GranteePrincipal { get; init; }

        public string KeyId { get; init; }

#nullable enable

        /// <summary>
        /// A friendly name for identifying the grant. Use this value to prevent unintended creation of duplicate grants when retrying this request.
        /// </summary>
        public string? Name { get; init; }

#nullable disable
        public string[] Operations { get; init; }

#nullable enable

        public string? RetiringPrincipal { get; init; }
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

EXAMPLES
Key ARN                | arn:aws:kms:us-east-1:123456789012:key/12345678-1234-1234-1234-123456789012
Alias ARN              | arn:aws:kms:us-east-1:123456789012:alias/MyAliasName
Globally Unique Key ID | 12345678-1234-1234-1234-123456789012
Alias Name             | alias/MyAliasName
*/
