using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class RunInstancesRequest : IEc2Request
{
#nullable disable
    public RunInstancesRequest() { }
#nullable enable

    public RunInstancesRequest(
        string imageId,
        string instanceType,
        int minCount = 1,
        int maxCount = 1,
        string? keyName = null,
        string? subnetId = null,
        string? userData = null,
        string? clientToken = null,
        InstanceMetadataOptionsRequest? metadataOptions = null,
        string[]? securityGroupIds = null)
    {
        if (minCount <= 0)
            throw new ArgumentException("Must be > 0", nameof(minCount));

        if (maxCount <= 0 || maxCount > 100)
            throw new ArgumentException("Must be between 1 and 100", nameof(maxCount));

        ImageId = imageId;
        InstanceType = instanceType;
        MinCount = minCount;
        MaxCount = maxCount;
        KeyName = keyName;

        SubnetId = subnetId;
        UserData = userData;
        ClientToken = clientToken;
        MetadataOptions = metadataOptions;
        SecurityGroupIds = securityGroupIds;
    }

    [DataMember(Name = "BlockDeviceMapping", Order = 1)]
    public BlockDeviceMapping[]? BlockDeviceMappings { get; set; }

    // Unique, case-sensitive identifier you provide to ensure the idempotency of the request. 
    [MaxLength(64)]
    [DataMember(Order = 2)]
    public string? ClientToken { get; set; }

    [DataMember(Order = 3)]
    public bool? DisableApiTermination { get; set; }

    [DataMember(Order = 4)]
    public bool? DryRun { get; set; }

    [DataMember(Order = 5)]
    public bool? EbsOptimized { get; set; }

    [DataMember(Order = 6)]
    public IamInstanceProfileSpecification? IamInstanceProfile { get; set; }

    // [Required]
    [DataMember(Order = 7)]
    public string ImageId { get; set; }

    // stop | terminate
    [DataMember(Order = 8)]
    public string? InstanceInitiatedShutdownBehavior { get; set; }

    // default = m1.small
    [DataMember(Order = 9)]
    public string? InstanceType { get; set; }

    [DataMember(Order = 10)]
    public int? Ipv6AddressCount { get; set; }

    [DataMember(Order = 11)]
    public string? KernelId { get; set; }

    [DataMember(Order = 12)]
    public string? KeyName { get; set; }

    [Range(1, 100)]
    [DataMember(Order = 13)]
    public int MaxCount { get; set; }

    [Range(1, 100)]
    [DataMember(Order = 14)]
    public int MinCount { get; set; }

    [DataMember(Order = 15)]
    public InstanceMetadataOptionsRequest? MetadataOptions { get; set; }

    [DataMember(Order = 16)]
    public RunInstancesMonitoringEnabled? Monitoring { get; set; }

    [DataMember(Order = 17)]
    public Placement? Placement { get; set; }

    [DataMember(Order = 18)]
    public string? PrivateIpAddress { get; set; }

    [DataMember(Name = "SecurityGroupId", Order = 19)]
    public string[]? SecurityGroupIds { get; set; }

    // [EC2-VPC] 
    // - If you don't specify a subnet ID, we choose a default subnet from your default VPC for you.
    // - If you don't have a default VPC, you must specify a subnet ID in the request.

    [DataMember(Order = 20)]
    public string? SubnetId { get; set; }

    [DataMember(Name = "TagSpecification", Order = 21)]
    public TagSpecification[]? TagSpecifications { get; set; }

    [DataMember(Order = 22)]
    public string? UserData { get; set; }

    public Dictionary<string, string> ToParams()
    {
        return Ec2RequestHelper.ToParams("RunInstances", this);
    }
}