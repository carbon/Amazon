using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Metadata;

namespace Amazon
{
    public sealed class InstanceRoleCredential : IAwsCredential
    {
        public InstanceRoleCredential() { }

        public InstanceRoleCredential(string roleName)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        internal InstanceRoleCredential(string roleName, IamSecurityCredentials credential)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));

            Set(credential);
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
            get => RoleName is null || Expires == default || ExpiresIn <= TimeSpan.FromMinutes(5);
        }

        private void Set(IamSecurityCredentials credential)
        {
            if (credential is null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            AccessKeyId     = credential.AccessKeyId;
            SecretAccessKey = credential.SecretAccessKey;
            SecurityToken   = credential.Token;
            Expires         = credential.Expiration;
        }

        #endregion

        private int renewCount = 0;

        private readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);

        // aws note: refresh 5 minutes before expiration

        public async Task<bool> RenewAsync()
        {
            if (!ShouldRenew) return false;

            // Lock so we only renew the credentials once
            if (!(await gate.WaitAsync(2000).ConfigureAwait(false)))
            {
                throw new TimeoutException("Timeout waiting for mutex to renew credentials (2s)");
            }

            try
            {
                if (ShouldRenew)
                {
                    if (RoleName is null)
                    {
                        RoleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false)
                            ?? throw new Exception("The instance is not configured with an IAM role");
                    }

                    var iamCredential = await IamSecurityCredentials.GetAsync(RoleName).ConfigureAwait(false);

                    Set(iamCredential);

                    Interlocked.Increment(ref renewCount);
                }
            }
            finally
            {
                gate.Release();
            }

            return true;
        }

        public static async Task<InstanceRoleCredential> GetAsync()
        {
            string roleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false);

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