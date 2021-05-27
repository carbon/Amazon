using System;
using System.Collections.Generic;

namespace Amazon.Kms
{
    public sealed class EncryptRequest : KmsRequest
    {
        public EncryptRequest(
            string keyId,
            byte[] plaintext, 
            IReadOnlyDictionary<string, string>? context = null, 
            string[]? grantTokens = null)
        {
            if (keyId is null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (keyId.Length == 0)
            {
                throw new ArgumentNullException(nameof(keyId), "May not be empty");
            }

            if (plaintext is null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (plaintext.Length > 1024 * 4)
            {
                throw new ArgumentException("Must be less than 4KB", nameof(plaintext));
            }

            KeyId = keyId;
            Plaintext = plaintext;
            EncryptionContext = context;
            GrantTokens = grantTokens;
        }

        /// <summary>
        /// An encryption context is a key/value pair that you can pass to 
        /// AWS KMS when you call the Encrypt function.
        /// It is integrity checked but not stored as part of the ciphertext 
        /// that is returned. Although the encryption context is not literally 
        /// included in the ciphertext, it is cryptographically bound to the 
        /// ciphertext during encryption and must be passed again when you 
        /// call the Decrypt function. 
        /// Decryption will only succeed if the value you pass for decryption
        /// is exactly the same as the value you passed during encryption
        /// and the ciphertext has not been tampered with. 
        /// The encryption context is logged by using CloudTrail.
        /// </summary>
        public IReadOnlyDictionary<string, string>? EncryptionContext { get; }

        public string[]? GrantTokens { get; }

        public string KeyId { get; }

        public byte[] Plaintext { get; }
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
