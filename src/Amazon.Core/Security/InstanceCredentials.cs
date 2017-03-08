using System;
using System.Net.Http;
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

        public bool ShouldRenew => ExpiresIn <= TimeSpan.FromMinutes(5);

        public string SecurityToken => Token;

        public async Task<IAwsCredential> RenewAsync() => 
            await GetAsync(RoleName).ConfigureAwait(false);

        public static async Task<InstanceRoleCredential> GetAsync(string roleName)
        {
            #region Preconditions

            if (roleName == null)
                throw new ArgumentNullException(nameof(roleName));

            #endregion

            var url = "http://169.254.169.254/latest/meta-data/iam/security-credentials/" + roleName;

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using (var http = new HttpClient())
            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonObject.Parse(responseText).As<InstanceRoleCredential>();

                result.RoleName = roleName;

                return result;
            }
        }
    }
}

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
