using System;
using System.Threading.Tasks;

namespace Amazon
{
    public class AwsCredentials : IAwsCredentials
    {
        public AwsCredentials(string accessKeyId, string secretAccessKey)
        {
            #region Preconditions

            if (accessKeyId == null)
                throw new ArgumentNullException("accessKeyId");

            if (secretAccessKey == null)
                throw new ArgumentNullException("secretAccessKey");

            #endregion

            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
        }

        public string AccessKeyId { get; }

        public string SecretAccessKey { get; }

        public string SecurityToken => null;

        #region Renewable

        public bool ShouldRenew => false;

        public Task<IAwsCredentials> RenewAsync()
        {
            throw new NotImplementedException("Not renewable");
        }

        #endregion
    }
}