using System;
using System.Threading.Tasks;

namespace Amazon
{
    public sealed class AwsCredential : IAwsCredential
    {
        public AwsCredential(string accessKeyId, string secretAccessKey)
        {
            AccessKeyId     = accessKeyId     ?? throw new ArgumentNullException(nameof(accessKeyId));
            SecretAccessKey = secretAccessKey ?? throw new ArgumentNullException(nameof(secretAccessKey));
        }

        // 16 - 32 characters
        public string AccessKeyId { get; }

        public string SecretAccessKey { get; }

        public string SecurityToken => null;

        // {id}:{secret}

        public static AwsCredential Parse(string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));

            var colonIndex = text.IndexOf(':');
            
            if (colonIndex == -1)
            {
                throw new Exception("AccessKeyId & SecretAccessKey must be seperated by ':'");
            }

            return new AwsCredential(
                accessKeyId     : text.Substring(0, colonIndex), 
                secretAccessKey : text.Substring(colonIndex + 1)
            );
        }

        #region Renewable

        public bool ShouldRenew => false;

        public Task<bool> RenewAsync() => throw new NotImplementedException("AwsCredential is not renewable");

        #endregion
    }
}