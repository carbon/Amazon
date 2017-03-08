using System;
using System.Threading.Tasks;

namespace Amazon
{
    using Helpers;

    // AccessKey

    public class AwsCredential : IAwsCredential
    {
        public AwsCredential(string accessKeyId, string secretAccessKey)
        {
            AccessKeyId = accessKeyId ?? throw new ArgumentNullException("accessKeyId");
            SecretAccessKey = secretAccessKey ?? throw new ArgumentNullException("secretAccessKey");
        }

        // 16 - 32 characters
        public string AccessKeyId { get; }

        public string SecretAccessKey { get; }

        public string SecurityToken => null;

        // {id}:{secret}

        public static AwsCredential Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split(Seperators.Colon);

            return new AwsCredential(parts[0], parts[1]);
        }

        #region Renewable

        public bool ShouldRenew => false;

        public Task<IAwsCredential> RenewAsync()
        {
            throw new NotImplementedException("Not renewable");
        }

        #endregion
    }
}