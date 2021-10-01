#nullable disable

using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amazon.Metadata;

public sealed class InstanceIdentity
{
    [JsonPropertyName("instanceId")]
    public string InstanceId { get; init; }

    [JsonPropertyName("accountId")]
    public string AccountId { get; init; }

    [JsonPropertyName("imageId")]
    public string ImageId { get; init; }

    [JsonPropertyName("instanceType")]
    public string InstanceType { get; init; }

    [JsonPropertyName("architecture")]
    public string Architecture { get; init; }

    [JsonPropertyName("region")]
    public string Region { get; init; }

    [JsonPropertyName("availabilityZone")]
    public string AvailabilityZone { get; init; }

    [JsonPropertyName("privateIp")]
    public string PrivateIp { get; init; }

#nullable enable

    [JsonPropertyName("kernelId")]
    public string? KernelId { get; set; }

    public static Task<InstanceIdentity> GetAsync()
    {
        return InstanceMetadataService.Instance.GetInstanceIdentityAsync();
    }
}
