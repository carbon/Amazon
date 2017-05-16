using System;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Metadata
{
    public class InstanceIdentity
    {
        public string InstanceId { get; set; }

        public string AccountId { get; set; }

        public string ImageId { get; set; }

        public string InstanceType { get; set; }

        public string Architecture { get; set; }

        public string Region { get; set; }

        public string AvailabilityZone { get; set; }

        public string PrivateIp { get; set; }

        private static readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public static async Task<InstanceIdentity> GetAsync()
        {
            var text = await http.GetStringAsync("http://169.254.169.254/latest/dynamic/instance-identity/document");

            return JsonObject.Parse(text).As<InstanceIdentity>();
        }
    }
}