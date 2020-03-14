
using System.Threading.Tasks;

namespace Amazon.Metadata
{
    public sealed class InstanceIdentity
    {
#nullable disable

        public string InstanceId { get; set; }

        public string AccountId { get; set; }

        public string ImageId { get; set; }

        public string InstanceType { get; set; }

        public string Architecture { get; set; }

        public string Region { get; set; }

        public string AvailabilityZone { get; set; }

        public string PrivateIp { get; set; }

#nullable enable

        public string? KernelId { get; set; }

        public static Task<InstanceIdentity> GetAsync()
        {
            return InstanceMetadataService.Instance.GetInstanceIdentityAsync();
        }
    }
}