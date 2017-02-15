using System;
using System.Threading.Tasks;

namespace Amazon
{
    public class AwsSession : IAwsCredentials
    {
        public string SessionToken { get; set; }

        public string SecretAccessKey { get; set; }

        public DateTime Expiration { get; set; }

        public string AccessKeyId { get; set; }

        public string SecurityToken { get; }

        public bool ShouldRenew => false;

        public Task<IAwsCredentials> RenewAsync()
        {
            throw new NotImplementedException();
        }
    }
}