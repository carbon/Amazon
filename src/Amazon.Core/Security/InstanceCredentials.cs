using System;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon
{
    public class InstanceRoleCredentials : IAwsCredentials
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

        public async Task<IAwsCredentials> RenewAsync()
            => await GetAsync(RoleName).ConfigureAwait(false);

        public static async Task<InstanceRoleCredentials> GetAsync(string roleName)
        {
            var url = "http://169.254.169.254/latest/meta-data/iam/security-credentials/" + roleName;

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            using (var http = new HttpClient())
            {
                using (var response = await http.SendAsync(httpRequest).ConfigureAwait(false))
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var json = JsonObject.Parse(responseText);

                    var result = json.As<InstanceRoleCredentials>();

                    result.RoleName = roleName;

                    return result;
                }
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
