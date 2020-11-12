using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amazon.Metadata
{
    public sealed class InstanceIdentity
    {
#nullable disable

        [JsonPropertyName("instanceId")]
        public string InstanceId { get; set; }

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        
        [JsonPropertyName("imageId")]
        public string ImageId { get; set; }

        [JsonPropertyName("instanceType")]
        public string InstanceType { get; set; }

        [JsonPropertyName("architecture")]
        public string Architecture { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("availabilityZone")]
        public string AvailabilityZone { get; set; }

        [JsonPropertyName("privateIp")]
        public string PrivateIp { get; set; }

#nullable enable

        [JsonPropertyName("kernelId")]
        public string? KernelId { get; set; }

        public static Task<InstanceIdentity> GetAsync()
        {
            return InstanceMetadataService.Instance.GetInstanceIdentityAsync();
        }
    }
}