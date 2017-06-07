using System;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon
{
    internal class IamSecurityCredentials
    {
        public string Code { get; set; }

        public string Type { get; set; }

        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string Token { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime Expiration { get; set; }

        const string baseUri = "http://169.254.169.254/latest/meta-data/iam/security-credentials/";

        private static readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(3)
        };

        public static async Task<IamSecurityCredentials> GetAsync(string roleName)
        {
            #region Preconditions

            if (roleName == null)
                throw new ArgumentNullException(nameof(roleName));

            #endregion
            
            var responseText = await http.GetStringAsync(
                requestUri: baseUri + roleName
            ).ConfigureAwait(false);

            var result = JsonObject.Parse(responseText).As<IamSecurityCredentials>();

            return result;
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