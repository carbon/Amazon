using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon
{
    public class InstanceRoleCredential : IAwsCredential
    {
        public InstanceRoleCredential() { }

        public InstanceRoleCredential(string roleName)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        internal InstanceRoleCredential(string roleName, IamSecurityCredentials credential)
            : this(roleName)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            #endregion

            AccessKeyId     = credential.AccessKeyId;
            SecretAccessKey = credential.SecretAccessKey;
            SecurityToken   = credential.Token;
            Expires         = credential.Expiration;
        }

        public string RoleName { get; internal set; }

        public string AccessKeyId { get; internal set; }

        public string SecretAccessKey { get; internal set; }

        public string SecurityToken { get; internal set; }

        public DateTime Expires { get; internal set; }

        public int RenewCount => renewCount;

        #region Helpers

        public TimeSpan ExpiresIn => Expires - DateTime.UtcNow;

        public bool IsExpired => DateTime.UtcNow < Expires;

        public bool ShouldRenew
        {
            get => RoleName == null || ExpiresIn <= TimeSpan.FromMinutes(5);
        }

        // aws: recomends refreshing 5 minutes before expiration

        #endregion

        private int renewCount = 0;

        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        public async Task<bool> RenewAsync()
        {
            if (RoleName == null)
            {
                RoleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false)
                    ?? throw new Exception("The instance is not configured with an IAM role");
            }

            // Lock so we only renew the credentials once
            await gate.WaitAsync().ConfigureAwait(false);

            try
            {
                if (ShouldRenew)
                {
                    var iamCredential = await IamSecurityCredentials.GetAsync(RoleName).ConfigureAwait(false);

                    AccessKeyId     = iamCredential.AccessKeyId;
                    SecretAccessKey = iamCredential.SecretAccessKey;
                    Expires         = iamCredential.Expiration;
                    SecurityToken   = iamCredential.Token;

                    Interlocked.Increment(ref renewCount);
                }

                return true;
            }
            finally
            {
                gate.Release();
            }
        }

        public static async Task<InstanceRoleCredential> GetAsync()
        {
            var roleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false);

            var iamCredential = await IamSecurityCredentials.GetAsync(roleName).ConfigureAwait(false);

            return new InstanceRoleCredential(roleName, iamCredential);
        }

        public static async Task<InstanceRoleCredential> GetAsync(string roleName)
        {
            var iamCredential = await IamSecurityCredentials.GetAsync(roleName).ConfigureAwait(false);

            return new InstanceRoleCredential(roleName, iamCredential);
        }
    }
}