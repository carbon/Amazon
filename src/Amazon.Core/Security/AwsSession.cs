using System;
using System.Threading.Tasks;

namespace Amazon
{
    public sealed class AwsSession : IAwsCredential
    {
        public AwsSession(
            string sessionToken, 
            string secretAccessKey, 
            DateTime expiration, 
            string accessKeyId,
            string? securityToken = null)
        {
            SessionToken = sessionToken;
            SecretAccessKey = secretAccessKey;
            Expiration = expiration;
            AccessKeyId = accessKeyId;
            SecurityToken = securityToken;
        }

        public string SessionToken { get; }

        public string SecretAccessKey { get; }

        public string? SecurityToken { get; }

        public string AccessKeyId { get; }

        public DateTime Expiration { get;  }

        public bool ShouldRenew => false;

        public Task<bool> RenewAsync()
        {
            throw new NotImplementedException();
        }
    }
}