using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Metadata
{
    internal static class InstanceMetadata
    {
        const string baseUri = "http://169.254.169.254/latest/meta-data";

        private static readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public static async Task<InstanceIdentity> GetIdentityAsync()
        {
            var text = await http.GetStringAsync("http://169.254.169.254/latest/dynamic/instance-identity/document");

            return JsonObject.Parse(text).As<InstanceIdentity>();
        }
        
        internal static async Task<IamSecurityCredentials> GetIamSecurityCredentials(string roleName)
        {
            if (roleName is null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var responseText = await http.GetStringAsync(
                requestUri: baseUri + "/iam/security-credentials/" + roleName
            ).ConfigureAwait(false);

            return JsonObject.Parse(responseText).As<IamSecurityCredentials>();            
        }

        // us-east-1a
        public static Task<string> GetAvailabilityZoneAsync()
        {
            return http.GetStringAsync(baseUri + "/placement/availability-zone");
        }

        public static Task<string> GetIamRoleName()
        {
            // TODO: Limit to first line...
            
            return http.GetStringAsync(baseUri + "/iam/security-credentials/");
        }

        private static async Task<IamRoleInfo> GetIamInfoAsync()
        {
            var url = baseUri + "/iam/info";

            var responseText = await http.GetStringAsync(url).ConfigureAwait(false);

            return JsonObject.Parse(responseText).As<IamRoleInfo>();
        }

        private static Task<string> GetCurrentRoleNameAsync()
        {
            return http.GetStringAsync(baseUri + "/iam/security-credentials/");
        }

        public static Task<string> GetInstanceIdAsync()
        {
            return http.GetStringAsync(baseUri + "/instance-id");
        }

        public static async Task<IPAddress> GetPublicIpAsync()
        {
            var result = await http.GetStringAsync(baseUri + "/public-ipv4");

            if (result.Length == 0) return null;

            return IPAddress.Parse(result);
        }

        public static Task<byte[]> GetUserDataAsync()
        {
            return http.GetByteArrayAsync(baseUri + "/user-data");
        }

        public static Task<string> GetUserDataStringAsync()
        {
            return http.GetStringAsync(baseUri + "/user-data");
        }
    }
}