using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.Kms
{
    public abstract class KmsRequest { }

    public abstract class KmsResponse { }

    public class CreateGrantRequest : KmsRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public GrantConstraints Constraints { get; set; }

        /// <summary>
        /// There may a slight delay for a grant created in AWS KMS to take effect throughout a region.
        /// If you need to mitigate this delay, a grant token is a type of identifier that is 
        /// designed to let the permissions in the grant take effect immediately.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; set; }

        /// <summary>
        /// The principal that is given permission to perform the operations that the grant permits.
        /// </summary>
        public string GranteePrincipal { get; set; }

        public string KeyId { get; set; }

        public string[] Operations { get; set; }

        public void SetOperations(params KmsOperation[] ops)
        {
            Operations = ops.Select(o => o.ToString()).ToArray();
        }

        [DataMember(EmitDefaultValue = false)]
        public string RetiringPrincipal { get; set; }
    }

    public class GrantConstraints
    {
        // Match all
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextEquals { get; set; }

        // Match any
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextSubset { get; set; }
    }

    public class CreateGrantResponse : KmsResponse
    {
        public string GrantId { get; set; }

        public string GrantToken { get; set; }
    }

    public class RetireGrantRequest : KmsRequest
    {
        public string GrantId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GrantToken { get; set; }

        public string KeyId { get; set; }
    }

    public class RetireGrantResponse : KmsResponse
    {
        // 200
    }

    public class ListGrantsRequest : KmsRequest
    {
        public string KeyId { get; set; }

        public int Limit { get; set; }

        public string Marker { get; set; }
    }

    public class ListGrantsResponse : KmsResponse
    {
        public string NextMarker { get; set; }

        public bool Truncated { get; set; }

        public List<Grant> Grants { get; set; }
    }

    public class Grant
    {
        public string GrantId { get; set; }

        public GrantConstraints Constraints { get; set; }

        public string GranteePrincipal { get; set; }

        public string IssuingAccount { get; set; }

        public string[] Operations { get; set; }

        public string RetiringPrincipal { get; set; }
    }


    public class DecryptRequest : KmsRequest
    {
        // String Map
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContext { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; set; }

        public string KeyId { get; set; }

        // [MaxSize(6144)]
        public byte[] CiphertextBlob { get; set; }
    }

    public class DecryptResponse : KmsResponse
    {
        public string KeyId { get; set; }

        public byte[] Plaintext { get; set; }
    }

    public class EncryptRequest : KmsRequest
    {
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
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContext { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] GrantTokens { get; set; }

        public string KeyId { get; set; }

        public byte[] Plaintext { get; set; }
    }

    public class EncryptResponse : KmsResponse
    {
        public byte[] CiphertextBlob { get; set; }

        public string KeyId { get; set; }
    }

    public class GenerateDataKeyRequest : KmsRequest
    {
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

    public class GenerateDataKeyResponse : KmsResponse
    {
        public string KeyId { get; set; }

        public byte[] CiphertextBlob { get; set; }

        public byte[] Plaintext { get; set; }
    }

    [Flags]
    public enum KeySpec
    {
        AES_256,
        AES_128
    }

    [Flags]
    public enum KmsOperation
    {
        Decrypt = 1,
        Encrypt = 2,
        GenerateDataKey,
        GenerateDataKeyWithoutPlaintext,
        ReEncryptFrom,
        ReEncryptTo,
        CreateGrant,
        RetireGrant

    }

    // You can encrypt up to 4 KB of arbitrary data such as an RSA key,
    // a database password, or other sensitive customer information.

}

/*
{
   "EncryptionContext": 
    {
        "string" :  "string"
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
