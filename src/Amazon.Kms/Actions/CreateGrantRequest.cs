using System.Linq;
using System.Runtime.Serialization;

namespace Amazon.Kms
{
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

        /// <summary>
        /// A friendly name for identifying the grant. Use this value to prevent unintended creation of duplicate grants when retrying this request.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        public string[] Operations { get; set; }
      

        [DataMember(EmitDefaultValue = false)]
        public string RetiringPrincipal { get; set; }
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
