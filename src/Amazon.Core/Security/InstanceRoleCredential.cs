using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon
{
    public class InstanceRoleCredential : IAwsCredential
    {
        public string Code { get; set; }

        public string Token { get; set; }

        public string SecretAccessKey { get; set; }

        public string Type { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime Expiration { get; set; }

        public string AccessKeyId { get; set; }

        public string RoleName { get; set; }

        public bool IsExpired => DateTime.UtcNow < Expiration;

        public TimeSpan ExpiresIn => Expiration - DateTime.UtcNow;

        public bool ShouldRenew
        {
            get => RoleName == null || ExpiresIn <= TimeSpan.FromMinutes(5);
        }

        public string SecurityToken => Token;


        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        public async Task<bool> RenewAsync()
        {
            if (RoleName == null)
            {
                RoleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false);

                if (RoleName == null)
                {
                    throw new Exception("The instance is not configured with an IAM role");
                }
            }

            // Lock so we only renew the credentials once
            await gate.WaitAsync().ConfigureAwait(false);

            try
            {
                if (ShouldRenew)
                {
                    var credential = await GetAsync(RoleName).ConfigureAwait(false);

                    AccessKeyId     = credential.AccessKeyId;
                    SecretAccessKey = credential.SecretAccessKey;
                    Expiration      = credential.Expiration;
                }

                return Code == "Success";
            }
            finally
            {
                gate.Release();
            }
        }

        const string baseUri = "http://169.254.169.254/latest/meta-data/";

        private static readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public static async Task<InstanceRoleCredential> GetAsync()
        {
            var roleName = await InstanceMetadata.GetIamRoleName().ConfigureAwait(false);

            return await GetAsync(roleName);
        }

        public static async Task<InstanceRoleCredential> GetAsync(string roleName)
        {
            #region Preconditions

            if (roleName == null)
                throw new ArgumentNullException(nameof(roleName));

            #endregion

            var url = baseUri + "iam/security-credentials/" + roleName;

            var responseText = await http.GetStringAsync(url).ConfigureAwait(false);

            var result = JsonObject.Parse(responseText).As<InstanceRoleCredential>();

            result.RoleName = roleName;

            return result;
        }
    }
}

// aws notes:
// - We recommend refreshing temporary credentials 5 minutes before their expiration.

/*
{
  "Code" : "Success",
  "LastUpdated" : "2012-04-26T16:39:16Z",
  "Type" : "AWS-HMAC",
  "AccessKeyId" : "AKIAIOSFODNN7EXAMPLE",
  "SecretAccessKey" : "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
  "Token" : "token",
  "Expiration" : "2012-04-27T22:39:16Z"
}
*/