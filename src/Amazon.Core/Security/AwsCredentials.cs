using System;
using System.Threading.Tasks;

namespace Amazon
{
    using Helpers;

    // AccessKey

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

        // 16 - 32 characters
        public string AccessKeyId { get; }

        public string SecretAccessKey { get; }

        public string SecurityToken => null;

        // {id}:{secret}

        public static AwsCredentials Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split(Seperators.Colon);

            return new AwsCredentials(parts[0], parts[1]);
        }

        #region Renewable

        public bool ShouldRenew => false;

        public Task<IAwsCredentials> RenewAsync()
        {
            throw new NotImplementedException("Not renewable");
        }

        #endregion
    }
}