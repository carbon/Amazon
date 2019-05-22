using System.Runtime.Serialization;

namespace Amazon.Kms
{
    public sealed class RetireGrantRequest : KmsRequest
    {
        public RetireGrantRequest() { }

        public RetireGrantRequest(string grantToken)
        {
            GrantToken = grantToken;
        }

        [DataMember(EmitDefaultValue = false)]
        public string GrantId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GrantToken { get; set; }

        public string KeyId { get; set; }
    }
}